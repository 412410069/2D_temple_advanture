using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShield : MonoBehaviour
{
    // public PlayerState playerState;      //這邊有一個很酷的東西，如果我想要用awake那邊的去找playerState，會找不到，完全不知道為什麼
    public GameObject shield;   //去unity抓shield過來
    public float shieldOpenTime;
    public float defultShieldOpenTime = 3;
    public float shieldTimer;
    public float secondRate = 1;

    private void Awake(){
        // playerState = GetComponentInParent<PlayerState>();
    }

    public void itemShield(PlayerState playerState){
        if(!playerState.gameOver && (Input.GetKeyDown(KeyCode.Keypad1) == true || Input.GetKeyDown(KeyCode.Alpha1) == true)){
            openShield(playerState);
        }

        if(shieldTimer < secondRate){           //控制真實秒數時間
            shieldTimer += Time.deltaTime;
        }
        else{
            if(shieldOpenTime > 0){
                shieldOpenTime -= 1;
                shieldTimer = 0;
            }
        }
        if(playerState.isShieldOpen && shieldOpenTime == 0){
            closeShield(playerState);
        }
    }

    private void openShield(PlayerState playerState){      //開啟護盾的數秒內，碰到炸彈不會結束遊戲
        Debug.Log("Shield open!");
        playerState.isShieldOpen = true;
        shieldOpenTime = defultShieldOpenTime;
        shield.SetActive(true);
    }
    private void closeShield(PlayerState playerState){ 
        Debug.Log("Shield closed");
        playerState.isShieldOpen = false;
        shield.SetActive(false);
        shieldOpenTime = -1;
    }
}
