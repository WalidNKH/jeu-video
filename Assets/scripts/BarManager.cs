using UnityEngine;
using System.Collections;

public class BarManager : MonoBehaviour
{

    [SerializeField] private GameObject plateauBar;
    
    //Get the SandwichManager who own all the functions to manage the sandwich
    private GameManager _gameManager;

    //On start we set the table to waiting for a sandwich
    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    //Stream the value needPlateauOnBar to the GameManager to make active the plateau on the bar
    public void waitForPlateauOnBar()
    {
        //Set the bar to waiting for a sandwich
        if (_gameManager.needPlateauOnBar)
        {
            Debug.Log("Bar waiting for sandwich: " + _gameManager.needPlateauOnBar);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        
    }
}