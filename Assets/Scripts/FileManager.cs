using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour
{
    private string filePath; // Đường dẫn lưu file
    public static FileManager instance;

    private void Awake()
    {
        if(instance == null) {
            instance = this;
        }
        // Xác định đường dẫn file lưu điểm trong StreamingAssets
        filePath = Path.Combine(Application.streamingAssetsPath, "score.txt");

        // Kiểm tra nếu thư mục chưa tồn tại thì tạo mới (Windows/Mac)
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
    }

    public void SaveScore(int score)
    {
        string scoreData = "Score: " + score + " - Time: " + System.DateTime.Now + "\n";

        try
        {
            File.AppendAllText(filePath, scoreData); // Ghi điểm vào file
            Debug.Log("Score saved to: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving score: " + e.Message);
        }
    }
}
