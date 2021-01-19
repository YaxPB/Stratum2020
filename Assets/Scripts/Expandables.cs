using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expandables : MonoBehaviour
{
    public Animator expandableAnimator;

    public void ExpandTheThing()
    {
        expandableAnimator.SetBool("isExpanded", true);
    }

    public void ShrinkTheThing()
    {
        expandableAnimator.SetBool("isExpanded", false);
    }

}
