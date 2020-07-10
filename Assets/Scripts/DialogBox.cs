using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
    public TextAsset[] theDialog;
    private int textNum;
    private TextAsset theText;

    // Start is called before the first frame update
    void Start()
    {
        theDialog = GetComponentsInParent<TextAsset>();
        textNum = 0;
        theText = theDialog[textNum];

    }

    public DialogBox(TextAsset[] theDialog, int textNum, TextAsset theText)
    {
        this.theDialog = theDialog;
        this.textNum = textNum;
        this.theText = theText;
    }

    public void nextPage()
    {
        Debug.Log("Testing 1, 2, 3");
    }
}
