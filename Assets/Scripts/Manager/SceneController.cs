using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    
    //  SceneList
    public enum Scene
    {
        Main,
        Lobby,
        InGame,
        Result
    }

    #region Singleton
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    //  Not Overloading to Par-Int
    public void LoadScene(Scene sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}