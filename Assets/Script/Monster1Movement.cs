using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Movement : MonoBehaviour
{
    public Game game;

    public float moveDistance = 1.0f;
    public float moveInterval = 1.0f;

    public GameObject player;
    public int trackDistance = 10;

    private float timer;

    private Vector2 player_position;
    private Vector3 moveDirection;
    private float player_x;
    private float player_y;

    private int x;
    private int y;

    private int direction;

    void Awake(){
        game = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>();
    }
    
    private void Update(){
        timer += Time.deltaTime;

        if (timer >= moveInterval)
        {
            MonsterMove();
            timer = 0f;
        }
    }

    private void MonsterMove(){
        player_position = player.transform.position;
        moveDirection = Vector3.zero;

        player_x = player_position.x;
        player_y = player_position.y;
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        
        if(Mathf.Abs(player_x - (int)x) + Mathf.Abs(player_y - (int)y) <= trackDistance){
            MonsterMoveTrack();
        }

        else{
            MonsterMoveRandom();
        }

        transform.position += moveDirection * moveDistance;
    }

    public void MonsterMoveTrack(){
        if(player_x != x && player_y != y)
        direction = Random.Range(0, 2);

        else if(player_x != x)
        direction = 0;

        else if(player_y != y)
        direction = 1;

        else
        direction = 2;

        for(int i=0;i<2;++i){
            if(direction == 0){
                if(player_x > x && (game.state[x + 1, y].type != Cell.Type.Wall)){
                    moveDirection = Vector3.right;
                }

                else if(player_x < x && (game.state[x - 1, y].type != Cell.Type.Wall)){
                        moveDirection = Vector3.left;
                }
                    
                else{
                    direction = 1;
                    continue;
                }
            }

            else if(direction == 1){
                if(player_y > y && (game.state[x, y + 1].type != Cell.Type.Wall)){
                    moveDirection = Vector3.up;
                }

                else if(player_y < y && (game.state[x, y - 1].type != Cell.Type.Wall)){
                        moveDirection = Vector3.down;
                }

                else{
                    direction = 0;
                    continue;
                }
            }

            else
            moveDirection = Vector3.zero;
        }
    }

    public void MonsterMoveRandom(){
        direction = Random.Range(0, 4);

        switch (direction)
        {
            case 0:
                if(game.state[x, y + 1].type == Cell.Type.Wall || game.state[x, y + 1].type == Cell.Type.Void) return;
                if(game.state[x, y + 1].revealed && game.state[x, y + 1].type == Cell.Type.Mine) return;
                moveDirection = Vector3.up;
                break;
            case 1:
                if(game.state[x, y - 1].type == Cell.Type.Wall || game.state[x, y - 1].type == Cell.Type.Void) return;
                if(game.state[x, y - 1].revealed && game.state[x, y - 1].type == Cell.Type.Mine) return;
                moveDirection = Vector3.down;
                break;
            case 2:
                if(game.state[x - 1, y].type == Cell.Type.Wall || game.state[x - 1, y].type == Cell.Type.Void) return;
                if(game.state[x - 1, y].revealed && game.state[x - 1, y].type == Cell.Type.Mine) return;
                moveDirection = Vector3.left;
                break;
            case 3:
                if(game.state[x + 1, y].type == Cell.Type.Wall || game.state[x + 1, y].type == Cell.Type.Void) return;
                if(game.state[x + 1, y].revealed && game.state[x + 1, y].type == Cell.Type.Mine) return;
                moveDirection = Vector3.right;
                break;
        }
    }
}