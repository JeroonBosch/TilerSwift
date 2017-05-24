using UnityEngine;
using UnityEngine.SceneManagement;

public class RootController : MonoBehaviour {
    private Level_Settings config;
    private static RootController instance; //Singleton
    private AudioSource Audio;
    private GameManager gameManager;
    private bool controlsEnabled;

    public static RootController Instance
    {
        get { return instance; }
    }

    public void Quit()
    {
        Application.Quit();
    }

    void Awake()
    {
        instance = this;
        Audio = GameObject.Find("Audio").GetComponent<AudioSource>();
        config = gameObject.GetComponent<Level_Settings>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public AudioSource AudioController()
    {
        return Audio;
    }

    public GameManager GameManager()
    {
        return gameManager;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void EnableControls()
    {
        controlsEnabled = true;
    }

    public void DisableControls()
    {
        controlsEnabled = false;
    }

    public Level_Settings GetConfigFile()
    {
        return config;
    }
}