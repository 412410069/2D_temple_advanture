//Q技能：探測，功能為可以遠距開啟一格資訊
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Q : MonoBehaviour
{
    public float secondRate = 1;//
    public float searchMineTimer;
    public float searchMineTime;
    public float defultSearchMineTime = 10; 
    public GetCellLogic getCellLogic;

    private void Awake(){
        getCellLogic = GameObject.FindGameObjectWithTag("grid").GetComponent<GetCellLogic>();
    }

    public void isValidSearchMine(PlayerState playerState, Board board, Game game, Cell[, ] state){
        if (searchMineTimer < secondRate){
            searchMineTimer += Time.deltaTime;
        }
        else{
            if (searchMineTime > 0){
                searchMineTime -=1;
                searchMineTimer = 0;
            }
        }
        if (!playerState.gameOver && searchMineTime <= 0 && !playerState.spellCooldown){
            searchMine(playerState, board, game, state);
        }
        // Debug.Log(searchMineTime);
    }

    private void searchMine(PlayerState playerState, Board board, Game game, Cell[, ] state){
        UnityEngine.Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.Tilemap.WorldToCell(WorldPosition);
        Cell cell = getCellLogic.GetCell(cellPosition.x,cellPosition.y, state); 
        if (!playerState.gameOver && Input.GetKeyDown(KeyCode.Q)){
            if (cell.revealed){
                playerState.spellCooldown = false;
                return;
            }
            cell.revealed = true;
            state[cellPosition.x,cellPosition.y] = cell;
            board.Draw(state);    
            searchMineTime = defultSearchMineTime;
            playerState.spellCooldown = true;
            Debug.Log(playerState.spellCooldown);
        }
    }
}
