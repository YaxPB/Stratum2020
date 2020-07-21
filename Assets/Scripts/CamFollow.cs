using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    // public Transform target;
    // the higher the value, the faster the camera locks onto target; keep between 0-1
    // public float smoothSpeed = 0.125f;
    // public Vector3 offset;

    [SerializeField]
    GameObject thePlayer = null;

    [SerializeField]
    float timeOffset = 0f;

    [SerializeField]
    Vector2 posOffset = new Vector2(0f, 0f);

    private Vector3 theVelocity;

    [SerializeField]
    float leftLimit = 0f;
    [SerializeField]
    float rightLimit = 0f;
    [SerializeField]
    float topLimit = 0f;
    [SerializeField]
    float bottomLimit = 0f;


    private void Update()
    {
        // Current position of the camera
        Vector3 startPos = transform.position;

        // Current position of thePlayer
        Vector3 endPos = thePlayer.transform.position;

        endPos.x += posOffset.x;
        endPos.y += posOffset.y;
        endPos.z += -10;

        // Smooth out the camera movement towards player position
        transform.position = Vector3.Lerp(startPos, endPos, timeOffset * Time.deltaTime);

        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
            Mathf.Clamp(transform.position.y, bottomLimit, topLimit),
            transform.position.z  
        );

    }


    private void LateUpdate()
    {
        // transform.position = target.position + offset;
    }

}
