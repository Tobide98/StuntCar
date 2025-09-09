using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPhaseCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("BoostCheck triggered");
            GameManagerScript.instance.pauseBtn.interactable = false;
            GameManagerScript.instance.playerConScript.StopMovement();
            GameManagerScript.instance.playerConScript.isReady = true;
            GameManagerScript.instance.playerConScript.ActivateBoostTap();
            GameManagerScript.instance.playerConScript.SetActiveFalseBtn(false);
            GameManagerScript.instance.playerConScript.lifeParentUI.SetActive(false);
            GameManagerScript.instance.BoostTutorial();
            //GameManagerScript.instance.playerConScript.TutorialBoost();
            this.gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
