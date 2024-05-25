using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Movement : MonoBehaviour
{
    public Game game;

    public float moveDistance = 1.0f;
    public float moveInterval = 1.0f;

    private float timer;

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

    private void MonsterMove()
    {
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        int direction;

        direction = Random.Range(0, 4);

        Vector3 moveDirection = Vector3.zero;
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

        transform.position += moveDirection * moveDistance;
    }
}
