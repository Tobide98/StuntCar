using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayTutorialTextFence()
    {
        Debug.Log("Hey! Bash through this fence!");
        GameManagerScript.instance.playerConScript.TutorialWall();
        //playerControlScript.TutorialWall();
    }

    public void DisplayTutorialTextHole()
    {
        Debug.Log("Hey! Bash through this hole!");
        GameManagerScript.instance.playerConScript.TutorialHole();

    }

    public void DisplayTutorialTextCone()
    {
        Debug.Log("Hey! Bash through this cone!");
        GameManagerScript.instance.playerConScript.TutorialCone();
    }

    public void DisplayBoostTutorial()
    {
        Debug.Log("Hey! Tap on the screen to increase your multiplier");

    }
}
