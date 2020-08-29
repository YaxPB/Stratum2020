using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    bool hasResponse;

    [TextArea(3, 10)]
    public string[] sentences;
    [TextArea(1, 2)]
    public string[] responses;
}
