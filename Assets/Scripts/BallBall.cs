using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBall : MonoBehaviour
{
    public float thrust;
    public float knockTime;

    public float speed = 20f;
    public int damage = 10;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    public bool didHit { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        Rigidbody2D enemyRb = hitInfo.GetComponent<Rigidbody2D>();
        if (enemy != null)
        {
            didHit = true;
            //KNOCKBACK CODE
            enemy.speed = enemy.speed/2;
            Vector2 difference = enemyRb.transform.position - transform.position;
            difference = difference.normalized * thrust;
            enemyRb.AddForce(difference, ForceMode2D.Impulse);
            
            Debug.Log("test");

            //Instantiate(impactEffect, transform.position, transform.rotation);

            StartCoroutine(KnockCo(enemyRb, enemy));

            enemy.TakeDamage(damage);
        }
    }

    private IEnumerator KnockCo(Rigidbody2D enemy, Enemy info)
    {
        if(enemy != null)
        {
            yield return new WaitForSeconds(knockTime);
            enemy.velocity = Vector2.zero;
            Debug.Log(enemy.velocity);

            //speed the enemy back up after the knockback;
            info.speed = info.regSpeed;

            Destroy(gameObject);
        }
    }
}
