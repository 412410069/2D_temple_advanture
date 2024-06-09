using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{   
    public PlayerState playerState;

    void Awake(){
        playerState = GetComponent<PlayerState>();
    }

    void Update(){
        
    }
}
