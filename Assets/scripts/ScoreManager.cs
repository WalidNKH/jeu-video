using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public Text scoreText;
    public Text timerText;
    
    [SerializeField] private float gameTime = 60f; // 1 minute
    private float currentTime;
    private bool isGameActive = true;

    public int score = 0;
    void Start()
    {
        scoreText.text = score.ToString() + " points";
        currentTime = gameTime;
        UpdateTimerDisplay();
    }
    
    void Update()
    {
        if (isGameActive && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
            
            if (currentTime <= 0)
            {
                EndGame();
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void AddScore()
    {
        score += 1;
        scoreText.text = score.ToString() + " points";
    }
    
    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
    
    private void EndGame()
    {
        isGameActive = false;
        currentTime = 0;
        UpdateTimerDisplay();
        
        // Sauvegarder le score final pour la scène de fin
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.Save();
        
        // Aller à la scène de fin immédiatement
        LoadEndScene();
    }
    
    private void LoadEndScene()
    {
        // Charger la scène de fin directement par nom
        try
        {
            SceneManager.LoadScene("End");
        }
        catch
        {
            // Si ça ne marche pas, essayer avec le chemin complet
            try
            {
                SceneManager.LoadScene("Assets/Scenes/walid/End");
            }
            catch
            {
                Debug.LogError("Impossible de charger la scène de fin. Vérifiez qu'elle est dans les Build Settings.");
            }
        }
    }
    
    public float GetRemainingTime()
    {
        return currentTime;
    }
    
    public bool IsGameActive()
    {
        return isGameActive;
    }
}
