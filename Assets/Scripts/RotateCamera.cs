using UnityEngine;

public class RotateCamera : MonoBehaviour {
    [SerializeField] private float speed = 5;
    //[SerializeField] private GameObject controller; — не обезатьльно брать GameObject Unity пожет примимать в инсепторе любой компонент.
    [SerializeField] private Controller controller; 
    
    private void Start(){
        controller = controller.GetComponent<Controller>();
    }
    private void Update() {
        // GetComponent в update это вообще прохо. за такое по рукам бют 
        // if (!controller.GetComponent<Controller>().game_over) {
        if (!controller.game_over) {
            GetComponent<Transform>().Rotate(0, speed * Time.deltaTime, 0);
        }
    }
}
