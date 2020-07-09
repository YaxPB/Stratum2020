using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatGenerator : MonoBehaviour
{
    public Rigidbody2D aNoteTL;
    public Rigidbody2D aNoteTR;
    public GameObject borderPrefabTL;
    public GameObject borderPrefabTR;
    public static BeatGenerator instance;

    private void Start()
    {
        instance = this;

    }

    private void Update()
    {
        
    }

    public void SpawnBeat()
    {
        Debug.Log("Boop");
        aNoteTL = Instantiate(borderPrefabTL.GetComponent<Rigidbody2D>(), instance.transform);
        aNoteTR = Instantiate(borderPrefabTR.GetComponent<Rigidbody2D>(), instance.transform);
        aNoteTL.velocity = new Vector3(-18f, 0f, 0f);
        aNoteTR.velocity = new Vector3(18f, 0f, 0f);

    }

}
