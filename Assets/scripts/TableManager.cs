using UnityEngine;
using System.Collections;
using StarterAssets;

public class TableManager : MonoBehaviour
{
    public static TableManager Instance { get; private set; }

    private ScoreManager _scoreManager;    
    public bool isTableWaitingForSandwich;

    [SerializeField] private GameObject plateauTable;
    [SerializeField] private GameObject alertCircle;
    [SerializeField] private ThirdPersonController playerController;

    private void Start()
    {
        _scoreManager = ScoreManager.Instance;
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
    
    public void ToggleTableWaiting(bool value)
    {
        Debug.Log("waiting");
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
            _scoreManager.AddScore();
        }
    }
}