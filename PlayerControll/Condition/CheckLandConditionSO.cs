using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Conditions/Check Land")]
public class CheckLandConditionSO : ConditionSO<PlayerContext>
{
    [SerializeField] private float m_minHeightCheck = 1f;
    [SerializeField] private LayerMask m_groundLayer = ~0;

    public override bool Evaluate(PlayerContext context)
    {
        if (context.Controller.isGrounded)
            return true;

        Vector3 rayOrigin = context.Controller.transform.position + context.Controller.center;
        float rayDistance = (context.Controller.height / 2f) + m_minHeightCheck;

        if (Physics.Raycast(rayOrigin, Vector3.down, rayDistance, m_groundLayer))
        {
            return true;
        }

        return false;
    }
}
