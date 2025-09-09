using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDataScript : MonoBehaviour
{
    private float totalMoney;
    public TextMeshProUGUI moneyText;
    // Start is called before the first frame update
    void Start()
    {
        GetPlayerData();
        UpdateDataVisual();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void resetData()
    {
        totalMoney = 0;
    }

    public void GetPlayerData()
    {
        resetData();
        if (PlayerPrefs.HasKey("Money")) totalMoney = PlayerPrefs.GetFloat("Money");
    }

    public void AddMoneyPlayer(float moneyAdded)
    {
        totalMoney += moneyAdded;
        PlayerPrefs.SetFloat("Money", totalMoney);
        UpdateDataVisual();
    }

    public void UpdateDataVisual()
    {
        moneyText.text = totalMoney.ToString("F0");
    }

}
