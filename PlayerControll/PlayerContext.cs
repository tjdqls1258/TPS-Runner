using System;
using UnityEngine;
using Util_Patten.FSM;

[Serializable]
public class PlayerContext : Context
{
    public CharacterController Controller;

    [Header("Stats")]
    public float BaseMoveSpeed = 10f;
    public float DashMoveSpeed = 15f;
    public float JumpForce = 8f;
    public float Gravity = -9.62f;
    public float Acceleration = 30f; //가속
    public float Deceleration = 40f;

    [Header("Runtime Data")]
    public Vector3 Velocity;    //속도
    public Vector2 MoveInput;   //입력
    public bool JumpInput;      //점프키 입력 여부
    public bool IsGrounded;     //땅에 있는지 여부
    public bool Dash;           //달리기 여부
    public bool Wall;

    public Transform ModleTransform;

    [Header("Wall Run Data")]
    public Vector3 WallNormal; // 벽의 수직 방향 (이 방향을 기준으로 앞을 향해 달림)
    public int WallSide;       // 1: 오른쪽 벽, -1: 왼쪽 벽, 0: 벽 없음
    public float wallCheckDistance = 1f;

    [Header("Camera")]
    public Transform CameraTransform;

    [Header("Animator")]
    public Animator animator;

    [Header("Check Air")]
    public float minHeightCheck = 1f;
    public LayerMask groundLayer = ~0;
    public LayerMask wallLayer = ~0;

    [Header("Animation Data")]
    [SerializeField] private string land;
    [SerializeField] private string move;
    [SerializeField] private string jump;
    [SerializeField] private string fall;
    [SerializeField] private string wall;

    [HideInInspector] public int landHash;
    [HideInInspector] public int moveHash;
    [HideInInspector] public int jumpHash;
    [HideInInspector] public int fallHash;
    [HideInInspector] public int wallHash;

    public override void Init()
    {
        landHash = Animator.StringToHash(land);
        moveHash = Animator.StringToHash(move);
        jumpHash = Animator.StringToHash(jump);
        fallHash = Animator.StringToHash(fall);
        wallHash = Animator.StringToHash(wall);
    }
}
