using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClamp : MonoBehaviour
{
    public Transform target;

    public float xmin;
    public float xmax;
    public float ymin;
    public float ymax;

    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, xmin, xmax),
            Mathf.Clamp(target.position.y, ymin, ymax),
            transform.position.z);
    }
}
