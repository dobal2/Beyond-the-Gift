using System;
using UnityEngine;

public class MovingSled : MonoBehaviour
{
    [SerializeField] GameObject m_sleds;
    [SerializeField] Transform m_position1; // Order 목표 위치
    [SerializeField] Transform m_position2; // Sled 목표 위치

    bool isOrderMoving = false;
    bool isSledMoving = false;

    Vector3 orderVelo = Vector3.zero;
    Vector3 sledVelo = Vector3.zero;

    private void Update()
    {
        // Order 이동 처리
        if (isOrderMoving)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                m_position1.position,
                ref orderVelo,
                0.5f
            );

            // 목표 위치에 도달하면 이동 종료
            if (Vector3.Distance(transform.position, m_position1.position) < 0.01f)
            {
                isOrderMoving = false;
                
                Invoke(nameof(MoveSledUI), 1f);
            }
        }

        // Sled 이동 처리
        if (isSledMoving)
        {
            m_sleds.transform.position = Vector3.SmoothDamp(
                m_sleds.transform.position,
                m_position2.position,
                ref sledVelo,
                0.5f
            );

            // 목표 위치에 도달하면 이동 종료
            if (Vector3.Distance(m_sleds.transform.position, m_position2.position) < 0.01f)
            {
                isSledMoving = false;
            }
        }
    }

    public void MoveOrderUI()
    {
        orderVelo = Vector3.zero;
        isOrderMoving = true;
    }

    public void MoveSledUI()
    {
        sledVelo = Vector3.zero;
        isSledMoving = true;
    }
}