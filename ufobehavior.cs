using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ufobehavior : MonoBehaviour
{
    public GameObject mProjectile = null;
    public Vector3 offset = new Vector3(0, 0, 0); // Defined in object settings
    private Vector3 worldOffset; // Multiplied by rotation
    private Animator animator;
    private float kHeroRotateSpeed = 90f;
    private float kHeroSpeed = 50f;
    private BossBackground.WorldBoundStatus status;
    private static BossBackground globalBehavior;
    private int wait = 0;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        if (null == mProjectile)
        {
            mProjectile = Resources.Load("Prefabs/Laser") as GameObject;
        }
        if (globalBehavior == null)
        {
            globalBehavior = GameObject.Find("GameManager").GetComponent<BossBackground>();
        }
    }

    float speed = 4.0f;

    // Update is called once per frame
    void Update()
    {
        wait++;
        status = globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
        if (status != BossBackground.WorldBoundStatus.Inside)
        {
            transform.position -= 3 * Input.GetAxis("Vertical") * transform.up * (kHeroSpeed * Time.smoothDeltaTime);
        }

        transform.position += Input.GetAxis("Vertical") * transform.up * (kHeroSpeed * Time.smoothDeltaTime);
        transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") * (kHeroRotateSpeed * Time.smoothDeltaTime));

        if (Input.GetAxis("Fire1") > 0f && wait > 5)  // this is Left-Control
        {
            wait = 0;
            GameObject e = Instantiate(mProjectile) as GameObject;
            EggBehavior egg = e.GetComponent<EggBehavior>(); // Shows how to get the script from GameObject
            if (null != egg)
            {
                worldOffset = transform.rotation * offset;
                e.transform.position = transform.position + worldOffset;
                egg.SetForwardDirection(transform.up);
            }

        }

    }
}