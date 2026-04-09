using UnityEngine;
using Util_Patten.FSM;

[CreateAssetMenu(menuName = "FSM/Parkour/Actions/Move Stop")]
public class MoveStopActionSO : ActionSO<PlayerContext>
{
    public override void OnEnter(PlayerContext context)
    {
        context.Velocity.x = 0f;
        context.Velocity.z = 0f;
    }

    public override void OnUpdate(PlayerContext context)
    {
        context.Velocity.x = 0f;
        context.Velocity.z = 0f;
    }
}
