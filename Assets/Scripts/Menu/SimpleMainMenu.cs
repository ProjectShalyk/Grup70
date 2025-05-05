using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene"; // Set this to your game scene name
    [SerializeField] private string creditsSceneName = "Credits"; // Set this to your credits scene name

    // Load the game scene
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Load the credits scene
    public void ShowCredits()
    {
        SceneManager.LoadScene(creditsSceneName);
    }

    // Quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}