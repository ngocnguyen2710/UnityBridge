using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player"); // The player
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 10, -10);
        transform.eulerAngles = new Vector3(40, 0, 0);
    }
}
