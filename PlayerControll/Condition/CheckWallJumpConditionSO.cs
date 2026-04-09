using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Conditions/Check Wall Jump")]
public class CheckWallJumpConditionSO : ConditionSO<PlayerContext>
{
    public override bool Evaluate(PlayerContext context)
    {
        return context.JumpInput;
    }
}
