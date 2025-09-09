using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public string obstacleName;
    public float elapsedTime;
    private bool isTriggerOn;
    public GameObject obj;
    public List<Collider> colliders;

    public bool isPerfect;
    public bool isGreat;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
        isTriggerOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(isTriggerOn) elapsedTime += Time.deltaTime;
        //if (elapsedTime >= 0.1 && elapsedTime <= 0.2)
        //{
        //    Handheld.Vibrate();
        //    //Debug.Log("Vibrate on: " + elapsedTime);
        //}
        if (isTriggerOn)
        {
            if (isPerfect)
            {
                GameManagerScript.instance.CheckTutorial(this.obstacleName);
                isTriggerOn = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isTriggerOn = true;
        }
        Debug.Log(this.obstacleName);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isTriggerOn = false;
            RemoveCollider();
            DisableObstacleCollider();
            StartCoroutine(DestroyThisObject());
        }
    }

    IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds(1);
        if(obj != null) Destroy(obj);
    }

    public void RemoveCollider()
    {
        if(colliders.Count >= 1)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].enabled = false;
                colliders[i].isTrigger = true;
            }
        }
    }

    public void DisableObstacleCollider()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
    }
}
