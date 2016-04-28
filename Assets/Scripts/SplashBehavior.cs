using UnityEngine;
using System.Collections;

public class SplashBehavior : MonoBehaviour 
{
    public static float timeAlive = 0.1f;
    float timer;
	// Use this for initialization
	void Start () 
    {
        timer = 0;
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
