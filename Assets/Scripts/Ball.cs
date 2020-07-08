using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform kickOff;
    public GameObject ballPrefab;

    public float despawn = 2f;
    
    public float ballCoolDown = 2f;
    private float nextBall = 0;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextBall)
        {
            if (Input.GetButtonDown("Kick"))
            {
                Shoot();
                nextBall = Time.time + ballCoolDown;
            }
        }
    }

    void Shoot()
    {
        Debug.Log("kicked!");
        GameObject flight = Instantiate(ballPrefab, kickOff.position, kickOff.rotation);
        Destroy(flight, despawn);
    }
}
