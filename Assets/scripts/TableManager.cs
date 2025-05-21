using UnityEngine;
using System.Collections;

public class TableManager : MonoBehaviour
{
    public bool isTableWaitingForSandwich;

    [SerializeField] private GameObject plateauTable;
    
    //Get the SandwichManager who own all the functions to manage the sandwich
    private GameManager _gameManager;

    //On start we set the table to waiting for a sandwich
    void Start()
    {
        _gameManager = GameManager.Instance;
        
        //Initialise the table to waiting for a sandwich
        _gameManager.setNeedPlateauOnBar(true);
        ToggleTableWaiting(true);
    }
    
    private void ToggleTableWaiting(bool value)
    {
        //Set the table to waiting for a sandwich
        isTableWaitingForSandwich = value;
        Debug.Log("Table waiting for sandwich: " + isTableWaitingForSandwich);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        //If the player enter the trigger and he has a sandwich in his hand
        //And if the table is waiting for a sandwich
        if(_gameManager.IsSandwichActive() && isTableWaitingForSandwich)
        {
            //Set the table to not waiting for a sandwich
            ToggleTableWaiting(false);
            //Set the isSandwichActive value to false on the PlayerArmature Object
            _gameManager.SetIsSandwichActive(false);
            //Set the plateau to inactive on the table
            plateauTable.SetActive(false);
            
            Debug.Log("Sandwich placed on table");
        }
        else 
        {
            Debug.Log("Pas de sandwich ou table pas en attente");
        }
    }
}