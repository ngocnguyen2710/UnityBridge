using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _jumpForce = 200;
    [SerializeField] private Rigidbody _rb;

    void Update() {
        var vel = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * _speed;
        vel.y = _rb.linearVelocity.y;
        _rb.linearVelocity = vel;
        if (vel != Vector3.zero) {
            _rb.rotation = Quaternion.LookRotation(vel);
        }        
        if (Input.GetKeyDown(KeyCode.Space)) {
            _rb.AddForce(Vector3.up * _jumpForce);
        }
    }
}
