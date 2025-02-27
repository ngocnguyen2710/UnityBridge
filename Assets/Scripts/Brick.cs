using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private BrickColor brickColor;
    public ParticleSystem collisionParticleSystem;

    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public BrickColor GetBrickColor() {
        return brickColor;
    }

    public Color GetBrickColorType() {
        switch (brickColor) {
            case BrickColor.Red:
                return Color.red;
            case BrickColor.Green:
                return Color.green;
            case BrickColor.Blue:
                return Color.blue;
            case BrickColor.Yellow:
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player") || collider.CompareTag("Bot")) {
            Character character = collider.gameObject.GetComponent<Character>();
            if ((int)GetBrickColor() == (int)character.GetCharacterType()) {
                var em = collisionParticleSystem.emission;
                em.enabled = true;
                collisionParticleSystem.Play();

                Deactivate();
                Invoke(nameof(Activate), 2.0f);
            }
        }
    }

    private void Activate() {
        //transform.gameObject.SetActive(true);
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    private void Deactivate() {
        //transform.gameObject.SetActive(false);
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
    }
}

public enum BrickColor {
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3
}