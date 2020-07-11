using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    //public float smoothSpeed = 0.125f;
    //public Vector3 offset;

    public float offsetX;
    public float offsetY;

    public bool border;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void LateUpdate()
    {
        Vector3 temp = transform.position;

        temp.x = player.position.x;
        temp.x += offsetX;

        temp.y = player.position.y;
        temp.y += offsetY;

        transform.position = temp;
    }

    /*void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // * Time.deltaTime (make smoothSpeed higher: 10f)
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }*/
}
