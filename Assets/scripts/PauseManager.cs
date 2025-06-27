using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private AudioSource backgroundMusic;
    private bool _isPaused = false;
    private float _previousMusicVolume;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
        
        if (backgroundMusic == null)
        {
            backgroundMusic = FindObjectOfType<AudioSource>();
        }
        
        if (backgroundMusic != null)
        {
            _previousMusicVolume = backgroundMusic.volume;
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    
    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(true);
        }
        
        if (backgroundMusic != null)
        {
            backgroundMusic.Pause();
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
        
        if (backgroundMusic != null)
        {
            backgroundMusic.UnPause();
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    
    public bool IsPaused()
    {
        return _isPaused;
    }
}