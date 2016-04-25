using UnityEngine;	
using System.Collections;

public class InteractiveControl : MonoBehaviour 
{

	public GameObject mProjectile = null;
    public AudioSource mAudioEffect = null;
    public Vector3 offset = new Vector3(0, 0, 0); // Defined in object settings
    private Vector3 worldOffset; // Multiplied by rotation
    private GlobalBehavior.WorldBoundStatus status;
    private static GlobalBehavior globalBehavior;

	#region user control references
	private float kHeroSpeed = 50f;
	private float kHeroRotateSpeed = 180f; // 90-degrees in 2 seconds
	#endregion
	// Use this for initialization
	void Start () 
    {
		// initialize projectile spawning
        if (null == mProjectile)
        {
            mProjectile = Resources.Load("Prefabs/Laser") as GameObject;
        }
        if (globalBehavior == null)
        {
            globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		#region user movement control
        transform.position += Input.GetAxis("Vertical") * transform.up * (kHeroSpeed * Time.smoothDeltaTime);
		status = globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
        if (status != GlobalBehavior.WorldBoundStatus.Inside)
        {
            transform.position -= 3 * Input.GetAxis("Vertical") * transform.up * (kHeroSpeed * Time.smoothDeltaTime);
        }
		transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") * (kHeroRotateSpeed * Time.smoothDeltaTime));
		#endregion

        if (Input.GetAxis("Fire1") > 0f)  // this is Left-Control
        {
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
