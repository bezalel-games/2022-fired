using StarterAssets;
using UnityEngine;

public class MovementAnimationControl : MonoBehaviour
{
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.12f;

    [SerializeField]
    private PlayerAnimatorParameters playerAnimatorParameters;
    
    private Animator _animator;
    private bool _hasAnimator;
    private Vector3 _targetDirection;

    public bool Walk { get; set; }

    public float Speed { get; set; }

    public Vector3 TargetDirection
    {
        get => _targetDirection;
        set => _targetDirection = new Vector3(value.x, 0, value.z).normalized;
    }

    public float RotationVelocity { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _hasAnimator = TryGetComponent(out _animator);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasAnimator)
        {
            return;
        }

        var r = RotationVelocity;
        r = ClampAngle(RotationVelocity * rotationSmoothTime, -180, 180);
        playerAnimatorParameters.directionXParameter.Set(_animator, TargetDirection.x);
        playerAnimatorParameters.directionZParameter.Set(_animator, TargetDirection.z);
        playerAnimatorParameters.rotationParameter.Set(_animator, r);
        playerAnimatorParameters.analogSpeedParameter.Set(_animator, Speed);
        playerAnimatorParameters.walkingParameter.Set(_animator, Walk);
        playerAnimatorParameters.motionSpeedParameter.Set(_animator, 1f);  // TODO: to setter/getter
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f)
            lfAngle += 360f;
        if (lfAngle > 360f)
            lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            // TODO: add Audio clip and play footstep
            return;
        }
    }
}
