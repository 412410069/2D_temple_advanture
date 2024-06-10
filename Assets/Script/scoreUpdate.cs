using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public Text scoreText;
    // Update is called once per frame
    void Update()
    {
        viewScore();
    }

    void viewScore(){
        int score = PlayerPrefs.GetInt("HighScore", 0);
        scoreText.text = "High Score: " + score;
    }
}
