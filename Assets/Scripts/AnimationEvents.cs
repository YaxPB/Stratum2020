using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void Start()
    {

    }
    public void EndHitAnim()
    {
        BorderBeats.instance.borderAnimTL.SetBool("PunchyPunchy", false);
        BorderBeats.instance.borderAnimTR.SetBool("PunchyPunchy", false);
    }
}
