using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity; ;
    }
}
