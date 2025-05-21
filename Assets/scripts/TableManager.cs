using UnityEngine;
using System.Collections;
using StarterAssets;

public class TableManager : MonoBehaviour
{
    public static TableManager Instance { get; private set; }

    
    public bool isTableWaitingForSandwich;

    [SerializeField] private GameObject plateauTable;
    [SerializeField] private GameObject alertCircle;
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
    
    public void ToggleTableWaiting(bool value)
    {
        isTableWaitingForSandwich = value;
        plateauTable.SetActive(!value);

        if (value)
        {   
            StartCoroutine(BlinkAlert());
        }
    }

    private IEnumerator BlinkAlert()
    {
        alertCircle.SetActive(true);
        while (isTableWaitingForSandwich)
        {
            alertCircle.SetActive(!alertCircle.activeSelf);
            yield return new WaitForSeconds(0.5f);
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
            alertCircle.SetActive(false);
        }
    }
}