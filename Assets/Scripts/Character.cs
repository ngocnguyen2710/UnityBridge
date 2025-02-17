using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour {
    [SerializeField] private CharacterType characterType;
    [SerializeField] private Animator anim;
    protected int eaten;
    private string currentAnimName;
    protected bool isWinning;

    public CharacterType GetCharacterType() {
        return characterType;
    }

    public Color GetCharacterTypeColor() {
        switch (characterType) {
            case CharacterType.Red:
                return Color.red;
            case CharacterType.Green:
                return Color.green;
            case CharacterType.Blue:
                return Color.blue;
            case CharacterType.Yellow:
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    public int GetEaten() {
        return eaten;
    }

    protected void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Brick")) {
            Brick brick = collider.gameObject.GetComponent<Brick>();
            if ((int)brick.GetBrickColor() == (int)GetCharacterType()) {
                Vector3 eatenBrickPosition = transform.position;
                eatenBrickPosition.y += eaten;
                GameObject brickPrefab = MapManager.instance.GetBrickPrefab(GetCharacterTypeColor());
                CreateBrickOnTop(brickPrefab, eatenBrickPosition);
                eaten++;
            }
        }
    }

    private void CreateBrickOnTop(GameObject prefab, Vector3 position) {
        GameObject cube = Instantiate(prefab, position, Quaternion.identity);
        Vector3 eulerAngles = transform.eulerAngles;
        cube.transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        cube.transform.SetParent(transform);
    }

    protected void ChangeAnim(string animName) {
        if(currentAnimName != animName) {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
}

public enum CharacterType {
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3
}