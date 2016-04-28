using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Level1GameState : MonoBehaviour {

    GlobalBehavior globalBehavior = FirstGameManager.TheGameState;
    
    #region  support runtime enemy creation
    // to support time ...
    private float mPreEnemySpawnTime = -1f; // 
    private Vector3 randomPosition;
    public const float kEnemySpawnInterval = 3.0f; // in seconds
    

    // spwaning enemy ...
    public GameObject mEnemyToSpawn = null;
    #endregion

    private int laserCount;
    public static int abductCount;

    // Use this for initialization
    void Start ()
    {
        #region initialize enemy spawning
        if (null == mEnemyToSpawn)
            mEnemyToSpawn = Resources.Load("Prefabs/Enemy") as GameObject;
        #endregion

        for (int i = 0; i < 5; i++)
        {
            randomPosition = new Vector3(Random.Range(globalBehavior.mWorldMin.x, globalBehavior.mWorldMax.x), Random.Range(globalBehavior.mWorldMin.y, globalBehavior.mWorldMax.y), 0.0f);
            GameObject e = Instantiate(mEnemyToSpawn, randomPosition, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if ((Input.GetAxis("Cancel") > 0) || Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
            GlobalBehavior.score = 0;
        }
        if (GameObject.Find("Enemy(Clone)") == null)
        {
            SceneManager.LoadScene("boss_scene");
        }
    }

    #region enemy spawning support
    private void SpawnAnEnemy()
    {
        if ((Time.realtimeSinceStartup - mPreEnemySpawnTime) > kEnemySpawnInterval)
        {
            randomPosition = new Vector3(Random.Range(globalBehavior.mWorldMin.x, globalBehavior.mWorldMax.x), Random.Range(globalBehavior.mWorldMin.y, globalBehavior.mWorldMax.y), 0.0f);
            GameObject e = Instantiate(mEnemyToSpawn, randomPosition, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
            mPreEnemySpawnTime = Time.realtimeSinceStartup;
        }
    }
    #endregion
}
