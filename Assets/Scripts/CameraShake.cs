using UnityEngine;

public class CameraShake : MonoBehaviour {
    private Vector3 base_position;
    public float shake_duraction = 1, factor = 1.2f;
    private void Start() {
        base_position = GetComponent<Transform>().localPosition;
    }
    private void Update() {
        if (shake_duraction > 0) {
            GetComponent<Transform>().localPosition = base_position + Random.insideUnitSphere * shake_duraction;
            shake_duraction -= Time.deltaTime * factor;
        } else {
            Destroy(GetComponent<CameraShake>());
        }
    }
}