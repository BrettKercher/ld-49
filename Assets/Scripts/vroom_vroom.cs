using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class vroom_vroom : MonoBehaviour {

    [SerializeField] private float _velocity = 0f;
    [SerializeField] private float _acceleration = 500f;
    [SerializeField] private float _eqAcceleration = 200f;
    [SerializeField] private float _angularAcceleration;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _backwardsAcceleration = 250f;

    [SerializeField] private Animator _animator;

    private Rigidbody2D _rigidBody;
    private ParticleSystem _particleSystem;
    
    public bool Pause { get; set; }

    // Start is called before the first frame update
    private void Awake() {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _particleSystem = GetComponent<ParticleSystem>();
        _rigidBody.freezeRotation = true;
        _rigidBody.drag = 8f;
        _rigidBody.mass = 0.5f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Pause) {
            return;
        }

        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");

        if (Input.GetButton("Fire3"))
        {
            //equestrian mode activated
            //move slowly indirection pushed
            var forwardVector = v * _eqAcceleration * Time.deltaTime * transform.right;
            var sideVector = -h * _eqAcceleration * Time.deltaTime * transform.up;
            _rigidBody.AddForce(forwardVector + sideVector);
        }
        else
        {
            //constant reverse acceleration
            if (v > 0)
            {
                _velocity = v * _acceleration * Time.deltaTime;
            }
            else
            {
                _velocity = v * _backwardsAcceleration * Time.deltaTime;
            }


            _rigidBody.AddForce(transform.right * _velocity);

            //tie the turning rate to the forward velocity 
            var speed = Vector2.Dot(_rigidBody.velocity, transform.right);
            _rigidBody.rotation -= h * speed * _angularAcceleration * Time.deltaTime;
            if (v > 0.5)
            {
                
                _animator.SetTrigger("isMoving");
            }
            else{_animator.ResetTrigger("isMoving");}

            //acceleration scaling to max speed but min acceleration
            _acceleration = 900 * (speed / _maxSpeed) + 200;
            if (speed > 4)
            {
                _particleSystem.enableEmission = true;
            }
            else
            {
                _particleSystem.enableEmission = false;
            }
        }
    }

    private void LateUpdate() {

    }
}