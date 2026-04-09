using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Conditions/Check Jump")]
public class CheckJumpConditionSO : ConditionSO<PlayerContext>
{
    public override bool Evaluate(PlayerContext context)
    {
        return context.IsGrounded && context.JumpInput;
    }
}
