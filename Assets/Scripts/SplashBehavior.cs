using UnityEngine;
using System.Collections;

public class SplashBehavior : MonoBehaviour 
{
    public static float timeAlive = 0.1f;
    public AudioSource mAudioEffect = null;
    float timer;
	// Use this for initialization
	void Start () 
    {
        timer = 0;
        if (mAudioEffect == null)
        {
            mAudioEffect = GetComponent<AudioSource>();
            mAudioEffect.Play();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;
        if(timer > timeAlive)
        {
            Destroy(gameObject);
        }
	}
}
