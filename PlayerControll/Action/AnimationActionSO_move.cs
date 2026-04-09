using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Actions/MoveAnimation")]
public class AnimationActionSO_move : ActionSO<PlayerContext>
{
    [Tooltip("값이 목표치에 도달하는 데 걸리는 시간. (0.1 ~ 0.2 추천. 클수록 미끄러지듯 변함)")]
    [SerializeField] private float m_dampTime = 0.1f;

    public override void OnUpdate(PlayerContext context)
    {
        if (context.animator == null) return;

        Vector3 horizontalVelocity = new Vector3(context.Velocity.x, 0, context.Velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        // 핵심: 매개변수가 4개인 SetFloat을 사용합니다!
        // (파라미터 해시, 목표 값, 댐핑 시간, 델타 타임)
        if(currentSpeed == 0f)
            context.animator.SetFloat(context.moveHash, currentSpeed);
        else
            context.animator.SetFloat(context.moveHash, currentSpeed, m_dampTime, Time.deltaTime);
    }
}
