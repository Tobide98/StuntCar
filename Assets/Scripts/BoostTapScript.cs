using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoostTapScript : MonoBehaviour
{
    public float totalTap;
    public float speedGet;
    public TextMeshProUGUI totalTapText;
    public TextMeshProUGUI totalSpeedGetText;
    // Start is called before the first frame update
    void Start()
    {
        totalTap = 0;
        speedGet = 0;
    }
    private void OnEnable()
    {
        totalTap = 0;
        speedGet = 0;
        totalTapText.text = " ";
        totalSpeedGetText.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            totalTap++;
            speedGet += 2;
            GameManagerScript.instance.currScore += 2;
            totalTapText.text = totalTap.ToString();
            totalSpeedGetText.text = "+" + speedGet.ToString();
            GameManagerScript.instance.RefreshScore();
            GameManagerScript.instance.playerConScript.vfxScript.particles[3].Play();
            GameManagerScript.instance.playerConScript.vfxScript.particles[8].Play();
        }


            GameManagerScript.instance.ResumeTime();
        
    }
}
