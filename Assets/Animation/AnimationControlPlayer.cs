using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControlPlayer : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer sprite;
    public PlayerState playerState;

    void Awake(){
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerState = GetComponentInParent<PlayerState>();
    }

    void Update(){
        animator.SetBool("IsHurt", playerState.meetMine);
        animator.SetBool("IsHurt", playerState.meetMonster);

        if(playerState.facing == PlayerState.Facing.Left){
            sprite.flipX = true;
        }
        if(playerState.facing == PlayerState.Facing.Right){
            sprite.flipX = false;
        }
    }
}
