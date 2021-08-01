using UnityEngine;
public class CameraConstantWidth : MonoBehaviour {
    [SerializeField] private Vector2 resolution = new Vector2(720, 1280);
    [Range(0, 1)] public float WidthOrHeight = 0;
    private new Camera camera;
    private float initial_size, field_of_view, horizontal_field_of_view = 120f;
    private void Start() {
        camera = GetComponent<Camera>();
        initial_size = camera.orthographicSize;
        field_of_view = camera.fieldOfView;
        horizontal_field_of_view = get_vertical_field_of_view(field_of_view, 1 / (resolution.x / resolution.y));
    }
    private void Update() {
        if (camera.orthographic) {
            float constantWidthSize = initial_size * (resolution.x / resolution.y / camera.aspect);
            camera.orthographicSize = Mathf.Lerp(constantWidthSize, initial_size, WidthOrHeight);
        } else {
            float constantWidthFov = get_vertical_field_of_view(horizontal_field_of_view, camera.aspect);
            camera.fieldOfView = Mathf.Lerp(constantWidthFov, field_of_view, WidthOrHeight);
        }
    }
    private float get_vertical_field_of_view(float horizontal_field_of_view, float aspect) {
        return (2 * Mathf.Atan(Mathf.Tan(horizontal_field_of_view * Mathf.Deg2Rad / 2) / aspect)) * Mathf.Rad2Deg;
    }
}