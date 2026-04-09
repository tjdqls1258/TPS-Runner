using UnityEngine;
using UnityEngine.InputSystem;
using Util_Patten.FSM;

public class PlayerStateMachine : StateMachine<PlayerContext, StateSO<PlayerContext>>
{
    private PlayerControls inputActions;

    private void Awake()
    {
        inputActions = new();
        context.CameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    protected override void Update()
    {
        if (inputActions == null) return;

        context.MoveInput = inputActions.Player.Move.ReadValue<Vector2>();
        context.JumpInput = inputActions.Player.Jump.WasPressedThisFrame();
        context.Dash = inputActions.Player.Dash.IsPressed();
        context.IsGrounded = context.Controller.isGrounded;

        base.Update();

        context.Controller.Move(context.Velocity * Time.deltaTime);
    }
}
