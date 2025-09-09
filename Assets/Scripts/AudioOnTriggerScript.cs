using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOnTriggerScript : MonoBehaviour
{
    public string audioId;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            AudioPlayer.instance.PlayPlayerAudio(audioId);
        }
    }
}
