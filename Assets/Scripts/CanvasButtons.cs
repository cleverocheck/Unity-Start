using UnityEngine;
using UnityEngine.UI;
public class CanvasButtons : MonoBehaviour {
    public Sprite sound_on, sound_off;
    private void Start() {
        if (!PlayerPrefs.HasKey("sound")) PlayerPrefs.SetInt("sound", 1);
        if (PlayerPrefs.GetInt("sound") == 0 && gameObject.name == "sound") {
            GetComponent<Image>().sprite = sound_off;
            GameObject.Find("GlobalController").GetComponent<AudioSource>().Pause();
        }
    }
    public void restart_game() {
        GameObject.Find("GlobalController").GetComponent<GlobalController>().restart_game(transform.position);
    }
    public void open_social() {
        if (PlayerPrefs.GetInt("sound") != 0) GetComponent<AudioSource>().Play();
        Application.OpenURL("https://www.instagram.com/");
    }
    public void open_store() {
        if (PlayerPrefs.GetInt("sound") != 0) GetComponent<AudioSource>().Play();
    }
    public void handle_music() {
        if (PlayerPrefs.GetInt("sound") == 0) {
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetInt("sound", 1);
            GetComponent<Image>().sprite = sound_on;
            GameObject.Find("GlobalController").GetComponent<AudioSource>().Play();
        } else {
            PlayerPrefs.SetInt("sound", 0);
            GetComponent<Image>().sprite = sound_off;
            GameObject.Find("GlobalController").GetComponent<AudioSource>().Pause();
        }
    }
}