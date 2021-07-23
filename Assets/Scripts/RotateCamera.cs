using UnityEngine;

public class RotateCamera : MonoBehaviour {
    public float speed = 5f;
    public GameObject controller;
    private void Update() {
        if (!controller.GetComponent<Controller>().game_over) {
            GetComponent<Transform>().Rotate(0, speed * Time.deltaTime, 0);
        }
    }
}