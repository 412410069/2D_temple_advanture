using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public PlayerState player;
    public float speed;
    public Cell cell;

    void Awake()
    {
        player = GetComponent<PlayerState>();
    }

    void Update()
    {   
        IsMovingCheck();
        if(!player.isMoving){
            PlayerInput();
        }
        else{
            PlayerMove();
        }
    }

    private void IsMovingCheck(){
        if(transform.position != player.position) player.isMoving = true;
        else player.isMoving = false;
    }

    private void PlayerInput(){
        if(Input.GetKeyDown(KeyCode.W) == true){
            player.facing = PlayerState.Facing.Up;
            player.position += new Vector3Int(0, 1, 0);
        }
        if(Input.GetKeyDown(KeyCode.S) == true){
            player.facing = PlayerState.Facing.Down;
            player.position += new Vector3Int(0, -1, 0);
        }
        if(Input.GetKeyDown(KeyCode.A) == true){
            player.facing = PlayerState.Facing.Left;
            player.position += new Vector3Int(-1, 0, 0);
        }
        if(Input.GetKeyDown(KeyCode.D) == true){
            player.facing = PlayerState.Facing.Right;
            player.position += new Vector3Int(1, 0, 0);
        }
    }

    private void PlayerMove(){
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
