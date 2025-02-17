using System.IO.Compression;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private FloatingJoystick floatingJoystick;
    // int eaten;
    private void Start() {
        OnInit();
    }

    void OnInit() {
        eaten = 0;
        isWinning = false;
    }

    void Update() {
        _rb.linearVelocity = new Vector3(floatingJoystick.Horizontal * _speed, _rb.linearVelocity.y, floatingJoystick.Vertical * _speed);

        Vector3 beginCast = transform.forward + transform.position;
        Ray ray = new Ray(beginCast, Vector3.down);
        Debug.DrawRay(beginCast, Vector3.down, Color.red, 1f);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            if (hit.transform.CompareTag("Bridge")) {
                Bridge bridge = hit.transform.gameObject.GetComponent<Bridge>();
                GameObject bridgeObject = hit.transform.gameObject;
                if (eaten > 0 && (int)bridge.GetBridgeColor() != (int)GetCharacterType()) {
                    //set the bridge color to player color
                    bridge.SetBridgeColor((int)GetCharacterType());
                    bridgeObject.GetComponent<MeshRenderer>().material.color = GetCharacterTypeColor();
                    //destroy the brick on top of the player
                    Destroy(transform.GetChild(transform.childCount - 1).gameObject);
                    eaten--;
                } 
                
                if ((int)bridge.GetBridgeColor() == (int)GetCharacterType()) {
                    _rb.linearVelocity = new Vector3(floatingJoystick.Horizontal * _speed, _rb.linearVelocity.y, floatingJoystick.Vertical * _speed);
                } else {
                    if (floatingJoystick.Vertical > 0) {
                        _rb.linearVelocity = new Vector3(floatingJoystick.Horizontal * _speed, _rb.linearVelocity.y, 0 * _speed);
                    } else {
                        _rb.linearVelocity = new Vector3(floatingJoystick.Horizontal * _speed, _rb.linearVelocity.y, floatingJoystick.Vertical * _speed);
                    }
                }
            }
        }
        
        if (floatingJoystick.Horizontal != 0 || floatingJoystick.Vertical != 0) {
            if (_rb.linearVelocity != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(_rb.linearVelocity);
            }
            ChangeAnim("walk");
        } else {
            ChangeAnim("idle");
        }
    }

    protected void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Finish") && isWinning == false) {
            ChangeAnim("celebrate");
            isWinning = true;
            if (floatingJoystick != null) {
                floatingJoystick.gameObject.SetActive(false); 
            }            
            _speed = 0;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            WinGame();
        }
        Character otherCharacter = collision.gameObject.GetComponent<Character>();
        if (otherCharacter is Bot) {
            if (eaten > otherCharacter.GetEaten()) {
                
            }
        }
    }

    private void WinGame() {
        //save score to file in StreamingAssets
        if (FileManager.instance != null) {
            FileManager.instance.SaveScore(eaten);
        }   
        MapManager.instance.WinGame();
    }
}
