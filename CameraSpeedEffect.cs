using Unity.Cinemachine;
using UnityEngine;

public class CameraSpeedEffect : MonoBehaviour
{
    [SerializeField] private CinemachineCamera m_cam;
    [SerializeField] CharacterController m_playerRb;

    private float m_baseFov;
    [SerializeField] float m_maxFov = 90f;
    [SerializeField] float m_speedForMaxFov = 20f;

    private void Awake()
    {
        m_baseFov = m_cam.Lens.FieldOfView;
    }

    private void Update()
    {
        float currentSpeed = m_playerRb.velocity.magnitude;
        float targetFov = Mathf.Lerp(m_baseFov, m_maxFov, currentSpeed / m_speedForMaxFov);

        m_cam.Lens.FieldOfView = targetFov;
    }
}
