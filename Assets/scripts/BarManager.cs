using UnityEngine;
using StarterAssets;

public class BarManager : MonoBehaviour
{
    public static BarManager Instance { get; private set; }

    [SerializeField] public GameObject plateauBar;
    [SerializeField] private ThirdPersonController playerController;
    
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
    
    private void OnTriggerEnter(Collider other)
    {
        if(!playerController.IsHoldingSandwich && plateauBar.activeSelf)
        {
            plateauBar.SetActive(false);
            playerController.IsHoldingSandwich = true;
            playerController.BackPack.SetActive(true);
        }
    }
    
    public void setNeedPlateauOnBar(bool value)
    {
        plateauBar.SetActive(value);
    }
}