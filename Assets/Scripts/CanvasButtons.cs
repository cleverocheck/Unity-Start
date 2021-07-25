using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour {
    public void restart_game() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void open_social() {
        Application.OpenURL("https://www.instagram.com/");
    }
}