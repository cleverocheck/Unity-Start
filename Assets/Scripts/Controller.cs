using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {
    private class CubePosition {
        public int x, y, z;
        private Vector3 vector;
        public Vector3 Vector {
            get {
                return vector;
            }
            set {
                vector = new Vector3(Convert.ToInt32(value.x), Convert.ToInt32(value.y), Convert.ToInt32(value.z));
                x = Convert.ToInt32(vector.x);
                y = Convert.ToInt32(vector.y);
                z = Convert.ToInt32(vector.z);
            }
        }
        public CubePosition(int default_x, int default_y, int default_z) {
            x = default_x;
            y = default_y;
            z = default_z;
            Vector = new Vector3(default_x, default_y, default_z);
        }
    }

    private class MaxPosition {
        private int max_x, max_y, max_z;
        public int MaxX {
            get {
                return max_x;
            }
            set {
                if (Math.Abs(value) > max_x) max_x = value;
            }
        }
        public int MaxY {
            get {
                return max_y;
            }
            set {
                if (Math.Abs(value) > max_y) max_y = value;
            }
        }
        public int MaxZ {
            get {
                return max_z;
            }
            set {
                if (Math.Abs(value) > max_z) max_z = value;
            }
        }
        public MaxPosition(int default_x, int default_y, int default_z) {
            MaxX = default_x;
            MaxY = default_y;
            MaxZ = default_z;
        }
        internal void Deconstruct(out int max_x, out int max_y, out int max_z) {
            max_x = this.MaxX;
            max_y = this.MaxY;
            max_z = this.MaxZ;
        }
    }

    public float change_speed = 0.5f;
    public float camera_speed = 2;
    public GameObject placeholder;
    public GameObject cube_create, cubes;
    public GameObject[] start_page;
    public bool game_over = false;
    private bool game_start = false;
    private CubePosition current_cube = new CubePosition(0, 1, 0);
    private Coroutine placeholder_tick;
    private List<Vector3> cubes_positions = new List<Vector3> {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, -1),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, -1)
    };
    float camera_default_y;
    float camera_move_y;
    private void Start() {
        placeholder_tick = StartCoroutine(show_cube_placeholder());
        camera_default_y = Camera.main.transform.localPosition.y;
        camera_move_y = camera_default_y + current_cube.y - 1;
    }
    private void Update() {
        if (placeholder != null) {
            if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && (!EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject == null)) {
#if !UNITY_EDITOR
                 if(Input.GetTouch(0).phase != TouchPhase.Began) return;
#endif
                if (!game_start) {
                    game_start = true;
                    foreach (GameObject UI in start_page) UI.SetActive(false);
                }
                GameObject new_cube = Instantiate(cube_create, placeholder.transform.position, Quaternion.identity) as GameObject;
                new_cube.transform.SetParent(cubes.transform);
                current_cube.Vector = placeholder.transform.position;
                cubes_positions.Add(current_cube.Vector);
                spawn_placeholder();
                camera_tick();
            }
            if (!game_over && cubes.GetComponent<Rigidbody>().velocity.magnitude > 0.1f) {
                Destroy(placeholder);
                game_over = true;
                StopCoroutine(placeholder_tick);
            }
        }
        Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.localPosition, new Vector3(Camera.main.transform.localPosition.x, camera_move_y, Camera.main.transform.localPosition.z), camera_speed * Time.deltaTime);
    }
    IEnumerator show_cube_placeholder() {
        while (true) {
            spawn_placeholder();
            yield return new WaitForSeconds(change_speed);
        }
    }
    private void spawn_placeholder() {
        List<Vector3> positions = new List<Vector3>();
        if (is_position_empty(new Vector3(current_cube.x + 1, current_cube.y, current_cube.z))) positions.Add(new Vector3(current_cube.x + 1, current_cube.y, current_cube.z));
        if (is_position_empty(new Vector3(current_cube.x - 1, current_cube.y, current_cube.z))) positions.Add(new Vector3(current_cube.x - 1, current_cube.y, current_cube.z));
        if (is_position_empty(new Vector3(current_cube.x, current_cube.y + 1, current_cube.z))) positions.Add(new Vector3(current_cube.x, current_cube.y + 1, current_cube.z));
        if (is_position_empty(new Vector3(current_cube.x, current_cube.y - 1, current_cube.z))) positions.Add(new Vector3(current_cube.x, current_cube.y - 1, current_cube.z));
        if (is_position_empty(new Vector3(current_cube.x, current_cube.y, current_cube.z + 1))) positions.Add(new Vector3(current_cube.x, current_cube.y, current_cube.z + 1));
        if (is_position_empty(new Vector3(current_cube.x, current_cube.y, current_cube.z - 1))) positions.Add(new Vector3(current_cube.x, current_cube.y, current_cube.z - 1));

        int current_position = UnityEngine.Random.Range(0, positions.Count);
        if (placeholder.transform.position != positions[current_position]) placeholder.transform.position = positions[current_position];
        else spawn_placeholder();
    }
    private bool is_position_empty(Vector3 targetPos) {
        if (targetPos.y <= 0) return false;
        foreach (Vector3 pos in cubes_positions) {
            if (pos.x == targetPos.x && pos.y == targetPos.y && pos.z == targetPos.z) return false;
        }
        return true;
    }
    private void camera_tick() {
        (int x, int y, int z) = new MaxPosition(0, 0, 0);
        foreach (Vector3 cube_position in cubes_positions) {
            x = Convert.ToInt32(cube_position.x);
            y = Convert.ToInt32(cube_position.y);
            z = Convert.ToInt32(cube_position.z);
        }
        camera_move_y = camera_default_y + current_cube.y - 1;
    }
}