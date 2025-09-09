using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierCheckerWall : MonoBehaviour
{
    // Start is called before the first frame update
    public float minimumScore;
    public int multiplier;
    public GameObject blocker;
    public ParticleSystem celebrationFx;
    void Start()
    {
        blocker.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            AudioPlayer.instance.PlayPlayerAudio("BreakWall");
            if(GameManagerScript.instance.currScore < minimumScore)
            {
                GameManagerScript.instance.playerConScript.speed = 0;
                GameManagerScript.instance.playerConScript.rb.isKinematic = true;
                GameManagerScript.instance.currMultiplierGet = multiplier;
                GameManagerScript.instance.playerConScript.vfxScript.particles[6].Stop();
                GameManagerScript.instance.GameOver(true);
                celebrationFx.Play();
                Destroy(this.gameObject);
            }
            else
            {
                blocker.SetActive(false);
                GameManagerScript.instance.currMultiplierGet = multiplier;
            }
        }
    }
}
