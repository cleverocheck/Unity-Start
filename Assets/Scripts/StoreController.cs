using UnityEngine;
using System;

public class StoreController : MonoBehaviour {
    [SerializeField] private GameObject cubes_container, canvas, cube_text;
    [SerializeField] private Material disabled_material;
    private GameObject[] cubes;
    private int[] cubes_price;
    private readonly float cube_spacing_hor = 1.85f, cube_spacing_vert = 2.5f, text_spacing_hor = 260, text_spacing_vert = 330;
    private void Start() {
        cubes = GameObject.Find("GlobalController").GetComponent<GlobalController>().cubes;
        cubes_price = GameObject.Find("GlobalController").GetComponent<GlobalController>().cubes_price;
        for (int i = 0; i < cubes.Length; i++) {
            GameObject new_cube = Instantiate(cubes[i], cubes_container.transform.position, Quaternion.Euler(-20, 45, -20));
            if (PlayerPrefs.GetInt("record") < cubes_price[i]) new_cube.GetComponent<MeshRenderer>().material = disabled_material;
            new_cube.transform.SetParent(cubes_container.transform);
            GameObject new_text = Instantiate(cube_text, canvas.transform.position, Quaternion.identity);
            if (cubes_price[i] == 0) new_text.GetComponent<TMPro.TextMeshProUGUI>().text = "<color=#e06055>FREE</color>";
            else new_text.GetComponent<TMPro.TextMeshProUGUI>().text = "<color=#e06055>BEST: </color><u color=#e06055>" + cubes_price[i];
            new_text.transform.SetParent(canvas.transform);
            new_text.transform.localScale = new Vector3(1, 1, 1);
            Vector3 cube_position = new_cube.transform.position, text_position = new_text.transform.position;
            if (i != 9 && i % 3 == 0) {
                cube_position.x = -cube_spacing_hor;
                text_position.x = -text_spacing_hor;
            } else if (i == 2 || i == 5 || i == 8) {
                cube_position.x = cube_spacing_hor;
                text_position.x = text_spacing_hor;
            }
            cube_position.y = (float)Math.Floor(i / 3f) * -cube_spacing_vert;
            text_position.y = ((float)Math.Floor(i / 3f) - 1) * -text_spacing_vert;
            new_cube.transform.localPosition = cube_position;
            new_text.transform.localPosition = text_position;
        }
    }
}