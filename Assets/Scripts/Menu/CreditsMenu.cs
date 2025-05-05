using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditsMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    
    // Return to main menu
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
