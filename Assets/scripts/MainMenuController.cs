using UnityEngine;
using UnityEngine.SceneManagement; // Needed to load scenes

public class MainMenuController : MonoBehaviour
{
    // This function will be called when the Start button is clicked.
    public void StartGame()
    {
        // Goes to the game, will adjust later if we need to goto a seperate scene later one.
        SceneManager.LoadScene("BattleScene");
    }

    // Optional: A function to exit the game. 
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting.");
    }
}