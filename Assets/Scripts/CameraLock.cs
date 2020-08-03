using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    //public Transform lockpoint;

    public GameObject Camera;
    public GameObject OgCamera;

    //public float offsetX;
    //public float offsetY;

    void Start()
    {
        //cameraFollowScript = GetComponent<CameraFollow>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            OgCamera.SetActive(false);
            Camera.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            OgCamera.SetActive(true);
            Camera.SetActive(false);
        }
    }
}
