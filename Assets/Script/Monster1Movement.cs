using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Movement : MonoBehaviour
{
    public GameObject monster1;

    public float moveDistance = 1.0f;
    public float moveInterval = 1.0f;

    private float timer;
    
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
        int direction = Random.Range(0, 4);

        Vector3 moveDirection = Vector3.zero;
        switch (direction)
        {
            case 0:
                moveDirection = Vector3.up;
                break;
            case 1:
                moveDirection = Vector3.down;
                break;
            case 2:
                moveDirection = Vector3.left;
                break;
            case 3:
                moveDirection = Vector3.right;
                break;
        }

        monster1.transform.position += moveDirection * moveDistance;
    }
}
