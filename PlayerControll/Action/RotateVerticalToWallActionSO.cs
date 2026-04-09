using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Actions/Rotate To Wall Forward")]
public class RotateToWallForwardActionSO : ActionSO<PlayerContext>
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 15f;
    [Range(0, 90)] public float tiltAngle = 90f; // 90도면 벽에 완전히 수직으로 붙음
    [Tooltip("벽에서 캐릭터를 얼마나 띄울 것인가?")]
    public float wallOffset = 0.5f;

    public override void OnUpdate(PlayerContext context)
    {
        if (context.WallNormal == Vector3.zero) return;

        // 1. 진행 방향(Forward) 계산
        Vector3 wallForward = Vector3.Cross(context.WallNormal, Vector3.up);
        if (Vector3.Dot(context.Controller.transform.forward, wallForward) < 0)
            wallForward = -wallForward;

        // 2. 벽이 캐릭터의 어느 쪽에 있는지 판별 (왼쪽? 오른쪽?)
        // 캐릭터의 진행 방향(wallForward)과 하늘(up)을 외적하면 캐릭터의 '오른쪽' 방향이 나옵니다.
        Vector3 playerRight = Vector3.Cross(Vector3.up, wallForward);

        // 벽의 법선(WallNormal)과 캐릭터의 오른쪽 방향을 비교합니다.
        // 내적값이 양수면 벽이 왼쪽에 있는 것이고, 음수면 오른쪽에 있는 것입니다.
        float sideDot = Vector3.Dot(playerRight, context.WallNormal);

        // 3. 벽 위치에 따라 기울기 방향(Sign) 결정
        // 왼쪽에 벽이 있으면 시계 반대방향(-), 오른쪽에 있으면 시계방향(+)으로 꺾어야 합니다.
        float tiltSign = (sideDot > 0) ? -1f : 1f;

        // 4. 최종 Up 벡터 계산
        // Slerp 대신 각도를 직접 계산하여 Quaternion.LookRotation에 넣는 것이 더 정확합니다.
        float finalAngle = (tiltAngle * tiltSign);

        // wallForward를 축으로 하여 Vector3.up을 finalAngle만큼 회전시킨 새로운 Up 벡터 생성
        Vector3 tiltedUp = Quaternion.AngleAxis(finalAngle, wallForward) * Vector3.up;

        // 5. 최종 회전 적용
        if (wallForward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(wallForward, tiltedUp);

            context.Controller.transform.rotation = Quaternion.Slerp(
                context.Controller.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if(context.ModleTransform != null)
                context.ModleTransform = context.Controller.transform.GetChild(0);

            context.ModleTransform.localPosition = new Vector3(0, wallOffset, 0);
        }
    }

    public override void OnExit(PlayerContext context)
    {
        base.OnExit(context);

        // 상태를 나갈 때, 현재 바라보는 방향(Forward)은 유지하되 
        // 하늘 방향(Up)을 강제로 정방향(Vector3.up)으로 맞춘 회전값을 한 번 쏴줍니다.
        // 이렇게 하면 다음 상태의 Slerp가 이 '똑바른' 기준점에서 시작됩니다.
        Vector3 forward = context.Controller.transform.forward;
        forward.y = 0;
        context.Controller.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        context.ModleTransform.localPosition = Vector3.zero;
    }
}