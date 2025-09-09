using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingDetection : MonoBehaviour
{
    public ObstacleScript obsScript;
    public string timing;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            switch (timing)
            {
                case "perfect": obsScript.isPerfect = true; obsScript.isGreat = false; break;
                case "great": obsScript.isPerfect = false; obsScript.isGreat = true;  break; //Handheld.Vibrate();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            obsScript.isGreat = false;
            obsScript.isPerfect = false;
        }
    }
}
