using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Conditions/Check Anim Finished")]
public class CheckAnimFinishedConditionSO : ConditionSO<PlayerContext>
{
    [Tooltip("끝난 것을 감지할 애니메이션의 State 이름")]
    [SerializeField] private string m_animStateName = "Land";

    [Tooltip("애니메이션 파라미터 해시")]
    [SerializeField] private string m_fall;
    [SerializeField] private string m_land;

    [Tooltip("애니메이션이 몇 % 진행되었을 때 넘어갈 것인가? (0.9 ~ 1.0 추천)")]
    [Range(0f, 1f)]
    [SerializeField] private float m_finishTime = 0.95f;

    private int m_fallHash;
    private int m_landHash;
    private void OnEnable()
    {
        m_fallHash = Animator.StringToHash(m_fall);
        m_landHash = Animator.StringToHash(m_land);
    }

    public override bool Evaluate(PlayerContext context)
    {
        AnimatorStateInfo stateInfo = context.animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(m_animStateName))
        {
            if (stateInfo.normalizedTime >= m_finishTime)
            {
                return true; // 조건 달성! 다음 상태로 넘어가라!
            }
        }
        else if(stateInfo.normalizedTime > 0.1f)
        {
            context.animator.SetBool(m_fallHash, false);
            context.animator.SetTrigger(m_landHash);
        }

        return false;
    }
}