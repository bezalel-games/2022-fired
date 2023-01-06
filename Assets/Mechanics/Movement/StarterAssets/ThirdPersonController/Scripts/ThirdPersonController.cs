using Avrahamy;
using UnityEngine;
using Logger = Nemesh.Logger;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


namespace StarterAssets
{

    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : OptimizedBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float moveSpeed = 4.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float sprintSpeed = 5.335f;

        [Tooltip("Walking speed of the character in m/s")]
        [SerializeField]
        private float walkSpeed = 2.0f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float rotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float speedChangeRate = 10.0f;

        public AudioClip landingAudioClip;
        public AudioClip[] footstepAudioClips;

        [Range(0, 1)]
        public float footstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float jumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float jumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float fallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool grounded = true;

        [Tooltip("Useful for rough ground")]
        public float groundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float groundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask groundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject cinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float topClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float bottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float cameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool lockCameraPosition = false;

        [SerializeField]
        [Tooltip("Check if the player always faces the same direction as the camera")]
        private bool playerFollowCamera = true;

        [SerializeField]
        private PlayerAnimatorParameters playerAnimatorParameters;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private Vector3 _targetDirection;
        private Vector3 _directionVelocity;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private bool _hasAnimator;
        private bool IsCurrentDeviceMouse => _input.UsingMouseToLook;

        private const float Threshold = 0.01f;

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

            // AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = jumpTimeout;
            _fallTimeoutDelta = fallTimeout;
        }


        private void Update()
        {
            if (Time.timeScale == 0f)  // TODO: just send some sort of bool from menu in the LockCameraPosition function
            {
                return;
            }
            _hasAnimator = TryGetComponent(out _animator);  // TODO: do only once at start!
            
            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            var position = transform.position;
            Vector3 spherePosition = new Vector3(position.x, position.y - groundedOffset, position.z);

            grounded = Physics.CheckSphere(spherePosition,
                groundedRadius,
                groundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                playerAnimatorParameters.groundedParameter.Set(_animator, grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= Threshold && !lockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

            // Cinemachine will follow this target
            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(
                _cinemachineTargetPitch + cameraAngleOverride,
                _cinemachineTargetYaw,
                0.0f
            );
        }

        private void Move()
        {
            float targetSpeed = _input.sprint ? sprintSpeed : _input.walk ? walkSpeed : moveSpeed;

            if (_input.move == Vector2.zero)
                targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed,
                    targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed * inputMagnitude;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (_animationBlend < 0.01f)
                _animationBlend = 0f;

            // normalise input direction
            var inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            _targetDirection = Vector3.SmoothDamp(_targetDirection, inputDirection, ref _directionVelocity, 0.2f);

            if (playerFollowCamera || _input.move != Vector2.zero)
            {
                if (playerFollowCamera)
                {
                    _targetRotation = _mainCamera.transform.eulerAngles.y;
                }
                else
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      _mainCamera.transform.eulerAngles.y;
                }

                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    _targetRotation,
                    ref _rotationVelocity,
                    rotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            var targetRotationOrig = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                     _mainCamera.transform.eulerAngles.y;

            // var target = Quaternion.Euler(0.0f, targetRotationOrig, 0.0f) * Vector3.forward;
            // _targetDirection = Vector3.SmoothDamp(_targetDirection, target, ref _directionVelocity, 0.01f);
            var targetDirection = Quaternion.Euler(0.0f, targetRotationOrig, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity * Time.deltaTime, 0.0f));

            // update animator if using character
            if (_hasAnimator)
            {
                var r = _rotationVelocity;
                r = ClampAngle(_rotationVelocity * rotationSmoothTime, -180, 180);
                playerAnimatorParameters.directionXParameter.Set(_animator, _targetDirection.x);
                playerAnimatorParameters.directionZParameter.Set(_animator, _targetDirection.z);
                playerAnimatorParameters.rotationParameter.Set(_animator, r);
                playerAnimatorParameters.analogSpeedParameter.Set(_animator, _speed);
                playerAnimatorParameters.motionSpeedParameter.Set(_animator, inputMagnitude);
                playerAnimatorParameters.useAnalogParameter.Set(_animator, _input.analogMovement);
                playerAnimatorParameters.sprintingParameter.Set(_animator, _input.sprint);
                playerAnimatorParameters.walkingParameter.Set(_animator, _input.walk);
            }
        }

        private void JumpAndGravity()
        {
            if (grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = fallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    playerAnimatorParameters.jumpingParameter.Set(_animator, false);
                    playerAnimatorParameters.fallingParameter.Set(_animator, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        playerAnimatorParameters.jumpingParameter.Set(_animator, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = jumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        playerAnimatorParameters.fallingParameter.Set(_animator, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f)
                lfAngle += 360f;
            if (lfAngle > 360f)
                lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.color = grounded ? transparentGreen : transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            var position = transform.position;
            Gizmos.DrawSphere(new Vector3(position.x, position.y - groundedOffset, position.z), groundedRadius);
        }

        #region Animation Event Callbacks

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (footstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, footstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(
                        footstepAudioClips[index],
                        transform.TransformPoint(_controller.center),
                        footstepAudioVolume
                    );
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            AudioSource.PlayClipAtPoint(
                landingAudioClip,
                transform.TransformPoint(_controller.center),
                footstepAudioVolume
            );
        }

        #endregion

        public void LockCameraPosition(bool state) => lockCameraPosition = state;

    }
}
