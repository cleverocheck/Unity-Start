using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {
    public float change_speed = 0.5f;
    public GameObject placeholder;
    public GameObject cube_create, cubes;
    public bool game_over = false;
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

    private void Start() {
        placeholder_tick = StartCoroutine(show_cube_placeholder());
    }
    private void Update() {
        if (placeholder != null) {
            if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0)) {
#if !UNITY_EDITOR
                 if(Input.GetTouch(0).phase != TouchPhase.Began) return;
#endif
                GameObject new_cube = Instantiate(cube_create, placeholder.transform.position, Quaternion.identity) as GameObject;
                new_cube.transform.SetParent(cubes.transform);
                current_cube.set_vector(placeholder.transform.position);
                cubes_positions.Add(current_cube.get_vector());
                spawn_placeholder();
            }

            if (!game_over && cubes.GetComponent<Rigidbody>().velocity.magnitude > 0.1f) {
                Destroy(placeholder);
                game_over = true;
                StopCoroutine(placeholder_tick);
            }
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
}

struct CubePosition {
    public int x, y, z;
    public CubePosition(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vector3 get_vector() {
        return new Vector3(x, y, z);
    }
    public void set_vector(Vector3 vector) {
        x = Convert.ToInt32(vector.x);
        y = Convert.ToInt32(vector.y);
        z = Convert.ToInt32(vector.z);
    }
}