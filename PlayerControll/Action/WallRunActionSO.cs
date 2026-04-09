using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Actions/Wall Run")]
public class WallRunActionSO : ActionSO<PlayerContext>
{
    [Header("Wall Run Settings")]
    public float wallRunSpeed = 12f;
    public float wallGravity = -9f;
    public float sticckToWallFroce = 5f;

    public float minWallRunSpeed = 8f;
    public float currentWallRunSpeed;

    public string m_moveAnimationName;
    public string m_wallAnimationName;

    private int m_moveSpeedHash;
    private int m_wallHash;

    private void OnEnable()
    {
        m_wallHash = Animator.StringToHash(m_wallAnimationName);
        m_moveSpeedHash = Animator.StringToHash(m_moveAnimationName);
    }

    public override void OnEnter(PlayerContext context)
    {
        Vector3 horizontalVelocity = new Vector3(context.Velocity.x, 0, context.Velocity.z);
        float entrySpeed = horizontalVelocity.magnitude;

        currentWallRunSpeed = Mathf.Max(entrySpeed, minWallRunSpeed);

        context.Velocity.y = 0f;
        context.Wall = true;

        context.animator.SetTrigger(m_wallHash);
    }

    public override void OnUpdate(PlayerContext context)
    {
        Vector3 wallForward = Vector3.Cross(context.WallNormal, Vector3.up);

        if (Vector3.Dot(context.Controller.transform.forward, wallForward) < 0)
            wallForward = -wallForward;

        context.Velocity.x = wallForward.x * currentWallRunSpeed;
        context.Velocity.z = wallForward.z * currentWallRunSpeed;

        context.Velocity.y += wallGravity * Time.deltaTime;

        context.Velocity += -context.WallNormal * sticckToWallFroce * Time.deltaTime;

        float currentSpeed = context.Velocity.magnitude;
        context.animator.SetFloat(m_moveSpeedHash, currentSpeed);
    }

    public override void OnExit(PlayerContext context)
    {
        context.Wall = false;
    }
}
