using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneSupport : MonoBehaviour
{

    public string LevelName = null;

    public Button mStartButton;
    public Button mQuitButton;
    public Button mCreditsButton;

    void Start ()
    {
        mStartButton = GameObject.Find("StartButton").GetComponent<Button>();
        mQuitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        mCreditsButton = GameObject.Find("CreditsButton").GetComponent<Button>();

        mStartButton.onClick.AddListener(
            () =>
            {
                LoadScene("LevelOne");
            });

        mQuitButton.onClick.AddListener(
            () =>
            {
                Application.Quit();
            });

        mCreditsButton.onClick.AddListener(
            () =>
            {
                LoadScene("Credits");
            });
    }

    void Update ()
    {

    }

    void LoadScene(string theLevel)
    {
        SceneManager.LoadScene(theLevel);
//        FirstGameManager.TheGameState.SetCurrentLevel(theLevel);
        
    }
}
