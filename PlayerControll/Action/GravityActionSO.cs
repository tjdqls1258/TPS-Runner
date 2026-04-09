using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Actions/Apply Gravity")]
public class GravityActionSO : ActionSO<PlayerContext>
{    
    public override void OnUpdate(PlayerContext context)
    {
        if (context.IsGrounded && context.Velocity.y < 0)
        {
            context.Velocity.y = -2f; // 땅에 붙어있도록 미세한 음수 유지
        }
        else
        {
            context.Velocity.y += context.Gravity * Time.deltaTime; // 중력 누적
        }
    }
}
