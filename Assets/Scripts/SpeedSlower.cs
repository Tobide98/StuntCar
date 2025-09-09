using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSlower : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (!GameManagerScript.instance.playerConScript.isCarSlowed)
            {
                GameManagerScript.instance.playerConScript.speed = 20f;
            }
            this.gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
