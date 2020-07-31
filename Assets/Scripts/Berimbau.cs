using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Berimbau : MonoBehaviour
{
    public Transform beatTimer;
    public Image timerFill;
    public Image beatZone;
    private Animator beatZoneAnim;
    [SerializeField] private float currentAmount;
    [SerializeField] private float speed;

    private int count;
    private Canvas berimBAM;

    private void Start()
    {
        count = 4;
        ResetPrompt();
        beatZoneAnim = beatZone.GetComponent<Animator>();
    }

    private void Update()
    {
        // find a way to get playerInput data 
        // every time the attack button is pressed during berimbau ability,
        // check to see if it lines up with the highlighted area

        if (Input.GetButtonDown("Fire1"))
        {
            BeatHit();
        }

        if (currentAmount < 100)
        {
            currentAmount += speed * Time.deltaTime;
        }
        timerFill.fillAmount = currentAmount / 100;

        if(currentAmount >= 100)
        {
            ResetPrompt();
        }
    }

    void BeatHit()
    {
        if (currentAmount >= 78 && currentAmount <= 100)
        {
            Debug.Log("Hit!");
            beatZoneAnim.SetTrigger("isHit");
            AudioManagerSFX.PlaySound("berimBAM");
            // Add some kind of value +1 to PlayerCombat?
        }
        else
        {
            Debug.Log("Miss");
            // AudioManagerSFX.PlaySound("berimMiss");
        }

    }

    private void ResetPrompt()
    {
        if (count == 0)
        {
            beatTimer.gameObject.SetActive(false);
        }
        int timerRotationZ = Random.Range(0, 360);
        beatTimer.transform.eulerAngles = new Vector3(
            gameObject.transform.eulerAngles.x,
            gameObject.transform.eulerAngles.y,
            timerRotationZ
            );
        currentAmount = 0;
        count--;
    }
}
