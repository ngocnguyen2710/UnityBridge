using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MapManager : MonoBehaviour
{
    [SerializeField] private int horizontalBricksNumber;
    [SerializeField] private int verticalBricksNumber;
    [SerializeField] private int colorTypeNumber;
    [SerializeField] private int brickOffset;
    [SerializeField] private GameObject blueBrickPrefab;
    [SerializeField] private GameObject redBrickPrefab;
    [SerializeField] private GameObject greenBrickPrefab;
    [SerializeField] private GameObject yellowBrickPrefab;
    [SerializeField] private Button replayButton;
    [SerializeField] private GameObject winScreen;
    public static MapManager instance;
    private bool hasWon = false;
    public List<Transform> waypoints = new List<Transform>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if(instance == null) {
            instance = this;
        }
        replayButton.onClick.AddListener(Replay);
        OnInit();
        // DrawCircle();
    }

    private void OnInit() {
        winScreen.SetActive(false);
        hasWon = false;
        MapGenerate(horizontalBricksNumber, verticalBricksNumber, colorTypeNumber);
    }

    public GameObject GetBrickPrefab(Color color) {
        if (color == Color.red) {
            return redBrickPrefab;
        } else if (color == Color.green) {
            return greenBrickPrefab;
        } else if (color == Color.yellow) {
            return yellowBrickPrefab;
        } else {
            return blueBrickPrefab;
        }
    }

    private void MapGenerate(int horizontalBricksNumber, int verticalBricksNumber, int colorTypeNumber) {
        int sumBricks = horizontalBricksNumber * verticalBricksNumber;
        List<int> colorList = new List<int>();
        // 1 -> Blue
        // 2 -> Red
        // 3 -> Green
        // 4 -> Yellow
        for (int i = 0; i < sumBricks/colorTypeNumber; i++) {
            for (int j = 1; j <= colorTypeNumber; j++) {
                colorList.Add(j);
            }
        }

        //shuffle
        colorList = colorList.OrderBy(i => Guid.NewGuid()).ToList();

        int index = 0;
        for (int i = 0; i < verticalBricksNumber; i++) {
            for (int j= 0; j < horizontalBricksNumber; j++) {
                // Offset + n with cube size n*n*n, in this case: 1
                if (colorList[index] == 2) {
                    // CreateBrick(redBrickPrefab, new Vector3(i * (brickOffset + 1) + brickOffset/2, 1, j * (brickOffset + 1) + brickOffset/2));
                    GameObject cube = Instantiate(redBrickPrefab, new Vector3(i * (brickOffset + 1) + brickOffset/2, 1, j * (brickOffset + 1) + brickOffset/2), Quaternion.identity);
                    cube.transform.SetParent(transform);
                    waypoints.Add(cube.transform);
                } else if (colorList[index] == 3) {
                    CreateBrick(greenBrickPrefab, new Vector3(i * (brickOffset + 1) + brickOffset/2, 1, j * (brickOffset + 1) + brickOffset/2));
                } else if (colorList[index] == 4) {
                    CreateBrick(yellowBrickPrefab, new Vector3(i * (brickOffset + 1) + brickOffset/2, 1, j * (brickOffset + 1) + brickOffset/2));
                } else {
                    CreateBrick(blueBrickPrefab, new Vector3(i * (brickOffset + 1) + brickOffset/2, 1, j * (brickOffset + 1) + brickOffset/2));
                }
                if (index < colorList.Count() - 1) {
                    index++;
                }
            }

        }
        // move map manager to center of the map 
        // ask mentor
        float width = verticalBricksNumber + 1 + brickOffset * (verticalBricksNumber - 1);
        float height = horizontalBricksNumber + 1 + brickOffset * (horizontalBricksNumber - 1);
        transform.position = new Vector3(-height/2f, 0, -width/2f);
    }

    private void CreateBrick (GameObject prefab, Vector3 position) {
        GameObject cube = Instantiate(prefab, position, Quaternion.identity);
        cube.transform.SetParent(transform);
    }

    public void WinGame() {
        hasWon = true;
        winScreen.SetActive(true);
        Debug.Log("You win!");
    }

    public void Replay() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void DrawCircle() {
        float radius = 0.1f;
        for (float radians = Mathf.PI/4f; radians <= Mathf.PI*2 ; radians += Mathf.PI/2f) {
            float x = radius * Mathf.Cos(radians);
            float z = radius * Mathf.Sin(radians);
            float y = 1;
            GameObject cube = Instantiate(blueBrickPrefab, new Vector3(x,y,z), Quaternion.identity);
            cube.transform.SetParent(transform);
        }
    }
}
