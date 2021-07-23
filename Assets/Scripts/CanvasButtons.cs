using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour {
    public void restart_game() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}