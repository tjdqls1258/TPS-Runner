using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Actions/LandAnimation")]
public class LandAnimationActionSO : ActionSO<PlayerContext>
{
    public override void OnUpdate(PlayerContext context)
    {
        context.animator.SetBool(context.fallHash, context.IsGrounded == false);            
    }

    public override void OnExit(PlayerContext context)
    {
        context.animator.SetBool(context.fallHash, false);
        context.animator.ResetTrigger(context.landHash);
    }
}
