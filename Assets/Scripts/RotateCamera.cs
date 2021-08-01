using UnityEngine;

public class RotateCamera : MonoBehaviour {
    [SerializeField] private float speed = 5;
    [SerializeField] private GameObject controller;
    private void Update() {
        if (!controller.GetComponent<Controller>().game_over) {
            GetComponent<Transform>().Rotate(0, speed * Time.deltaTime, 0);
        }
    }
}