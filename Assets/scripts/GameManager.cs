using UnityEngine;
using StarterAssets;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public ThirdPersonController playerController;
    private BarManager _barManager;
    private TableManager _tableManager;
    
    public GameObject[] tableManagers;
    private TableManager[] _tableManagersArray;
    
    private void Start()
    {
        playerController.BackPack.SetActive(false);
        playerController.IsHoldingSandwich = false;
        _barManager = BarManager.Instance;

        // Activer le plateau au bar
        _barManager.setNeedPlateauOnBar(true);

        // Initialiser chaque table
        _tableManagersArray = new TableManager[tableManagers.Length];
        for (int i = 0; i < tableManagers.Length; i++)
        {
            TableManager tableManager = tableManagers[i].GetComponent<TableManager>();
            if (tableManager != null)
            {
                _tableManagersArray[i] = tableManager;
            }
            else
            {
                Debug.LogError("TableManager component not found on the GameObject: " + tableManagers[i].name);
            }
        }
        
        // Démarrer le jeu avec une première table en attente
        Invoke("SetRandomTableWaiting", 0.5f);
    }

    private void Update()
    {
        // Retrait de l'appel automatique - sera géré après chaque livraison
    }

    public void SetRandomTableWaiting()
    {
        // Vérifier si le jeu est encore actif
        if (ScoreManager.Instance != null && !ScoreManager.Instance.IsGameActive())
        {
            Debug.Log("Game is no longer active, not selecting new table");
            return;
        }
        
        if (_tableManagersArray.Length > 0)
        {
            // Vérifier si une table est déjà en attente
            bool isAnyTableWaiting = false;
            foreach (var tableManager in _tableManagersArray)
            {
                if (tableManager != null && tableManager.isTableWaitingForSandwich)
                {
                    isAnyTableWaiting = true;
                    break; // Sortir de la boucle dès qu'une table est trouvée
                }
            }

            // Si aucune table n'est en attente, en sélectionner une aléatoirement
            if (!isAnyTableWaiting)
            {
                int randomIndex = Random.Range(0, _tableManagersArray.Length);

                // Vérifier si l'objet n'est pas détruit avant de l'utiliser
                if (_tableManagersArray[randomIndex] != null)
                {
                    Debug.Log($"Selecting table at index {randomIndex} to be waiting");
                    _tableManagersArray[randomIndex].ToggleTableWaiting(true);
                }
                else
                {
                    Debug.LogWarning("TableManager at index " + randomIndex + " is null or destroyed.");
                }
            }
        }
        else
        {
            Debug.LogError("No tables available in the array.");
        }
    }
    
    public void OnTableServed()
    {
        Debug.Log("OnTableServed called - Starting new cycle");
        
        // Vérifier si le jeu est encore actif
        if (!ScoreManager.Instance.IsGameActive())
        {
            Debug.Log("Game is no longer active, stopping table cycles");
            return;
        }
        
        // Réactiver le plateau au bar
        _barManager.setNeedPlateauOnBar(true);
        Debug.Log("Bar plate reactivated");
        
        // Attendre un court instant puis sélectionner une nouvelle table
        Invoke("SetRandomTableWaiting", 1f);
    }
}
