using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    MovePlayer mp;

    private void Start()
    {
        mp = FindObjectOfType<MovePlayer>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(mp != null)
        {
            if (mp.load)
            {
                LoadNextLevel();
            }
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //change index after adding credits and comic
        if (levelIndex <= 6)
        {
            if (mp != null)
            {
                mp.load = false;
            }
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            if (mp != null)
            {
                mp.load = false;
            }
            levelIndex = 0;
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(levelIndex);
        }
    }
}
