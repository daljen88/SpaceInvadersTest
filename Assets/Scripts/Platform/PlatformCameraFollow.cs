using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public float followSpeed;
    public Vector3 offset;
    public Vector2 safeArea;
    Vector3 cameraPos;
    void Start()
    {
        offset = followTarget.position - transform.position;
    }

    void Update()
    {
        //follow fisso
        //transform.position= followTarget.position - offset;
        //follow morbido
        cameraPos = Vector3.Lerp(transform.position, followTarget.position - offset, Time.deltaTime * followSpeed);
        if (Mathf.Abs(transform.position.y - (followTarget.position - offset).y) < safeArea.y)
        {
            cameraPos.y = transform.position.y;
        }
        if (Mathf.Abs(transform.position.x - (followTarget.position - offset).x) < safeArea.x)
        {
            cameraPos.x = transform.position.x;
        }
        transform.position = cameraPos;
    }
}
