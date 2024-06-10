using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameOverText : MonoBehaviour
{
    public Text text;
    public PlayerState playerState;
    private String causeOfDeath = "";
    private String escapedText = "";

    void Awake(){
        text = GetComponentInChildren<Text>();
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
    }
    
    void Start(){
        Reason();
        UpdateText();
    }

    private void Reason(){
        if(playerState.escaped){
            Escape();
        }
        Death();
    }

    private void Escape(){
        // if(Random.Range(0, 1) == 1){
            escapedText = "A Successful Run";
        // }
        // else{
        //     escapedText = "Flee Like A Chicken";
        // }
    }

    private void Death(){
        if(playerState.meetMine){
            causeOfDeath = "Boooooom!!!";
        }
        if(playerState.meetMonster){
            if(playerState.isValidMonsterMovement){
                causeOfDeath = "(>`∀´)> Stuuuupid! What A Failure";
            }
            else{
                causeOfDeath = "Got Slayed By Monsters";
            }
        }
    }

    private void UpdateText(){
        if(causeOfDeath != ""){
            text.text = "Cause Of Death: " + causeOfDeath;
        }
        if(escapedText != ""){
            text.text = escapedText;
        }
    }
}
