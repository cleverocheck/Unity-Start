using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour {
    public AudioClip button_audio;
    private static GlobalController instance = null;
    public void restart_game(Vector3 sound_position) {
        if (PlayerPrefs.GetInt("sound") != 0) AudioSource.PlayClipAtPoint(button_audio, sound_position, 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }
}