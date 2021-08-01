using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour {
    [SerializeField] private AudioClip button_audio;
    public GameObject[] cubes;
    public int[] cubes_price;
    private static GlobalController instance = null;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }
    public void restart_game(Vector3 sound_position) {
        if (PlayerPrefs.GetInt("sound") != 0) AudioSource.PlayClipAtPoint(button_audio, sound_position, 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}