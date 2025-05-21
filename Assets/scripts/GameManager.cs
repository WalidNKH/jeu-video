using UnityEngine;
using System.Collections;
using StarterAssets;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public ThirdPersonController playerController;
    private BarManager _barManager;
    private TableManager _tableManager;
    
    private void Start()
    {
        playerController.BackPack.SetActive(false);
        playerController.IsHoldingSandwich = false;
        _barManager = BarManager.Instance;
        _tableManager = TableManager.Instance;
        
        _barManager.setNeedPlateauOnBar(true);
        _tableManager.ToggleTableWaiting(true);
    }
}
