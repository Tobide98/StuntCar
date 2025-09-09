using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerControlScript : MonoBehaviour
{
    public float speed;
    public int life;
    public Rigidbody rb;
    public ObstacleScript currObs;
    public bool isCarBoost;
    public bool isCarSlowed;
    public float boostTime = 0.3f;
    public float slowedTime = 1f;
    private bool carIdle;
    public DWGDestroyer destroyerScript;
    public BoostTapScript boostScript;
    public Animator playerAnim;
    public VFXParentScript vfxScript;

    public bool isReady;
    private float slowState = 5f;
    private float boostState = 25f;
    private float normalState = 20f;
    private float stuntState = 15f;
    private float lastState = 35f;
    public float currSpeed;

    [Header("UI Related")]
    public Sprite perfectImg;
    public Sprite greatImg;
    public Sprite missImg;
    public Image timingImg;
    public Animator timingAnim;
    public GameObject boostTapBtn;
    public Button wallBoostBtn;
    public Button holeBtn;
    public Button coneBtn;
    public List<CanvasGroup> lifeImgs;
    public GameObject lifeParentUI;

    private void FixedUpdate()
    {
        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);
        if(!isReady)
        {
            Vector3 spd = transform.forward * speed * Time.fixedDeltaTime;
            rb.velocity = spd;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //ResetStatePlayer();
    }

    public void ResetStatePlayer()
    {
        StopMovement();
        speed = 0;
        currSpeed = 0;
        life = 3;
        rb.isKinematic = false;
        currObs = null;
        boostScript.totalTap = 0;
        isCarBoost = false;
        isCarSlowed = false;
        carIdle = true;
        SetActiveFalseBtn(true);
        SetInteractableActionBtn(false);
        boostTapBtn.SetActive(false);
        ResetLifeUI();
        playerAnim.SetTrigger("Default");
        isReady = false;
        vfxScript.StopAllParticles();
        this.transform.position = GameManagerScript.instance.currLevel.playerSpawnPos.position;
    }

    public void StartPlayer()
    {
        speed = normalState;
        currSpeed = normalState;
    }

    void Update()
    {
        BoostCarSpeedCheck();
        SlowDownCarCheck();
        if (currObs != null && !GameManagerScript.instance.isTutorial) SetInteractableActionBtn(true);
        else if (GameManagerScript.instance.isTutorial)
        {
            if (currObs != null && currObs.isPerfect)
            SetInteractableActionBtn(true);
        }
        else SetInteractableActionBtn(false);
    }

    public void ShowTimingText(string text)
    {
        timingAnim.SetTrigger("Start");
        switch (text)
        {
            case "Perfect": timingImg.sprite = perfectImg; break;
            case "Great": timingImg.sprite = greatImg; break;
            case "Miss": timingImg.sprite = missImg; break;
        }
    }

    public void BoostCarSpeedCheck()
    {
        if (isCarBoost)
        {
            boostTime -= Time.deltaTime;
            speed = boostState;
            currSpeed = boostState;
            if (boostTime < 0)
            {
                isCarBoost = false;
                boostTime = 0.3f;
                speed = normalState;
                currSpeed = normalState;
            }
        }
    }

    public void SlowDownCarCheck()
    {
        if (isCarSlowed)
        {
            speed = slowState;
            currSpeed = slowState;
            slowedTime -= Time.deltaTime;
            if (slowedTime < 0 && life > 0)
            {
                isCarSlowed = false;
                slowedTime = 1f;
                speed = normalState;
                currSpeed = normalState;
            }
            else if (life <= 0)
            {
                speed = 0;
                currSpeed = 0;
                rb.isKinematic = true;
            }
        }
    }

    public void CheckCarLife()
    {
        lifeImgs[life].alpha = 0.5f;
        if (life <= 0)
        {
            speed = 0;
            rb.isKinematic = true;
            vfxScript.particles[7].Play();
            GameManagerScript.instance.GameOver(false);
        }
    }

    public void ResetLifeUI()
    {
        lifeParentUI.SetActive(true);
        for (int i = 0; i < lifeImgs.Count; i++)
        {
            lifeImgs[i].alpha = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Wall"))
        {
            currObs = other.gameObject.GetComponent<ObstacleScript>();
            //AddDWGDestroyer();
        }
        if (other.gameObject.tag.Equals("Hole"))
        {
            currObs = other.gameObject.GetComponent<ObstacleScript>();
        }
        if (other.gameObject.tag.Equals("Cone"))
        {
            currObs = other.gameObject.GetComponent<ObstacleScript>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SetInteractableActionBtn(false);
        if (other.gameObject.tag.Equals("Wall"))
        {
            speed = normalState;
            currSpeed = normalState;
            if (carIdle && currObs != null)
            {
                OnMissTiming();
            }
            isCarBoost = false;
            currObs = null;
        }

        if (other.gameObject.tag.Equals("Hole"))
        {
            speed = normalState;
            currSpeed = normalState;
            if (carIdle && currObs != null)
            {
                OnMissTiming();
            }
            isCarBoost = false;
            currObs = null;
        }

        if (other.gameObject.tag.Equals("Cone"))
        {
            speed = normalState;
            currSpeed = normalState;
            if (carIdle && currObs != null)
            {
                OnMissTiming();
            }
            isCarBoost = false;
            currObs = null;
        }
    }

    public void SetInteractableActionBtn(bool isActive)
    {
        wallBoostBtn.interactable = isActive;
        coneBtn.interactable = isActive;
        holeBtn.interactable = isActive;
    }

    public void SetActiveFalseBtn(bool isActive)
    {
        wallBoostBtn.gameObject.SetActive(isActive);
        coneBtn.gameObject.SetActive(isActive);
        holeBtn.gameObject.SetActive(isActive);
    }

    public void WallBoost()
    {
        SetInteractableActionBtn(false);
        carIdle = false;
        AudioPlayer.instance.PlayPlayerAudio("CarRev");
        if (currObs.obstacleName.Equals("Wall"))
        {
            if (currObs.isPerfect)
            {
                PerfectTiming();
                isCarBoost = true;
                vfxScript.particles[0].Play();
            }
            else if (currObs.isGreat)
            {
                GreatTiming();
                isCarBoost = true;
                vfxScript.particles[0].Play();
            }
            else if (!currObs.isGreat && !currObs.isPerfect)
            {
                OnMissTiming();
            }
        }
        else
        {
            OnMissTiming();
        }
        currObs = null;
        carIdle = true;

        GameManagerScript.instance.ResumeTime();
        ResetButtons();

    }

    public void HoleJump()
    {
        SetInteractableActionBtn(false);
        carIdle = false;
        speed = stuntState;
        currSpeed = stuntState; 
        AudioPlayer.instance.PlayPlayerAudio("Jump");
        if (currObs.obstacleName.Equals("Hole"))
        {
            if (currObs.isPerfect)
            {
                PerfectTiming();
                playerAnim.SetTrigger("Hole");
                vfxScript.particles[1].Play();
            }
            else if (currObs.isGreat)
            {
                GreatTiming();
                playerAnim.SetTrigger("Hole");
                vfxScript.particles[1].Play();
            }
            else if (!currObs.isGreat && !currObs.isPerfect)
            {
                OnMissTiming();
            }
        }
        else
        {
            OnMissTiming();
        }
        currObs = null;
        carIdle = true;

        GameManagerScript.instance.ResumeTime();
        ResetButtons();

    }

    public void ConeSide()
    {
        carIdle = false;
        speed = stuntState;
        currSpeed = stuntState;
        SetInteractableActionBtn(false);
        AudioPlayer.instance.PlayPlayerAudio("Jump");
        if (currObs.obstacleName.Equals("Cone"))
        {
            if (currObs.isPerfect)
            {
                PerfectTiming();
                playerAnim.SetTrigger("Cone");
                vfxScript.particles[2].Play();
            }
            else if (currObs.isGreat)
            {
                GreatTiming();
                playerAnim.SetTrigger("Cone");
                vfxScript.particles[2].Play();
            }
            else if (!currObs.isGreat && !currObs.isPerfect)
            {
                OnMissTiming();
            }
        }
        else
        {
            OnMissTiming();
        }
        currObs = null;
        carIdle = true;


        GameManagerScript.instance.ResumeTime();
        ResetButtons();

    }


    public void PerfectTiming()
    {
        ShowTimingText("Perfect");
        GameManagerScript.instance.AddScore("Perfect");
        isCarSlowed = false;
        currObs.DisableObstacleCollider();
    }

    public void GreatTiming()
    {
        ShowTimingText("Great");
        GameManagerScript.instance.AddScore("Great");
        isCarSlowed = false;
        currObs.DisableObstacleCollider();
    }

    public void OnMissTiming()
    {
        SetInteractableActionBtn(false);
        ShowTimingText("Miss");
        GameManagerScript.instance.AddScore("Miss");
        playerAnim.SetTrigger("Miss");
        isCarSlowed = true;
        isCarBoost = false;
        vfxScript.particles[5].Play();
        currObs.DisableObstacleCollider();
        AudioPlayer.instance.PlayPlayerAudio("Miss");
    }

    public void ActivateBoostTap()
    {
        speed = 0;
        boostTapBtn.SetActive(true);
        StartCoroutine(OnBoostPhaseFinish());
        AudioPlayer.instance.PlayPlayerAudio("Tap4");
    }

    IEnumerator OnBoostPhaseFinish()
    {
        yield return new WaitForSeconds(4);
        boostTapBtn.SetActive(false);
        rb.isKinematic = false;
        if (isReady)
        {
            speed = lastState;
            currSpeed = lastState;
            vfxScript.particles[6].Play();
            AudioPlayer.instance.PlayPlayerAudio("SpeedUp");
        }
    }

    public void StopMovement()
    {
        speed = 0;
        rb.isKinematic = true;
    }

    public void AddDWGDestroyer()
    {
        destroyerScript = this.gameObject.AddComponent<DWGDestroyer>();
        destroyerScript.force = 20;
        destroyerScript.radius = 10;
    }

    //TutorialScript

    public void TutorialWall()
    {
        wallBoostBtn.gameObject.SetActive(true);
        holeBtn.gameObject.SetActive(false);
        coneBtn.gameObject.SetActive(false);

    }

    public void TutorialHole()
    {
        wallBoostBtn.gameObject.SetActive(false);
        holeBtn.gameObject.SetActive(true);
        coneBtn.gameObject.SetActive(false);

    }

    public void TutorialCone()
    {
        wallBoostBtn.gameObject.SetActive(false);
        holeBtn.gameObject.SetActive(false);
        coneBtn.gameObject.SetActive(true);

    }

    public void TutorialBoost()
    {
        Debug.Log("TutorialBoost triggered");
    }

    public void ResetButtons()
    {
        wallBoostBtn.gameObject.SetActive(true);
        holeBtn.gameObject.SetActive(true);
        coneBtn.gameObject.SetActive(true);
    }
}
