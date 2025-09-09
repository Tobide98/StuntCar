using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDisabler : MonoBehaviour
{
    public List<Collider> colliders;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            DisableAllColliders();
        }
    }

    private void DisableAllColliders()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].enabled = false;
        }
    }
}
