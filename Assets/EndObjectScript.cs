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
        Debug.Log($"Final score retrieved: {finalScore}");
        
        if (scoreText != null)
        {
            scoreText.text = $"Votre score est de : {finalScore}";
            Debug.Log($"Score text updated: {scoreText.text}");
        }
        else
        {
            Debug.LogError("ScoreText reference not set in EndObjectScript! Make sure to assign it in the inspector.");
        }
    }
    
    public void RestartGame()
    {
        Debug.Log("RestartGame button clicked!");
        SceneManager.LoadScene("MainScene");
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