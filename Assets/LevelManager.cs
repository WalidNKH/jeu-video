using UnityEngine;
 
using UnityEngine.SceneManagement;

 using System;
 
 
public class LevelManager : MonoBehaviour

 {

     public void ChangeScene(string sceneName)

     {

         SceneManager.LoadScene(sceneName);

     }
 
    public void Quit()

     {

         Application.Quit();

     }
 
 
}