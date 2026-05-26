using System;
using System.Data;
using UnityEngine;

public class RollingUI : MonoBehaviour
{
    [SerializeField] Transform targetPos;
    [SerializeField] Transform productUI;

    Boolean isMoving = false;
    Vector3 velo = Vector3.zero;
    float time = 0f;

    private void Start()
    {
    }

    private void Update()
    {
        if (isMoving)
        {
            time += Time.deltaTime;
            productUI.position = Vector3.SmoothDamp
               (productUI.position, targetPos.position, ref velo, 0.5f);

            if (time > 2f)
                isMoving = false;
        }
    }

    public void MoveProductUI()
    {
        isMoving = true;
    }
}