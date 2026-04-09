using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Actions/Move")]
public class CameraMoveAtionSO : ActionSO<PlayerContext>
{
    [Header("Movement Settings")]
    public float rotationSpeed = 15f;

    [Header("Turn Settings")]
    [Tooltip("몇 도 이상 꺾을 때 Turn 애니메이션을 재생할 것인가?")]
    public float turnThresholdAngle = 135f;

    private bool isTurning; // 턴 애니메이션이 재생 중인지 체크하는 플래그

    private void OnEnable()
    {
        //turnAnimHash = Animator.StringToHash("Turn180");
    }

    public override void OnEnter(PlayerContext context)
    {
        isTurning = false;
    }

    public override void OnExit(PlayerContext context)
    {
    }

    public override void OnUpdate(PlayerContext context)
    {
        if (context.MoveInput.sqrMagnitude < 0.01f)
        {
            context.Velocity.x = 0;
            context.Velocity.z = 0;
            return;
        }

        Vector3 camForward = context.CameraTransform.forward;
        Vector3 camRight = context.CameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 targetDir = (camForward * context.MoveInput.y + camRight * context.MoveInput.x).normalized;
        float angleDifference = Vector3.Angle(context.Controller.transform.forward, targetDir);

        if (angleDifference >= turnThresholdAngle && !isTurning)
        {
            // 턴 애니메이션 실행 트리거 (애니메이터에 "Turn180" Trigger 파라미터 필요)
            //context.Anim.SetTrigger(turnAnimHash);
            isTurning = true;

            // 턴하는 동안 이동 속도를 잠깐 줄임
            context.Velocity.x *= 0.2f;
            context.Velocity.z *= 0.2f;
        }
        else if (angleDifference < 45f)
        {
            // 목표 방향과 얼추 비슷해지면 턴 상태 해제 (다시 턴을 감지할 수 있게 함)
            isTurning = false;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        context.Controller.transform.rotation = Quaternion.Slerp(
            context.Controller.transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);

        Vector3 targetVelocity = targetDir * (context.Dash ? context.DashMoveSpeed : context.BaseMoveSpeed);
        Vector3 currentHorizontalVelocity = new Vector3(context.Velocity.x, 0, context.Velocity.z);
        float currentAccelRate = (context.MoveInput.sqrMagnitude > 0.01f) ? context.Acceleration : context.Deceleration;

        currentHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetVelocity,
            currentAccelRate * Time.deltaTime
        );

        context.Velocity.x = currentHorizontalVelocity.x;
        context.Velocity.z = currentHorizontalVelocity.z;
    }
}
