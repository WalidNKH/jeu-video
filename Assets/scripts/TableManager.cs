using UnityEngine;
using System.Collections;
using StarterAssets;

public class TableManager : MonoBehaviour
{
    private ScoreManager _scoreManager;
    private GameManager _gameManager;
    public bool isTableWaitingForSandwich;

    [SerializeField] private GameObject plateauTable;
    [SerializeField] private GameObject alertCircle;
    [SerializeField] private ThirdPersonController playerController;
    
    private Coroutine blinkCoroutine;
    private MeshRenderer[] plateauRenderers;

    private void Start()
    {
        _scoreManager = ScoreManager.Instance;
        _gameManager = FindObjectOfType<GameManager>();
        
        // Trouver le plateauTable si pas assigné
        if (plateauTable == null)
        {
            plateauTable = transform.Find("plateauTable")?.gameObject;
        }
        
        // Récupérer les renderers une seule fois
        if (plateauTable != null)
        {
            plateauRenderers = plateauTable.GetComponentsInChildren<MeshRenderer>();
            Debug.Log($"{gameObject.name}: Found {plateauRenderers.Length} MeshRenderers in plateauTable");
            for (int i = 0; i < plateauRenderers.Length; i++)
            {
                Debug.Log($"  Renderer {i}: {plateauRenderers[i].name}");
            }
        }
        else
        {
            Debug.LogError($"{gameObject.name}: plateauTable is NULL!");
        }
    }
    
    private void Awake()
    {
        // Removed singleton pattern as each table needs its own TableManager instance
    }
    
    public void ToggleTableWaiting(bool value)
    {
        Debug.Log($"Table {gameObject.name} - ToggleTableWaiting: {value}");
        isTableWaitingForSandwich = value;

        if (value)
        {   
            Debug.Log($"Starting blink alert for {gameObject.name}");
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }
            blinkCoroutine = StartCoroutine(BlinkPlateauAlert());
        }
        else
        {
            // Arrêter uniquement la coroutine de clignotement
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }
            
            // S'assurer que le plateau est visible
            SetPlateauVisible(true);
        }
    }

    private IEnumerator BlinkPlateauAlert()
    {
        Debug.Log($"BlinkPlateauAlert started for {gameObject.name}");
        
        while (isTableWaitingForSandwich)
        {
            SetPlateauVisible(false);
            yield return new WaitForSeconds(0.5f);
            
            if (!isTableWaitingForSandwich) break; // Vérifier avant de rallumer
            
            SetPlateauVisible(true);
            yield return new WaitForSeconds(0.5f);
        }
        
        // S'assurer que le plateau est visible à la fin
        SetPlateauVisible(true);
        Debug.Log($"BlinkPlateauAlert ended for {gameObject.name}");
    }
    
    private void SetPlateauVisible(bool visible)
    {
        if (plateauTable != null)
        {
            // Utiliser SetActive pour un effet plus visible
            plateauTable.SetActive(visible);
            Debug.Log($"Plateau {gameObject.name} SetActive: {visible}");
        }
        else
        {
            Debug.LogError($"Plateau {gameObject.name}: plateauTable is NULL!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(playerController.IsHoldingSandwich && isTableWaitingForSandwich)
        {
            ToggleTableWaiting(false);
            playerController.IsHoldingSandwich = false;
            isTableWaitingForSandwich = false;
            plateauTable.SetActive(true);
            playerController.BackPack.SetActive(false);
            _scoreManager.AddScore();
            
            // Notifier le GameManager qu'une table a été servie
            if (_gameManager != null)
            {
                _gameManager.OnTableServed();
            }
        }
    }
}