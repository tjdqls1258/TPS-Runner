using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Actions/Wall Jump")]
public class WallJumpActionSO : ActionSO<PlayerContext>
{
    [Header("Jump Force Settings (Horizontal Focus)")]
    // [SerializeField] private float jumpUpForce = 12f; // 위로 가는 힘 삭제!

    [Tooltip("벽 반대 방향으로 밀어내는 강력한 힘 (클수록 멀리 튕겨나감)")]
    [SerializeField] private float jumpOutForce = 15f;

    [Tooltip("점프 시 전진하는 힘 (클수록 앞으로 멀리 날아감)")]
    [SerializeField] private float jumpForwardForce = 8f;

    public override void OnEnter(PlayerContext context)
    {
        if (context.Controller == null) return;

        // 벽의 노멀(WallNormal)을 가져옵니다. (벽에서 튀어나오는 수평 방향)
        Vector3 wallNormal = context.WallNormal;
        wallNormal.y = 0; // 혹시라도 수직 성분이 섞여 있다면 제거 (수평 유지)
        wallNormal.Normalize();

        // 캐릭터의 현재 앞 방향을 가져옵니다.
        Vector3 forwardDir = context.Controller.transform.forward;
        forwardDir.y = 0; // 수평 유지
        forwardDir.Normalize();

        // 수직 힘(Vector3.up)을 완전히 배제하고 수평 벡터만 합성합니다.
        // 최종 점프 방향 계산: (벽 반대 방향 * 힘) + (앞쪽 * 힘)
        Vector3 finalJumpVelocity = (wallNormal * jumpOutForce) + (forwardDir * jumpForwardForce);

        context.Velocity.y = 0f;
        context.Velocity = finalJumpVelocity;

        if (context.animator != null)
        {
            context.animator.SetTrigger(context.landHash);
        }
    }

    public override void OnUpdate(PlayerContext context)
    {

    }
}
