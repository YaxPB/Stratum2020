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
    public ParticleSystem attackUP;
    MovePlayer mp;

    private RectTransform textRotation;
    [SerializeField] private float currentAmount;
    [SerializeField] private float speed;
    [SerializeField] private Text hitThisButton;
    private string[] buttonPrompts = { "W", "A", "S", "D"};

    private int count;
    private float regSpeed;
    private int multiplier = 0;
    private Canvas berimBAM;
    private Cooldown cool;

    void Start()
    {
        mp = FindObjectOfType<MovePlayer>();
        cool = FindObjectOfType<Cooldown>();
        regSpeed = mp.runSpeed;
        count = 4;
        ResetPrompt();
        beatZoneAnim = beatZone.GetComponent<Animator>();
    }

    private void Update()
    {
        // Mostly for testing, enables left-click to shortcut berimbau prompts
        if (Input.GetButtonDown("Fire1"))
        {
            BeatHit();
        }

        if(hitThisButton.text.Equals("W"))
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                BeatHit();
            }
        }

        if (hitThisButton.text.Equals("A"))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                BeatHit();
            }
        }

        if (hitThisButton.text.Equals("S"))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                BeatHit();
            }
        }

        if (hitThisButton.text.Equals("D"))
        {
            if (Input.GetKeyDown(KeyCode.D))
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
            AudioManagerSFX.PlaySound("attackUP");
            attackUP.Play();
            multiplier++;
            mp.runSpeed += 1.5f;
            // Add some kind of value +1 to PlayerCombat?
        }
        else
        {
            AudioManagerSFX.PlaySound("berimBAM");
            Debug.Log("Miss");
            // AudioManagerSFX.PlaySound("berimMiss");
        }

    }

    private void ResetPrompt()
    {
        if (count == 0)
        {
            PlayerCombat.instance.SendMessage("BuffBoi", multiplier);
            Invoke("ResetStuff", 5f);
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

    public void ResetStuff()
    {
        mp.runSpeed = regSpeed;
        cool.berimbauIsCooling = true;
    }
}
