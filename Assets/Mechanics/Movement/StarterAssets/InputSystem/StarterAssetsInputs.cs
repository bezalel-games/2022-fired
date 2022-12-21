using System;
using BitStrap;
using Nemesh;
using UnityEditor;
using UnityEngine;
using Logger = Nemesh.Logger;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour, StarterAssetsActions.IPlayerActions
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool walk;

        [field: ReadOnly]
        [field: SerializeField]
        [field: InspectorName("Using Mouse to Look")]
        public bool UsingMouseToLook { get; set; }
        // {
        // 	get => useMouse;
        // 	private set => useMouse = value;
        // }

        [Header("Movement Settings")]
        public bool analogMovement;

        [SerializeField]
        private bool sprintToggle = true;

        [SerializeField]
        private bool walkToggle = true;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;
        
        [SerializeField]
        [VectorRange(0, 1, 0, 1)]
        private Vector2 mouseSensitivity = new Vector2(0.25f, 0.25f);
        
        private StarterAssetsActions _myControls;

        private void OnValidate()
        {
            ValidateSensitivity();
        }

        private void ValidateSensitivity()
        {
            if (_myControls != null)
            {
                // TODO: change only for mouse?
                _myControls.Player.Look.ApplyBindingOverride(
                    new InputBinding
                    {
                        groups = "KeyboardMouse",
                        overrideProcessors =
                            $"InvertVector2(invertX=false),ScaleVector2(x={mouseSensitivity.x},y={mouseSensitivity.y})"
                    }
                );
            }
        }

        private void OnEnable()
        {
            if (_myControls == null)
            {
                _myControls = new StarterAssetsActions();
                _myControls.Player.SetCallbacks(this);
            }

            _myControls.Player.Enable();
            ValidateSensitivity();
        }

        private void OnDisable()
        {
            _myControls.Player.Disable();
        }

        #region Input Callbacks

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (cursorInputForLook)
            {
                LookInput(context.ReadValue<Vector2>());
                UsingMouseToLook = context.control.device == Mouse.current;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    JumpInput(true);
                    break;
                case InputActionPhase.Canceled:
                    JumpInput(false);
                    break;
                default:
                    return;
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    SprintInput(!sprintToggle || !sprint);
                    break;
                case InputActionPhase.Canceled:
                    SprintInput(sprintToggle && sprint);
                    break;
                default:
                    return;
            }
        }

        public void OnWalk(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    WalkInput(!walkToggle || !walk);
                    break;
                case InputActionPhase.Canceled:
                    WalkInput(walkToggle && walk);
                    break;
                default:
                    return;
            }
        }

        #endregion

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void WalkInput(bool newWalkState)
        {
            walk = newWalkState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}