using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBeats : MonoBehaviour
{
    private Rigidbody2D theBeat;
    private BeatGenerator theGenerator;
    private BorderBeats theBorderBeats;
    public TheBeats instance;

    void Start()
    {
        instance = this;
        theBorderBeats = BorderBeats.instance;
        theBeat = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            Debug.Log("HIT");
            theBorderBeats.HitAnim();
            Destroy(theBeat.gameObject);
        }

    }


    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
    }

}
