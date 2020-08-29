using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSceneTransition : MonoBehaviour
{
    private LevelLoader LL;
    public float VideoLength;

    void Awake()
    {
        LL = FindObjectOfType<LevelLoader>();
        Invoke("LoadScene", VideoLength);
    }

    void LoadScene()
    {
        if (LL != null)
        {
            LL.LoadNextLevel();
        }
    }
}
