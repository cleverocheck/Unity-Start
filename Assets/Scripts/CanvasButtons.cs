using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CanvasButtons : MonoBehaviour {
    [SerializeField] private Sprite sound_on, sound_off;
    private GameObject global_controller;
    private void Start() {
        global_controller = GameObject.Find("GlobalController");
        if (!PlayerPrefs.HasKey("sound")) PlayerPrefs.SetInt("sound", 1);
        if (PlayerPrefs.GetInt("sound") == 0 && gameObject.name == "Sound") {
            GetComponent<Image>().sprite = sound_off;
            global_controller.GetComponent<AudioSource>().Pause();
            global_controller.GetComponent<Animator>().SetBool("Off", false);
            global_controller.GetComponent<Animator>().SetBool("On", false);
        }
    }
    private void Update() {
        if (gameObject.name == "Sound") {
            if (PlayerPrefs.GetInt("sound") == 0) GetComponent<Image>().sprite = sound_off;
            else GetComponent<Image>().sprite = sound_on;
        }
    }
    public void restart_game() {
        global_controller.GetComponent<GlobalController>().restart_game(transform.position);
    }
    public void open_social() {
        if (PlayerPrefs.GetInt("sound") != 0) GetComponent<AudioSource>().Play();
        Application.OpenURL("https://www.instagram.com/");
    }
    public void open_store() {
        if (PlayerPrefs.GetInt("sound") != 0) GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Store");
    }
    public void close_store() {
        if (PlayerPrefs.GetInt("sound") != 0) GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Main");
    }
    public void change_sound() {
        if (PlayerPrefs.GetInt("sound") == 0) {
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetInt("sound", 1);
            global_controller.GetComponent<AudioSource>().UnPause();
            global_controller.GetComponent<Animator>().SetBool("Off", false);
            global_controller.GetComponent<Animator>().SetBool("On", true);
        } else {
            PlayerPrefs.SetInt("sound", 0);
            global_controller.GetComponent<Animator>().SetBool("Off", true);
            global_controller.GetComponent<Animator>().SetBool("On", false);
        }
    }
}