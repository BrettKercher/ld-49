using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class vroom_vroom : MonoBehaviour {

    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _friction = 0.8f;

    private Rigidbody2D _rigidBody;
    
    // Start is called before the first frame update
    private void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update() {
        var forward = Input.GetAxis("Vertical") * _speed * Time.deltaTime;
        _rigidBody.AddForce(new Vector2(0, forward));
    }

    private void LateUpdate() {
        _rigidBody.velocity *= _friction;
    }
}