using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DWGDestroyerAdder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            GameManagerScript.instance.playerConScript.AddDWGDestroyer();
            GameManagerScript.instance.playerConScript.rb.isKinematic = false;
            Destroy(this.gameObject);
        }
    }
}
