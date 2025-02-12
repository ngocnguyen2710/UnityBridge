using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private BridgeColor bridgeColor;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public BridgeColor GetBridgeColor() {
        return bridgeColor;
    }

    public void SetBridgeColor(int color) {
        switch (color) {
            case (int)BridgeColor.Red:
                meshRenderer.material.color = Color.red;
                bridgeColor = BridgeColor.Red;
                break;
            case (int)BridgeColor.Green:
                meshRenderer.material.color = Color.green;
                bridgeColor = BridgeColor.Green;
                break;
            case (int)BridgeColor.Blue:
                meshRenderer.material.color = Color.blue;
                bridgeColor = BridgeColor.Blue;
                break;
            case (int)BridgeColor.Yellow:
                meshRenderer.material.color = Color.yellow;
                bridgeColor = BridgeColor.Yellow;
                break;
        }
    }

    private void OnTriggerEnter(Collider collider) {
        // if (collider.CompareTag("Player") && GetBridgeColor() == BridgeColor.Blue) {
        //     collider.transform.gameObject
        // }
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

public enum BridgeColor {
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3,
    None = 4
}