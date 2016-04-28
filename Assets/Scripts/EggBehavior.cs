using UnityEngine;
using System.Collections;

public class EggBehavior : MonoBehaviour
{

    private float mSpeed = 100f;
    static BossBackground globalBehavior;
    static GameObject mSplash;
    BossBackground.WorldBoundStatus status;
    void Start()
    {
        if (globalBehavior == null)
        {
            globalBehavior = GameObject.Find("GameManager").GetComponent<BossBackground>();
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

        if (status != BossBackground.WorldBoundStatus.Inside)
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
