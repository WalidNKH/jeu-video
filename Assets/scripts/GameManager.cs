using UnityEngine;
using System.Collections;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    public ThirdPersonController playerController;
//We assume that the GameManager is instantiated in an object who contain a plateau prefab
//We can have multiple GameManager in the scene, one for each table

//In this file we have isSandwichActive variable that is set to true when the player enters the trigger on a table wich mean the player has a sandwich in his hand
//We have to also set a table to waiting for a sandwich to be placed on it, if a table is waiting it means that on the bar we have a sandwich waiting to be picked up
//So the flow is:
//1. A table is waiting for a sandwich to be placed on it (asking for a sandwich)
//2. The player see that the table is waiting for a sandwich and he can pick up a sandwich from the bar
//3. The player pick up a sandwich from the bar 
//4. The player place the sandwich on the table
//5. The table is no longer waiting for a sandwich

//This script is the manager, it only manage the state of the bar, sandwich and table

    public void SetIsSandwichActive(bool value)
    {
        //Set the isSandwichActive value to false on the PlayerArmature Object
        playerController.IsSandwichActive = value;
    }

    //Create bool getter for isSandwichActive
    public bool IsSandwichActive()
    {
        return playerController.IsSandwichActive;
    }
}
