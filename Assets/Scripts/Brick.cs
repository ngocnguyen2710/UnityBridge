using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private BrickColor brickColor;

    public BrickColor GetBrickColor() {
        return brickColor;
    }

    private void OnTriggerEnter(Collider collider) {
        Brick brick = transform.gameObject.GetComponent<Brick>();
        if (collider.CompareTag("Player") && brick.GetBrickColor() == BrickColor.Blue) {
            Deactive();
            Invoke(nameof(Active), 2.0f);
        }
    }

    private void Active() {
        transform.gameObject.SetActive(true);
    }

    private void Deactive() {
        transform.gameObject.SetActive(false);
    }
}

public enum BrickColor {
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3
}