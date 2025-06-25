using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndObjectScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private void Start()
    {
        DisplayFinalScore();
    }
    
    private void DisplayFinalScore()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        
        if (scoreText != null)
        {
            scoreText.text = $"Score Final: {finalScore} points\n\nCliquez pour rejouer";
        }
        else
        {
            Debug.LogWarning("ScoreText reference not set in EndObjectScript");
        }
    }
    
    public void RestartGame()
    {
        ChangeScene("MainScene");
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}