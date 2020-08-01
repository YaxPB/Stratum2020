using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDistance;
    public int randRange = 2;

    public int cameraZPos = -10;

    public Transform camera;

    Vector3 cameraPos;

    void Start()
    {
        cameraPos = camera.position;
    }

    void Update()
    {
        Shake();
    }

    void Shake()
    {
        Vector3 shakePos = new Vector3(camera.position.x + Random.Range(0,randRange) * shakeDistance, camera.position.y + Random.Range(0, randRange) * shakeDistance, cameraZPos);

        camera.position = shakePos; 
    }
}
