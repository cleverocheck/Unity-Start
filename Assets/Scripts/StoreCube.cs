using UnityEngine;

public class StoreCube : MonoBehaviour {
    public int need_to_unlock;
    public Material disabled_material;
    private void Start() {
        if (PlayerPrefs.GetInt("record") < need_to_unlock) GetComponent<MeshRenderer>().material = disabled_material;
    }
}