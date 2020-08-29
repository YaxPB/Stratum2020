using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform kickOff;
    public BallBall ballPrefab;

    public float despawn = 2f;
    
    public float ballCoolDown = 2f;
    private float nextBall = 0;
    
    BallBall flight;
    private Cooldown cool;
    public Animator anim;

    private void Awake()
    {
        cool = FindObjectOfType<Cooldown>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextBall)
        {
            if (Input.GetButtonDown("Kick"))
            {
                AudioManagerSFX.PlaySound("ballIsLife");
                anim.SetTrigger("Ballin");
                Invoke("Shoot",0.35f);
                cool.ballIsCooling = true;
                nextBall = Time.time + ballCoolDown;
            }
        }
    }

    void Shoot()
    {
        flight = Instantiate<BallBall>(ballPrefab, kickOff.position, kickOff.rotation);
        Invoke("ByeBall", despawn);
    }

    void ByeBall()
    {
        if(!flight.didHit)
        {
            Destroy(flight.gameObject);
        }
    }
}
