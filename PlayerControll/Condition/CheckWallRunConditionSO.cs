using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Conditions/Check WallRun")]
public class CheckWallRunConditionSO : CheckAirbornConditionSO
{
    public override bool Evaluate(PlayerContext context)
    {
        Transform playerTr = context.Controller.transform;

        if (RayCastHelper(true, context, playerTr))
            return true;
        else if (RayCastHelper(false, context, playerTr))
            return true;

        if (base.Evaluate(context)) return false;

        context.WallSide = 0;
        return false;
    }

    protected bool RayCastHelper(bool right, PlayerContext context, Transform playerTr)
    {
        if (context.JumpInput)
        {
            if (Physics.Raycast(playerTr.position, right ? playerTr.right : -playerTr.right, out RaycastHit hit, context.wallCheckDistance, context.wallLayer))
            {
                context.WallNormal = hit.normal;
                context.WallSide = right ? 1 : -1;
                context.Wall = true;
                Debug.DrawRay(playerTr.position, right ? playerTr.right : -playerTr.right, Color.green);
                return true;
            }
        }

        Debug.DrawRay(playerTr.position, right ? playerTr.right : -playerTr.right, Color.red);
        return false;
    }
}
