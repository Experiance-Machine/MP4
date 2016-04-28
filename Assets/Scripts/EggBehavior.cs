using UnityEngine;
using System.Collections;

public class EggBehavior : MonoBehaviour
{

    private float mSpeed = 100f;
    static GlobalBehavior globalBehavior;
    static GameObject mSplash;
    GlobalBehavior.WorldBoundStatus status;
    void Start()
    {
        if (globalBehavior == null)
        {
            globalBehavior = FirstGameManager.TheGameState;
        }
        if (mSplash == null)
        {
            mSplash = Resources.Load("Prefabs/Splash") as GameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;

        status = globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);

        if (status != GlobalBehavior.WorldBoundStatus.Inside)
        {
            // Debug.Log("collided position: " + this.transform.position);
            GameObject e = Instantiate(mSplash, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
            Destroy(gameObject);
        }
    }

    public void SetForwardDirection(Vector3 f)
    {
        transform.up = f;
    }
}
