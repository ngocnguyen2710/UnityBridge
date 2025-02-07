using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private BrickColor brickColor;
    public ParticleSystem collisionParticleSystem;
    public BrickColor GetBrickColor() {
        return brickColor;
    }

    private void OnTriggerEnter(Collider collider) {
        Brick brick = transform.gameObject.GetComponent<Brick>();
        if (collider.CompareTag("Player") && brick.GetBrickColor() == BrickColor.Blue) {
            var em = collisionParticleSystem.emission;

            em.enabled = true;
            collisionParticleSystem.Play();

            Deactive();
            Invoke(nameof(Active), 2.0f);
            
        }
    }

    private void Active() {
        // transform.gameObject.SetActive(true);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    private void Deactive() {
        // transform.gameObject.SetActive(false);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }
}

public enum BrickColor {
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3
}