using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenScript : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    public void CallEventOnFinish()
    {
        this.gameObject.SetActive(false);
    }
}
