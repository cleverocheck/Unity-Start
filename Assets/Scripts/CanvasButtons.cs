using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour {
    public Sprite music_on, music_off;
    private void Start() {
        if (PlayerPrefs.GetInt("music") == 0 && gameObject.name == "Music") GetComponent<Image>().sprite = music_off;
    }
    public void restart_game() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void open_social() {
        if (PlayerPrefs.GetInt("music") != 0) GetComponent<AudioSource>().Play();
        Application.OpenURL("https://www.instagram.com/");
    }

    public void handle_music() {
        if (PlayerPrefs.GetInt("music") == 0) {
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetInt("music", 1);
            GetComponent<Image>().sprite = music_on;
        } else {
            PlayerPrefs.SetInt("music", 0);
            GetComponent<Image>().sprite = music_off;
        }
    }
}