//E技能：念力，功能為抓起怪物
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_E : MonoBehaviour
{
    public void forceMonster(PlayerState playerState, Board board){
        UnityEngine.Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.Tilemap.WorldToCell(WorldPosition);
        if (!playerState.gameOver && Input.GetKeyDown(KeyCode.E)){
            playerState.isValidMonsterMovement = true;
            Debug.Log("EEEEEE");
        }
    }
}
