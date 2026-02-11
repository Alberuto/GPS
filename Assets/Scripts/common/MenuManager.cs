using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public void LoadGameScene() {
        SceneManager.LoadScene("1 game");
    }
    public void QuitGame() {
        Application.Quit();
    }
}