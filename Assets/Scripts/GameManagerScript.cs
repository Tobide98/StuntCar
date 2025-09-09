using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public List<LevelScript> levels;
    public LevelScript currLevel;
    public PlayerControlScript playerConScript;
    public PlayerDataScript playerData;
    public Transform gameplayParent;
    public AdsManager adsManager;
    public float currScore = 0;
    public float currMoneyGet;
    public float endScore;
    public int currMultiplierGet;
    private int perfectCount;
    private int greatCount;
    private int missCount;
    public int levelNum;
    public int prevlevelNum;
    private bool isPause;

    [Header("UI Related")]
    public GameObject gameplayUI;
    public GameObject mainmenuUI;
    public GameObject preGameplayUI;
    public GameObject pauseUI;
    public Image speedBar;
    public TextMeshProUGUI speedScoreText;
    public TextMeshProUGUI levelText;
    public Button pauseBtn;

    [Header("End Game Screen UI")]
    public GameObject endGameScreenUI;
    public TextMeshProUGUI totalPerfectText;
    public TextMeshProUGUI totalGreatText;
    public TextMeshProUGUI totalMissText;
    public TextMeshProUGUI multiplierGetText;
    public TextMeshProUGUI totalScoreGetText;
    public TextMeshProUGUI totalMoneyGetText;
    public GameObject nextLevelBtn;
    public GameObject retryLevelBtn;
    public GameObject doubleRewardAdsBtn;
    public Sprite winImg;
    public Sprite loseImg;
    public Image EndGameTitleImg;

    [Header("Tutorial")]
    public TutorialScript tutorialScript;
    public bool isTutorial;

    [Header("Analytics")]
    public FacebookInitScript facebookScript;

    public static GameManagerScript instance;


    void Start()
    {
        initGame();
        levelNum = 0;
        AudioPlayer.instance.PlayBgm("BGM");
    }

    public void initGame()
    {
        currLevel = Instantiate(levels[levelNum].gameObject, gameplayParent).GetComponent<LevelScript>();
        ResetState();
    }

    public void StartGame()
    {
        playerConScript.StartPlayer();
        mainmenuUI.SetActive(false);
        preGameplayUI.SetActive(false);
        gameplayUI.SetActive(true);
        AudioPlayer.instance.PlayPlayerAudio("CarRev");
        CheckLevelTutorial();

        AnalyticManager.instance.LogEvent("Start ", "Level ", prevlevelNum.ToString());
    }

    public void CheckLevelTutorial()
    {
        if(prevlevelNum == 0)
        {
            isTutorial = true;
            playerConScript.rb.isKinematic = true;
        }
        else
        {
            isTutorial = false;
        }
    }

    public void RestartGame()
    {
        if (currLevel != null) Destroy(currLevel.gameObject);
        currLevel = Instantiate(levels[prevlevelNum].gameObject, gameplayParent).GetComponent<LevelScript>();
        ResetState();
        preGameplayUI.SetActive(true);
        CheckLevelTutorial();

        AnalyticManager.instance.LogEvent("Retry ", "Level ", prevlevelNum.ToString());
    }

    public void NextLevel()
    {
        if (currLevel != null) Destroy(currLevel.gameObject);
        if (levelNum >= 5) levelNum = 0;
        currLevel = Instantiate(levels[levelNum].gameObject, gameplayParent).GetComponent<LevelScript>();
        prevlevelNum = levelNum;
        ResetState();
        preGameplayUI.SetActive(true);
    }

    public void ResetState()
    {
        gameplayUI.SetActive(false);
        endGameScreenUI.SetActive(false);
        playerConScript.ResetStatePlayer();
        currMultiplierGet = 1;
        currScore = 100;
        currMoneyGet = 0;
        ResetTimingCounter();
        if(prevlevelNum == 0) levelText.text = "tutorial ";
        else levelText.text = "Level " + (prevlevelNum).ToString();
        RefreshScore();
    }

    public void AddScore(string timing)
    {
        switch (timing) 
        {
            case "Perfect": currScore += 15;
                perfectCount++;
                break;
            case "Great": currScore += 10;
                greatCount++;
                break;
            case "Miss": currScore -= 10;
                missCount++;
                playerConScript.life--; 
                playerConScript.CheckCarLife(); 
                break;
        }
        RefreshScore();
    }

    public void ResetTimingCounter()
    {
        perfectCount = 0;
        greatCount = 0;
        missCount = 0;
    }

    public void RefreshScore()
    {
        speedBar.fillAmount = currScore / 300;
        //if (currScore >= 300) currScore = 300;
        speedScoreText.text = currScore.ToString();
    }

    public void GameOver(bool isWin)
    {
        isPause = false;
        pauseUI.SetActive(false);
        playerConScript.rb.isKinematic = true;
        pauseBtn.interactable = true;
        currMoneyGet = currScore / 10.3f;
        var totalScoreGet = currScore * currMultiplierGet;
        //to string ("F0")
        totalPerfectText.text = "x" + perfectCount.ToString();
        totalGreatText.text = "x"+greatCount.ToString();
        totalMissText.text = "x" + missCount.ToString();
        totalScoreGetText.text = totalScoreGet.ToString("F0");
        multiplierGetText.text = "x" + currMultiplierGet.ToString();
        totalMoneyGetText.text = "+"+currMoneyGet.ToString("F0");
        if (isWin)
        {
            levelNum = prevlevelNum + 1;
            nextLevelBtn.SetActive(true);
            retryLevelBtn.SetActive(false);
            doubleRewardAdsBtn.SetActive(true);
            EndGameTitleImg.sprite = winImg;
            facebookScript.LogAchieveLevelEvent(currLevel.ToString());
            AnalyticManager.instance.LogEvent("Complete ", "Level ",prevlevelNum.ToString());
            playerData.AddMoneyPlayer(currMoneyGet);
        }
        else
        {
            nextLevelBtn.SetActive(false);
            retryLevelBtn.SetActive(true);
            doubleRewardAdsBtn.SetActive(false);
            EndGameTitleImg.sprite = loseImg;
            AnalyticManager.instance.LogEvent("Fail ", "Level ", prevlevelNum.ToString());
        }
        StartCoroutine(ShowEndGameScreen());
    }

    IEnumerator ShowEndGameScreen()
    {
        yield return new WaitForSeconds(2.5f);
        endGameScreenUI.SetActive(true);
    }

    public void OpenPauseMenu()
    {
        isPause = true;
        pauseUI.SetActive(true);
        playerConScript.rb.isKinematic = true;
    }

    public void ContinueGame()
    {
        isPause = false;
        pauseUI.SetActive(false);
        playerConScript.rb.isKinematic = false;
        playerConScript.speed = playerConScript.currSpeed;
    }

    public void ReturnToMenu()
    {
        isPause = false;
        if (currLevel != null) Destroy(currLevel.gameObject);
        currLevel = Instantiate(levels[prevlevelNum].gameObject, gameplayParent).GetComponent<LevelScript>();
        ResetState();
        pauseUI.SetActive(false);
        mainmenuUI.SetActive(true);
    }


    void Update()
    {
        if(isPause) playerConScript.speed = 0;
    }

    public void doubleRewardBtn()
    {
        adsManager.WatchInterstitial(OnDOubleRewardActivated);
        AnalyticManager.instance.LogEvent("Ads_Clicked");
    }

    public void OnDOubleRewardActivated()
    {
        playerData.AddMoneyPlayer(currMoneyGet);
        currMoneyGet *= 2;
        totalMoneyGetText.text = currMoneyGet.ToString("F0");
        AnalyticManager.instance.LogEvent("Ads_Watched");
        doubleRewardAdsBtn.SetActive(false);

    }

    private void OnEnable()
    {
        instance = this;

    }

    private void OnDisable()
    {
        instance = null;
    }

    public void CheckTutorial(string objName)
    {
        if (isTutorial == true)
        {
            isPause = true;
            if (objName == "Wall")
            { 
                tutorialScript.DisplayTutorialTextFence(); 
            }
            if(objName == "Hole")
            {
                tutorialScript.DisplayTutorialTextHole();
            }
            if (objName == "Cone")
            {
                tutorialScript.DisplayTutorialTextCone();
            }
            playerConScript.SetInteractableActionBtn(true);
        }
    }

    public void BoostTutorial()
    {
        if(isTutorial == true)
        {
            isPause = true;
            Debug.Log("Pausing in boost");
            tutorialScript.DisplayBoostTutorial();
        }
    }

    public void ResumeTime()
    {
        if(isPause == true)
        isPause = false;
    }
}
