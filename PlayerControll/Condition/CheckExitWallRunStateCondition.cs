using Unity.VisualScripting;
using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Conditions/Check WallState")]
public class CheckExitWallRunStateCondition : ConditionSO<PlayerContext>
{
    [SerializeField] private Vector3 m_halfExtents;
    public override bool Evaluate(PlayerContext context)
    {
        if (context.WallSide == 0) return true;

        Transform playerTr = context.ModleTransform;

        if (IsWallForward(context, playerTr))
            return true;

        if (IsWallStillThereWithBox(context, playerTr))
            return false;

        return true;
    }

    private bool IsWallStillThereWithBox(PlayerContext context, Transform playerTr)
    {
        Vector3 checkDir = -context.WallNormal;
        Vector3 boxCenter = playerTr.position + (checkDir * context.wallCheckDistance * 0.5f);

        Collider[] hitWalls = Physics.OverlapBox(
            boxCenter,
            m_halfExtents,
            playerTr.rotation,
            context.wallLayer
        );

        bool isHit = hitWalls.Length > 0;

        return isHit;
    }

    private bool IsWallForward(PlayerContext context, Transform playerTr)
    {
        Vector3 wallForward = Vector3.Cross(context.WallNormal, Vector3.up);
        Debug.DrawRay(playerTr.position + context.WallNormal, wallForward * (-context.WallSide), Color.blue);
        return Physics.Raycast(playerTr.position + context.WallNormal, wallForward * (-context.WallSide), context.wallCheckDistance, context.wallLayer);
    }
}
