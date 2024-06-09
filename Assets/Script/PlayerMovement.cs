//處離玩家的移動
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public PlayerState playerState;
    public float speed;
    public Cell cell;
    public Game game;

    void Awake()
    {
        playerState = GetComponent<PlayerState>();
        game = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>();
    }

    void Update()
    {   
        IsMovingCheck();
        if(!playerState.isMoving){
            PlayerInput();
        }
        else{
            PlayerMove();
        }
    }

    private void IsMovingCheck(){
        if(transform.position != playerState.position) playerState.isMoving = true;
        else playerState.isMoving = false;
    }

    private void PlayerInput(){
        int x = playerState.position.x;
        int y = playerState.position.y;
        if(!playerState.gameOver && Input.GetKeyDown(KeyCode.W) == true){
            playerState.facing = PlayerState.Facing.Up;
            if(game.state[x, y + 1].type != Cell.Type.Wall){
                playerState.position += new Vector3Int(0, 1, 0);
            }
        }
        if(!playerState.gameOver && Input.GetKeyDown(KeyCode.S) == true){
            playerState.facing = PlayerState.Facing.Down;
            if(game.state[x, y - 1].type != Cell.Type.Wall){
                playerState.position += new Vector3Int(0, -1, 0);
            }
        }
        if(!playerState.gameOver && Input.GetKeyDown(KeyCode.A) == true){
            playerState.facing = PlayerState.Facing.Left;
            if(game.state[x - 1, y].type != Cell.Type.Wall){
                playerState.position += new Vector3Int(-1, 0, 0);
            }
        }
        if(!playerState.gameOver && Input.GetKeyDown(KeyCode.D) == true){
            playerState.facing = PlayerState.Facing.Right;
            if(game.state[x + 1, y].type != Cell.Type.Wall){
                playerState.position += new Vector3Int(1, 0, 0);
            }
        }
    }

    private void PlayerMove(){
        transform.position = Vector3.MoveTowards(transform.position, playerState.position, speed * Time.deltaTime);
    }
    private void PlayerMeetWall(char keyDirection){

    }
}
