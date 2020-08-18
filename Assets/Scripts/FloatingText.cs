using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float urTimehasCome = 3f;
    public Vector3 offset = new Vector3(0, 1, 0);

    void Start()
    {
        Destroy(gameObject, urTimehasCome);

        //transform.localPosition += offset;
    }
}
