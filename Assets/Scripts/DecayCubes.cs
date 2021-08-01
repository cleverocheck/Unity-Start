using UnityEngine;
using System.Collections;

public class DecayCubes : MonoBehaviour {
    [SerializeField] private float camera_speed = 7;
    [SerializeField] private GameObject restart_button, explosion, controller;
    private bool collisition_active;
    private void Update() {
        if (controller.GetComponent<Controller>().game_over && !collisition_active) {
            Camera.main.transform.Translate(Vector3.forward * -camera_speed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Cube" && !collisition_active) {
            for (int i = collision.transform.childCount - 1; i >= 0; i--) {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100f, Vector3.up, 5f);
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100f, Vector3.down, 5f);
                child.SetParent(null);
            }
            Destroy(collision.gameObject);
            collisition_active = true;
            Camera.main.gameObject.AddComponent<CameraShake>();
            GameObject vfx = Instantiate(explosion, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z), Quaternion.identity) as GameObject;
            Destroy(vfx, vfx.gameObject.GetComponent<ParticleSystem>().main.startLifetime.constant);
            if (PlayerPrefs.GetInt("sound") != 0) GetComponent<AudioSource>().Play();
            StartCoroutine(show_restart_button());
        }
    }
    IEnumerator show_restart_button() {
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        GameObject.Find("GlobalController").GetComponent<AudioSource>().Pause();
        GameObject.Find("GlobalController").GetComponent<GlobalController>().show_game_over_ad(() => {
            restart_button.SetActive(true);
            GameObject.Find("GlobalController").GetComponent<AudioSource>().UnPause();
        });
    }
}