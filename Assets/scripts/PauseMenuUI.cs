using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Image backgroundPanel;
    
    private void Start()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(OnResumeButtonClicked);
        }
        
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }
    }
    
    private void OnResumeButtonClicked()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.ResumeGame();
        }
    }
    
    private void OnRestartButtonClicked()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.RestartGame();
        }
    }
    
    private void OnMainMenuButtonClicked()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.ReturnToMainMenu();
        }
    }
    
    private void OnDestroy()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
        }
        
        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
        }
    }
}