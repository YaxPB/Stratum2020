using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform kickOff;

    public GameObject ballPrefab;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Kick"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Debug.Log("kicked!");
        Instantiate(ballPrefab, kickOff.position, kickOff.rotation);
    }
}
