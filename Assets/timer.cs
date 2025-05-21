using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class timer : MonoBehaviour
{
[SerializeField] TextMeshProUGUI timerText;
[SerializeField] float remainingTime;

void Update(){
    if (remainingTime > 0)
    {
    remainingTime -= Time.deltaTime;
    }
    else if (remainingTime < 0)
    {
    remainingTime = 60;
    SceneManager.LoadScene("GameOver");
    }
    timerText.color = Color.red;
    int minutes = Mathf.FloorToInt (remainingTime / 60);
    int seconds = Mathf.FloorToInt(remainingTime % 60);
    timerText.text = string. Format ("{0:00}: {1:00}", minutes, seconds);
}
}