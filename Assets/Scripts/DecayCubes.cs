using UnityEngine;

public class DecayCubes : MonoBehaviour {
    public float camera_speed = 7;
    public GameObject restart_button, controller, explosion;
    private bool collisition_active;
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Cube" && !collisition_active) {
            for (int i = collision.transform.childCount - 1; i >= 0; i--) {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100f, Vector3.up, 5f);
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100f, Vector3.down, 5f);
                child.SetParent(null);
            }
            restart_button.SetActive(true);
            Destroy(collision.gameObject);
            collisition_active = true;
            Camera.main.gameObject.AddComponent<CameraShake>();
            GameObject vfx = Instantiate(explosion, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z), Quaternion.identity) as GameObject;
            Destroy(vfx, 1.5f);
            if (PlayerPrefs.GetInt("music") != 0) GetComponent<AudioSource>().Play();
        }
    }
    private void Update() {
        if (controller.GetComponent<Controller>().game_over && !collisition_active) {
            Camera.main.transform.Translate(Vector3.forward * -camera_speed * Time.deltaTime);
        }
    }
}