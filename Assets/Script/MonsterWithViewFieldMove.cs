using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMonsterWithViewFieldMove : MonoBehaviour
{
    public Game game;
    public Monster1Movement Monster1Movement;
    public float moveDistance = 1.0f;
    public float moveInterval = 1.0f;

    public GameObject player;

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
        Monster1Movement = GameObject.FindGameObjectWithTag("monster1").GetComponent<Monster1Movement>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= Monster1Movement.moveInterval)
        {
            MonsterViewMove();
            timer = 0f;
        }
    }

    void MonsterViewMove(){
        player_position = player.transform.position;
        moveDirection = Vector3.zero;

        player_x = player_position.x;
        player_y = player_position.y;
        x = (int)transform.position.x;
        y = (int)transform.position.y;

        // if(){
        //     Monster1Movement.MonsterMoveTrack();
        // }

        // else{
        //     Monster1Movement.MonsterMoveRandom();
        // }

        transform.position += moveDirection * moveDistance;
    }
}
