using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Berimbau : MonoBehaviour
{
    public Transform beatTimer;
    public RectTransform particleFXHolder;
    public Image timerFill;
    public Image beatZone;
    private Animator beatZoneAnim;

    private RectTransform textRotation;
    [SerializeField] private float currentAmount;
    [SerializeField] private float speed;
    [SerializeField] private Text hitThisButton;
    private string[] buttonPrompts = { "J", "K", "L" };

    private int count;
    private int multiplier = 0;
    private Canvas berimBAM;

    void Start()
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
        //**Note from Jake: Maybe the buttons spell out something and do something when a word is spelled out
        //  - Would require some mechanical reworking, but doable! Test for feel FIRST.

        // Just for testing, remove from final (user shouldn't be able to left-click through all berimbau prompts)
        if (Input.GetButtonDown("Fire1"))
        {
            BeatHit();
        }

        if(hitThisButton.text.Equals("J"))
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                BeatHit();
            }
        }

        if (hitThisButton.text.Equals("K"))
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                BeatHit();
            }
        }

        if (hitThisButton.text.Equals("L"))
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                BeatHit();
            }
        }
        
        if (currentAmount < 100)
        {
            currentAmount += speed * Time.deltaTime;
        }
        timerFill.fillAmount = currentAmount / 100;
        particleFXHolder.rotation = Quaternion.Euler(new Vector3(0f, 0f, -timerFill.fillAmount * 360));

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
            multiplier++;
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
            PlayerCombat.instance.SendMessage("BuffBoi", multiplier);
            multiplier = 0;
            count = 4;
            beatTimer.gameObject.SetActive(false);
        }
        int timerRotationZ = Random.Range(0, 360);
        int whichButton = Random.Range(0, 3);
        hitThisButton.transform.localEulerAngles = new Vector3(
            gameObject.transform.eulerAngles.x,
            gameObject.transform.eulerAngles.y,
            -timerRotationZ
            );
        hitThisButton.text = buttonPrompts[whichButton];
        beatTimer.transform.eulerAngles = new Vector3(
            gameObject.transform.eulerAngles.x,
            gameObject.transform.eulerAngles.y,
            timerRotationZ
            );
        currentAmount = 0;
        count--;
    }

}
