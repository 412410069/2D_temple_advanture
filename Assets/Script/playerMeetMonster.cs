using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMeetMonster : MonoBehaviour
{
    PlayerState playerState;
    Game game;
    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponent<PlayerState>();
        game = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision2D){
        Debug.Log("meet monster1!");        //沒觸發    
        if(collision2D.gameObject.tag == "monster1"){
            Debug.Log("meet monster1!");
            playermeetMonsterAndEndGame();
        }
        else if(collision2D.gameObject.tag == "monster2"){
            Debug.Log("meet monster2!");
            playermeetMonsterAndEndGame();
        }
        else if(collision2D.gameObject.tag == "monster3"){
            Debug.Log("meet monster3!");
            playermeetMonsterAndEndGame();
        }
        else if(collision2D.gameObject.tag == "monster4"){
            Debug.Log("meet monster4!");
            playermeetMonsterAndEndGame();
        }  
    }

    public void playermeetMonsterAndEndGame(){
         playerState.gameOver = true;
            playerState.meetMonster = true;
            game.gameOver();
    }
}