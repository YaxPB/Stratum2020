using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicCamera : MonoBehaviour
{
    public Transform camera;

    public Transform[] cameraPos;

    public float transitionSpeed;

    private int pageLocation = 0;

    private LevelLoader LL;

    void Awake()
    {
        LL = FindObjectOfType<LevelLoader>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && pageLocation + 1 < cameraPos.Length)
        {
            StartCoroutine(ChangePOS(cameraPos[pageLocation].position, cameraPos[pageLocation + 1].position));
            pageLocation++;
            //Debug.Log(pageLocation);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            if (LL != null)
            {
                LL.LoadNextLevel();
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && pageLocation > 0)
        {
            StartCoroutine(ChangePOS(cameraPos[pageLocation].position, cameraPos[pageLocation - 1].position));
            pageLocation--;
            //Debug.Log(pageLocation);
        }
    }

    IEnumerator ChangePOS(Vector3 cameraPos1, Vector3 cameraPos2)
    {
        float lerpAmount = 0;

        while(lerpAmount < 1)
        {
            camera.position = Vector3.Lerp(cameraPos1, cameraPos2, lerpAmount);
            lerpAmount += Time.fixedDeltaTime * transitionSpeed;

            yield return new WaitForFixedUpdate();
        }
    } 
}
