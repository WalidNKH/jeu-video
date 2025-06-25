using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }
    
    [SerializeField] private Text timerText;
    [SerializeField] private float gameTime = 60f; // 1 minute
    
    private float currentTime;
    private bool isGameActive = true;
    
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
    
    private void Start()
    {
        currentTime = gameTime;
        UpdateTimerDisplay();
    }
    
    private void Update()
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
    
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    private void EndGame()
    {
        isGameActive = false;
        currentTime = 0;
        UpdateTimerDisplay();
        
        // Sauvegarder le score final pour la scène de fin
        PlayerPrefs.SetInt("FinalScore", ScoreManager.Instance.score);
        PlayerPrefs.Save();
        
        // Aller à la scène de fin après un petit délai
        Invoke("LoadEndScene", 1f);
    }
    
    private void LoadEndScene()
    {
        SceneManager.LoadScene("End");
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