using UnityEngine;

public class FootIKHandler : MonoBehaviour
{
    private Animator anim;

    [Range(0, 1)] public Color rayColor = Color.red;
    [Range(0, 1)] public float footIKWeight = 1.0f; // IK 적용 강도
    public LayerMask groundLayer; // 지면 레이어
    public float footOffset = 0.1f; // 발바닥 두께 보정값

    void Awake() => anim = GetComponent<Animator>();

    // 중요: IK 계산은 반드시 이 함수에서 수행해야 함
    void OnAnimatorIK(int layerIndex)
    {
        if (anim == null) return;

        // IK 가중치 설정 (0이면 애니메이션 그대로, 1이면 IK 강제 적용)
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, footIKWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, footIKWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, footIKWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, footIKWeight);

        // 왼쪽 발 보정
        AdjustFootIK(AvatarIKGoal.LeftFoot);
        // 오른쪽 발 보정
        AdjustFootIK(AvatarIKGoal.RightFoot);
    }

    private void AdjustFootIK(AvatarIKGoal foot)
    {
        RaycastHit hit;
        Vector3 footPos = anim.GetIKPosition(foot); // 현재 애니메이션상의 발 위치

        // 발 위치에서 아래로 레이를 쏨
        if (Physics.Raycast(footPos + Vector3.up, Vector3.down, out hit, 2f, groundLayer))
        {
            Vector3 targetPos = hit.point;
            targetPos.y += footOffset; // 지면에서 살짝 띄움

            // 발의 위치를 지면 높이로 설정
            anim.SetIKPosition(foot, targetPos);

            // 발의 각도를 지면 기울기에 맞게 회전 (선택 사항)
            Quaternion targetRot = Quaternion.FromToRotation(Vector3.up, hit.normal) * anim.GetIKRotation(foot);
            anim.SetIKRotation(foot, targetRot);
        }
    }
}