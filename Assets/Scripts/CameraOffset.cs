using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    public Transform cameraTarget;

    public float offsetX;
    public float offsetY;

    void LateUpdate()
    {
        Vector3 pos = cameraTarget.position;

        pos.x += offsetX;
        pos.y += offsetY;
    }
}
