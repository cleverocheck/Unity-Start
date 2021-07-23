using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {
    private CubePosition current_cube = new CubePosition(0, 1, 0);
    public float change_speed = 0.5f;
    public Transform placeholder;

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
        StartCoroutine(show_cube_placeholder());
    }
    IEnumerator show_cube_placeholder() {
        while (true) {
            SpawnPositions();
            yield return new WaitForSeconds(change_speed);
        }
    }
    private void SpawnPositions() {
        List<Vector3> positions = new List<Vector3>();
        if (is_position_empty(new Vector3(current_cube.x + 1, current_cube.y, current_cube.z))) positions.Add(new Vector3(current_cube.x + 1, current_cube.y, current_cube.z));
        if (is_position_empty(new Vector3(current_cube.x - 1, current_cube.y, current_cube.z))) positions.Add(new Vector3(current_cube.x - 1, current_cube.y, current_cube.z));
        if (is_position_empty(new Vector3(current_cube.x, current_cube.y + 1, current_cube.z))) positions.Add(new Vector3(current_cube.x, current_cube.y + 1, current_cube.z));
        if (is_position_empty(new Vector3(current_cube.x, current_cube.y - 1, current_cube.z))) positions.Add(new Vector3(current_cube.x, current_cube.y - 1, current_cube.z));
        if (is_position_empty(new Vector3(current_cube.x, current_cube.y, current_cube.z + 1))) positions.Add(new Vector3(current_cube.x, current_cube.y, current_cube.z + 1));
        if (is_position_empty(new Vector3(current_cube.x, current_cube.y, current_cube.z - 1))) positions.Add(new Vector3(current_cube.x, current_cube.y, current_cube.z - 1));

        int current_position = UnityEngine.Random.Range(0, positions.Count);
        if (placeholder.position != positions[current_position]) placeholder.position = positions[current_position];
        else SpawnPositions();
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