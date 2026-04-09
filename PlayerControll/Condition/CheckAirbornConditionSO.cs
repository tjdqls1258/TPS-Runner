using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Conditions/Check Airborne")]
public class CheckAirbornConditionSO : ConditionSO<PlayerContext>
{
    public override bool Evaluate(PlayerContext context)
    {
        if (context.Controller.isGrounded)
            return false;

        Vector3 rayOrigin = context.Controller.transform.position + context.Controller.center;
        float rayDistance = (context.Controller.height / 2f) + context.minHeightCheck;

        if (Physics.Raycast(rayOrigin, Vector3.down, rayDistance, context.groundLayer))
        {
            return false;
        }

        return true;
    }
}
