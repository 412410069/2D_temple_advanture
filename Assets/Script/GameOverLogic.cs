//遊戲結束時的處理
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameOverLogic : MonoBehaviour
{
    public GameObject exitScene;   //因為在一開始時他不是開的，我們沒辦法用程式找到他，一定要在inspector中拉進去
    public PlayerState playerState;

    void Awake(){
        exitScene = GameObject.Find("BackgroundBlur");
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
    }

    void Start(){
        exitScene.SetActive(false);
    }

    void Update(){
        if(playerState.gameOver){
            exitScene.SetActive(true);
        }
    }

    public void BackToMainMenu(){
        SceneManager.LoadSceneAsync(0);
    }

    // public void gameOver(){     //記得沒有辦法找到Scene
    //     exitScene.SetActive(true);
    // }
}

