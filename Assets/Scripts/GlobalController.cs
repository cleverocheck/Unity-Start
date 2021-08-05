using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using System.Collections;

public class GlobalController : MonoBehaviour, IUnityAdsListener {
    [SerializeField] private AudioClip button_audio;
    [SerializeField] private string game_android_id, type_android_game_over, type_android_banner;
    // эти строки мы ставим если хотим выложить игру в google play вместо private bool test_mode = true; (при билде будет отключен тестовый режим рекламы)
    // #if UNITY_EDITOR
    // private bool test_mode = true;
    // #else
    // private bool test_mode = false;
    // #endif
    private bool test_mode = true;
    public delegate void show_game_over_ad_callback();
    private show_game_over_ad_callback ad_game_over_callback;
    public GameObject[] cubes;
    public int[] cubes_price;
    private static GlobalController instance = null;
    private void Start() {
        Advertisement.AddListener(this);
        Advertisement.Initialize(game_android_id, test_mode);
        if (PlayerPrefs.GetInt("sound") == 0) {
            GetComponent<AudioSource>().Stop();
            GetComponent<Animator>().SetBool("Off", false);
            GetComponent<Animator>().SetBool("On", false);
        }
    }
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
    public void show_game_over_ad(show_game_over_ad_callback callback) {
        if (Advertisement.IsReady(type_android_game_over)) {
            Advertisement.Show(type_android_game_over);
            ad_game_over_callback = callback;
        } else {
            Advertisement.Initialize(game_android_id, test_mode);
            callback();
        }
    }
    public void OnUnityAdsReady(string ad_id) { }
    public void OnUnityAdsDidStart(string ad_id) { }
    public void OnUnityAdsDidError(string message) {
        if (ad_game_over_callback != null) {
            ad_game_over_callback();
            ad_game_over_callback = null;
        }
    }
    public void OnUnityAdsDidFinish(string ad_id, ShowResult result) {
        if (ad_game_over_callback != null) {
            ad_game_over_callback();
            ad_game_over_callback = null;
        }
    }
}