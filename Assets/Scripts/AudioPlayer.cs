using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SEMISOFT.AudioSystem;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer instance;
    public AudioSource SFX;
    public AudioSource BGM;
    public AudioSource PlayerSFX;

    public bool SFXon;
    public string sfxState;

    public GameObject SFXOnBtn;
    public GameObject SFXOffBtn;

    public void Awake()
    {
        instance = this;
        StartCoroutine(AudioSystem.LoadAllAudio());
    }

    private void OnDestroy()
    {

    }

    private void Start()
    {
        CheckSettings();
    }

    public void PlayBgm(string name)
    {
        AudioEventEnum myEnum = (AudioEventEnum)Enum.Parse(typeof(AudioEventEnum), name);
        AudioSystem.PlayAudioLoop(BGM.gameObject, myEnum);
    }

    public void PlayAudio(string name)
    {
        AudioEventEnum myEnum = (AudioEventEnum)Enum.Parse(typeof(AudioEventEnum), name);
        AudioSystem.PlayAudioOneShot(SFX.gameObject, myEnum);
    }

    public void PlayAudioLoop(string name)
    {
        AudioEventEnum myEnum = (AudioEventEnum)Enum.Parse(typeof(AudioEventEnum), name);
        AudioSystem.PlayAudioLoop(SFX.gameObject, myEnum);
    }

    public void PlayAudio(AudioEventEnum audioEvent)
    {
        AudioSystem.PlayAudioOneShot(SFX.gameObject, audioEvent);
    }

    public void PlayPlayerAudio(string name)
    {
        AudioEventEnum myEnum = (AudioEventEnum)Enum.Parse(typeof(AudioEventEnum), name);
        AudioSystem.PlayAudioOneShot(PlayerSFX.gameObject, myEnum);
    }

    public void PlayLoopPlayerAudio(string name)
    {
        AudioEventEnum myEnum = (AudioEventEnum)Enum.Parse(typeof(AudioEventEnum), name);
        AudioSystem.PlayAudioLoop(PlayerSFX.gameObject, myEnum);
    }

    public void StopSFXAudio()
    {
        SFX.Stop();
    }

    public void StopPlayerSFXAudio()
    {
        PlayerSFX.Stop();
    }


    public void TurnOffSFX()
    {
        sfxState = "off";
        PlayerPrefs.SetString("sfxState", sfxState);
        SFX.mute = true;
        SFXOffBtn.SetActive(true);
        SFXOnBtn.SetActive(false);
    }

    public void TurnOnSFX()
    {
        sfxState = "on";
        PlayerPrefs.SetString("sfxState", sfxState);
        SFX.mute = false;
        //SFXOnBtn.SetActive(true);
        //SFXOffBtn.SetActive(false);
    }
   
    public void CheckSettings()
    {
        if (PlayerPrefs.HasKey("sfxState"))
        {
            sfxState = PlayerPrefs.GetString("sfxState");
            if (sfxState.Equals("on")) TurnOnSFX();
            else if (sfxState.Equals("off")) TurnOffSFX();
        }
        else TurnOnSFX();
    }

}
