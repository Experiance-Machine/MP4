﻿/* Copyrights:
 *  Background:     https://www.youtube.com/watch?v=-KLOrIu1RC4
 *  Alien/Laser:    http://opengameart.org/content/alien-ufo-pack
 *  Enemies:        http://opengameart.org/content/match-tiles-emoji-asset-public-domain
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossBackground : MonoBehaviour
{

    #region World Bound support
    private Bounds mWorldBound;  // this is the world bound
    private Vector2 mWorldMin;  // Better support 2D interactions
    private Vector2 mWorldMax;
    private Vector2 mWorldCenter;
    private Camera mMainCamera;
    #endregion

    #region  support runtime enemy creation
    // to support time ...
    private float mPreEnemySpawnTime = -1f; // 
    private Vector3 randomPosition;
    public const float kEnemySpawnInterval = 3.0f; // in seconds
    public bool frozen = false;


    // spwaning enemy ...
    public GameObject mEnemyToSpawn = null;
    #endregion

    public Text echoText;
    private int enemyCount;
    private int laserCount;
    public static int abductCount;

    //private bool toggleFrozen;

    // Use this for initialization
    void Start()
    {

        #region world bound support
        mMainCamera = Camera.main;
        mWorldBound = new Bounds(Vector3.zero, Vector3.one);
        UpdateWorldWindowBound();
        #endregion


        //toggleFrozen = false;
        //echoText.text = "";
    }

    /*void SetEchoText()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        laserCount = GameObject.FindGameObjectsWithTag("Laser").Length;
        echoText.text = "Emoji: " + enemyCount + ", Lasers: " + laserCount + ", Abducted: " + abductCount;
    }*/

    // Update is called once per frame
    void Update()
    {
        /*
        if (!frozen)
        {
            //SpawnAnEnemy();
        }

		if (Input.GetAxis("Jump") > 0 && !toggleFrozen) 
        {
            //frozen = !frozen;
            toggleFrozen = true;
		}
        else if (Input.GetAxis("Jump") <= 0)
        {
            toggleFrozen = false;
        }
        */
        if ((Input.GetAxis("Cancel") > 0) || Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
            GlobalBehavior.score = 0;
        }
        //SetEchoText();
    }


    #region Game Window World size bound support
    public enum WorldBoundStatus
    {
        CollideTop,
        CollideLeft,
        CollideRight,
        CollideBottom,
        Outside,
        Inside
    };

    /// <summary>
    /// This function must be called anytime the MainCamera is moved, or changed in size
    /// </summary>
    public void UpdateWorldWindowBound()
    {
        // get the main 
        if (null != mMainCamera)
        {
            float maxY = mMainCamera.orthographicSize;
            float maxX = mMainCamera.orthographicSize * mMainCamera.aspect;
            float sizeX = 2 * maxX;
            float sizeY = 2 * maxY;
            float sizeZ = Mathf.Abs(mMainCamera.farClipPlane - mMainCamera.nearClipPlane);

            // Make sure z-component is always zero
            Vector3 c = mMainCamera.transform.position;
            c.z = 0.0f;
            mWorldBound.center = c;
            mWorldBound.size = new Vector3(sizeX, sizeY, sizeZ);

            mWorldCenter = new Vector2(c.x, c.y);
            mWorldMin = new Vector2(mWorldBound.min.x, mWorldBound.min.y);
            mWorldMax = new Vector2(mWorldBound.max.x, mWorldBound.max.y);
        }
    }

    public Vector2 WorldCenter { get { return mWorldCenter; } }
    public Vector2 WorldMin { get { return mWorldMin; } }
    public Vector2 WorldMax { get { return mWorldMax; } }

    public WorldBoundStatus ObjectCollideWorldBound(Bounds objBound)
    {
        WorldBoundStatus status = WorldBoundStatus.Inside;

        if (mWorldBound.Intersects(objBound))
        {
            if (objBound.max.x > mWorldBound.max.x)
                status = WorldBoundStatus.CollideRight;
            else if (objBound.min.x < mWorldBound.min.x)
                status = WorldBoundStatus.CollideLeft;
            else if (objBound.max.y > mWorldBound.max.y)
                status = WorldBoundStatus.CollideTop;
            else if (objBound.min.y < mWorldBound.min.y)
                status = WorldBoundStatus.CollideBottom;
            else if ((objBound.min.z < mWorldBound.min.z) || (objBound.max.z > mWorldBound.max.z))
                status = WorldBoundStatus.Outside;
        }
        else
            status = WorldBoundStatus.Outside;
        return status;

    }
    #endregion

}