using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Actions/Jump")]
public class JumpActionSO : ActionSO<PlayerContext>
{
    public override void OnEnter(PlayerContext context)
    {
        context.animator.SetFloat(context.moveHash, 0);
        context.animator.SetTrigger(context.jumpHash);
        context.Velocity.y = context.JumpForce;
    }

    public override void OnExit(PlayerContext context)
    {
        context.animator.ResetTrigger(context.jumpHash);
    }
}
