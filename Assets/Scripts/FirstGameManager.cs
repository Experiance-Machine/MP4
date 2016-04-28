using UnityEngine;
using System.Collections;

public class FirstGameManager : MonoBehaviour {

    private static GlobalBehavior theGameState = null;

    private static void CreateGlobalManager()
    {
        GameObject newGameState = new GameObject();
        newGameState.name = "GlobalStateManager";
        newGameState.AddComponent<GlobalBehavior>();
        theGameState = newGameState.GetComponent<GlobalBehavior>();
    }

    public static GlobalBehavior TheGameState
    {
        get
        {
            return theGameState;
        }
    }

    void Awake()
    {
        if (null == theGameState)
        {
            CreateGlobalManager();
        }
    }
	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this);
	}
}
