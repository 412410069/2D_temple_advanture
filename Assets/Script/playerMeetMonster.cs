using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMeetMonster : MonoBehaviour
{
    PlayerState playerState;
    public GameOverLogic gameOverLogic; //因為在一開始時他不是開的，我們沒辦法用程式找到他，一定要在inspector中拉進去
    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponent<PlayerState>();
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
            gameOverLogic.gameOver();
    }
}
