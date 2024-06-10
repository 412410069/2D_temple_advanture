using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour
{
    public Text text;
    public PlayerState playerState;
    private String causeOfDeath;
    void Awake(){
        text = GetComponentInChildren<Text>();
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
    }

    void Update(){
        if(playerState.gameOver){
            Death();
            UpdateText();
        }
    }

    private void Death(){
        if(playerState.meetMine){
            causeOfDeath = "Boooooom!!!";
        }
        if(playerState.meetMonster){
            causeOfDeath = "Got Slayed By Monsters";
        }
    }

    private void UpdateText(){
        text.text = "Cause Of Death: " + causeOfDeath;
    }
}
