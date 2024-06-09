using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeetMineLogic : MonoBehaviour
{
    public GameOverLogic gameOverLogic;

    public void PlayerMeetMine(PlayerState playerState, Cell[,] state){
        int x = playerState.position.x;
        int y = playerState.position.y;
        if(!playerState.gameOver && !playerState.isMoving && !playerState.isShieldOpen){
            if(state[x, y].type == Cell.Type.Mine){
                Debug.Log("meet mine!");
                playerState.meetMine = true;
                playerState.gameOver = true;
                gameOverLogic.gameOver();             //還沒辦法找到ExitScene
            }
        }
    }
}