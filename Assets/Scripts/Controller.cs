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
        public int max_hor;
        public int MaxX {
            get {
                return max_x;
            }
            set {
                if (Math.Abs(value) > max_x) max_x = value;
                if (Math.Abs(value) > max_hor) max_hor = value;
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
                if (Math.Abs(value) > max_z) max_x = value;
                if (Math.Abs(value) > Math.Abs(MaxX) && Math.Abs(value) > max_hor) max_hor = value;
            }
        }
        public MaxPosition(int default_x, int default_y, int default_z) {
            MaxX = default_x;
            MaxY = default_y;
            MaxZ = default_z;
        }
    }

    public GameObject record_ui, score_ui;
    public float change_speed = 0.5f, difficulty_factor = 0.001f, camera_speed = 2, camera_step = 2.5f, camera_fate_speed = 1.5f;
    public Color[] colors;
    public GameObject placeholder, cubes, vfx_cube;
    public GameObject[] cubes_create;
    // TODO: вместо такого подхода как в гайде лучше сделать централизованную системы цветов кубов и BEST достижения
    public int[] cubes_price;
    public GameObject[] start_page;
    public bool game_over = false;
    private bool game_start = false;
    private Color current_color;
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
    private float camera_default_y;
    private float camera_move_y;
    private float camera_move_z;
    private int old_max_hor;
    private void Start() {
        placeholder_tick = StartCoroutine(show_cube_placeholder());
        camera_default_y = Camera.main.transform.localPosition.y;
        camera_move_y = camera_default_y + current_cube.y - 1;
        camera_move_z = Camera.main.transform.localPosition.z;
        current_color = Camera.main.backgroundColor;
        record_ui.GetComponent<TMPro.TextMeshProUGUI>().text = "<color=#e06055><size=35>BEST: </size></color><u color=#e06055>" + PlayerPrefs.GetInt("record");
    }
    private bool IsPointerOverUIObject() {
        PointerEventData current_position = new PointerEventData(EventSystem.current);
        current_position.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(current_position, results);
        return results.Count > 0;
    }
    private void Update() {
        if (placeholder != null) {
            if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && (!IsPointerOverUIObject() || EventSystem.current.currentSelectedGameObject == null)) {
#if !UNITY_EDITOR
                 if(Input.GetTouch(0).phase != TouchPhase.Began) return;
#endif
                if (!game_start) {
                    game_start = true;
                    foreach (GameObject UI in start_page) UI.SetActive(false);
                }
                int max_cube = cubes_create.Length;
                for (int i = 0; i < cubes_price.Length; i++) {
                    if (PlayerPrefs.GetInt("record") + 1 < cubes_price[i]) {
                        if (i == 0) max_cube = i + 1;
                        else max_cube = i;
                        break;
                    }
                }
                GameObject new_cube = Instantiate(cubes_create[UnityEngine.Random.Range(0, max_cube)], placeholder.transform.position, Quaternion.identity) as GameObject;
                new_cube.transform.SetParent(cubes.transform);
                current_cube.Vector = placeholder.transform.position;
                cubes_positions.Add(current_cube.Vector);
                GameObject vfx = Instantiate(vfx_cube, current_cube.Vector, Quaternion.identity) as GameObject;
                Destroy(vfx, vfx.gameObject.GetComponent<ParticleSystem>().main.startLifetime.constant);
                change_speed -= change_speed * difficulty_factor;
                if (PlayerPrefs.GetInt("sound") != 0) GetComponent<AudioSource>().Play();
                spawn_placeholder();
                camera_tick();
            }
            if (!game_over && cubes.GetComponent<Rigidbody>().velocity.magnitude > 0.2f) {
                Destroy(placeholder);
                game_over = true;
                StopCoroutine(placeholder_tick);
            }
        }
        if (!game_over) {
            Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.localPosition, new Vector3(Camera.main.transform.localPosition.x, camera_move_y, Camera.main.transform.localPosition.z), camera_speed * Time.deltaTime);
            Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.localPosition, new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, camera_move_z), camera_speed * Time.deltaTime);
            if (Camera.main.backgroundColor != current_color) Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, current_color, Time.deltaTime / camera_fate_speed);
        }
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
        if (positions.Count > 1) {
            if (placeholder.transform.position != positions[current_position]) placeholder.transform.position = positions[current_position];
            else spawn_placeholder();
        } else if (positions.Count == 0) game_over = true;
        else placeholder.transform.position = positions[0];
    }
    private bool is_position_empty(Vector3 targetPos) {
        if (targetPos.y <= 0) return false;
        foreach (Vector3 pos in cubes_positions) {
            if (pos.x == targetPos.x && pos.y == targetPos.y && pos.z == targetPos.z) return false;
        }
        return true;
    }
    private void camera_tick() {
        MaxPosition max = new MaxPosition(0, 0, 0);
        foreach (Vector3 cube_position in cubes_positions) {
            max.MaxX = Convert.ToInt32(cube_position.x);
            max.MaxY = Convert.ToInt32(cube_position.y);
            max.MaxZ = Convert.ToInt32(cube_position.z);
        }
        camera_move_y = camera_default_y + current_cube.y - 1;
        if (max.max_hor % 3 == 0 && old_max_hor != max.max_hor) {
            camera_move_z = (Camera.main.transform.localPosition - new Vector3(0, 0, camera_step)).z;
            old_max_hor = max.max_hor;
        }

        if (PlayerPrefs.GetInt("record") < max.MaxY - 1) PlayerPrefs.SetInt("record", max.MaxY - 1);
        record_ui.GetComponent<TMPro.TextMeshProUGUI>().text = "<color=#e06055><size=35>BEST: </size></color><u color=#e06055>" + PlayerPrefs.GetInt("record");
        score_ui.GetComponent<TMPro.TextMeshProUGUI>().text = "<color=#e06055><size=35>NOW: </size></color><u color=#e06055>" + (max.MaxY - 1);
        if (max.MaxY % 4 == 0) current_color = colors[2];
        else if (max.MaxY % 3 == 0) current_color = colors[1];
        else current_color = colors[0];
    }
}