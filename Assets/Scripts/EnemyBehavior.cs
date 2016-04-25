using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour 
{
	
	private const float kReferenceSpeed = 20f;

	public float mSpeed = kReferenceSpeed;
	public float mTowardsCenter = 0.5f;
    private static GameObject mAbduct;
    public Vector3 enemyPortalOffset = new Vector3(0, 3.5f, 0); // Defined in object settings
    private Vector3 worldPortalOffset; // Multiplied by rotation
    private SpriteRenderer renderer;
    private static Sprite spriteDefault;
    private static Sprite spriteStunned;
    private static Sprite spriteAfraid;
    private Vector3 run;
    private Vector3 mRotation;

    private static GameObject hero;
    private Vector3 heroDiff;
    private float heroDot;
    public float maxScare = 30f;


    enum stateType {Stunned, Afraid, Default}
    private stateType state;

    private static GlobalBehavior globalBehavior;
    private GlobalBehavior.WorldBoundStatus status;

    private float stunTimer;
    private int stunCount;
    public int stunMax = 3;
    public float stunDuration = 5.0f; // in seconds
		
	// Use this for initialization
	void Start () 
    {
        if (globalBehavior == null)
        {
            globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
        }

		NewDirection();
        if(mAbduct == null)
            mAbduct = Resources.Load("Prefabs/AbductSplash") as GameObject;

        if(hero == null)
            hero = GameObject.Find("Hero") as GameObject;

        renderer = GetComponent<SpriteRenderer>();
        if (null != renderer)
        {
            spriteDefault = Resources.Load("Textures/enemyDefault", typeof(Sprite)) as Sprite;
            spriteStunned = Resources.Load("Textures/enemyStunned", typeof(Sprite)) as Sprite;
            spriteAfraid = Resources.Load("Textures/enemyAfraid", typeof(Sprite)) as Sprite;
        }

        state = stateType.Default;
	}

	// Update is called once per frame
    void Update () 
    {
        heroDiff = hero.transform.position - transform.position;
        heroDot = Vector2.Dot(heroDiff, -hero.transform.up);

        if (state == stateType.Default)
        {
            if (heroDiff.magnitude < maxScare && heroDot > 0)
            {
                state = stateType.Afraid;
                renderer.sprite = spriteAfraid;
            }
            transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
        }
        else if(state == stateType.Stunned)
        {
            mRotation = transform.rotation.eulerAngles;
            mRotation.z += (9 * Time.deltaTime);
            transform.rotation = Quaternion.Euler(mRotation);

            stunTimer += Time.deltaTime;
            if(stunTimer > stunDuration)
            {
                state = stateType.Default;
                renderer.sprite = spriteDefault;
            }
        }
        else if(state == stateType.Afraid)
        {

            if (heroDiff.magnitude > maxScare || heroDot < 0)
            {
                state = stateType.Default;
                renderer.sprite = spriteDefault;
            }

            run = (mSpeed * .2f * Time.smoothDeltaTime) * (-1 * heroDiff).normalized;

            transform.position += run;

            //transform.rotation = Quaternion.AngleAxis(rotateAngle, transform.forward);
            var angle = Mathf.Atan2(run.y, run.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, transform.forward);
            transform.Rotate(Vector3.forward * -90);

        }
        
        status = globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
			
        if (status != GlobalBehavior.WorldBoundStatus.Inside) 
        {
            NewDirection();
        }
    }

	// New direction will be something randomly within +- 45-degrees away from the direction
	// towards the center of the world
	private void NewDirection() 
    {

		// we want to move towards the center of the world
		Vector2 v = globalBehavior.WorldCenter - new Vector2(transform.position.x, transform.position.y);  
				// this is vector that will take us back to world center
		v.Normalize();
		Vector2 vn = new Vector2(v.y, -v.x); // this is a direciotn that is perpendicular to V

		float useV = 1.0f - Mathf.Clamp(mTowardsCenter, 0.01f, 1.0f);
		float tanSpread = Mathf.Tan( useV * Mathf.PI / 2.0f );

		float randomX = Random.Range(0f, 1f);
		float yRange = tanSpread * randomX;
		float randomY = Random.Range (-yRange, yRange);

		Vector2 newDir = randomX * v + randomY * vn;
		newDir.Normalize();
		transform.up = newDir;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		// Only care if hitting a Laser (vs. hitting another Enemy!
		if (other.gameObject.name == "Laser(Clone)") 
        {
            worldPortalOffset = other.gameObject.transform.rotation * enemyPortalOffset;
            GameObject e = Instantiate(mAbduct, other.gameObject.transform.position + worldPortalOffset, other.gameObject.transform.rotation) as GameObject;
            Destroy(other.gameObject);
            
            Stunned();
		}
	}

    void Stunned()
    {
        state = stateType.Stunned;
        if (null != renderer)
        {
            renderer.sprite = spriteStunned;
        }
        stunTimer = 0;

        stunCount++;
        if(stunCount > stunMax)
        {
            GlobalBehavior.abductCount++;
            Destroy(gameObject);
        }
    }
	
}
