using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _jumpForce = 200;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private Animator anim;
    [SerializeField] private FloatingJoystick floatingJoystick;
    
    private string currentAnimName;
    int eaten;

    private void Start() {
        OnInit();
    }

    void OnInit() {
        eaten = 0;
    }

    void Update() {
        // var vel = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * _speed;
        // vel.y = _rb.linearVelocity.y;
        // _rb.linearVelocity = vel;
        // if (vel != Vector3.zero) {
        //     _rb.rotation = Quaternion.LookRotation(vel);
        // }        
        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     _rb.AddForce(Vector3.up * _jumpForce);
        // }
        Vector3 beginCast = transform.forward + transform.position;
        // beginCast.z+= 5;
        Ray ray = new Ray(beginCast, Vector3.down);
        Debug.DrawRay(beginCast, Vector3.down, Color.red, 1f);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            if (hit.transform.CompareTag("Bridge")) {
                if (eaten > 0) {
                    Transform brickOut = transform.GetChild(0);
                    brickOut.localScale = new Vector3(5, 0.01f, 2);
                    brickOut.SetParent(hit.transform);
                    Vector3 brickOutPosition = brickOut.position;
                    brickOutPosition.y = 0.1f;
                    brickOutPosition.z = hit.point.z;
                    brickOut.position = brickOutPosition;
                    
                    brickOut.gameObject.GetComponent<Collider>().isTrigger = false;
                    eaten--;
                }
            }
        }

        // if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f) {
        //     ChangeAnim("walk");
        // } else {
        //     ChangeAnim("idle");
        // }
        _rb.linearVelocity = new Vector3(floatingJoystick.Horizontal * _speed, _rb.linearVelocity.y, floatingJoystick.Vertical * _speed);
        if (floatingJoystick.Horizontal != 0 || floatingJoystick.Vertical != 0) {
            transform.rotation = Quaternion.LookRotation(_rb.linearVelocity);
            ChangeAnim("walk");
        } else {
            ChangeAnim("idle");
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Brick")) {
            Brick brick = collider.gameObject.GetComponent<Brick>();
            if (brick.GetBrickColor() == BrickColor.Blue) {
                Vector3 eatenBrickPosition = transform.position;
                eatenBrickPosition.y += eaten;
                GameObject cube = Instantiate(brickPrefab, eatenBrickPosition, Quaternion.identity);
                Vector3 eulerAngles = transform.eulerAngles;
		        cube.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, eulerAngles.z);
                cube.transform.SetParent(transform);
                eaten++;
                // Debug.Log(eaten);
            }
        }
    }

    protected void ChangeAnim(string animName) {
        if(currentAnimName != animName) {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
}
