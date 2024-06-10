using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WalkerGeneration : MonoBehaviour
{   
    public List<WalkerObject> walkers;
    public int maxWalkers = 10;
    public int tileCount = 0;
    public float fillPercentage = .4f;
    public float waitTime = .01f;
    public bool tileReavel = false;

    public void Generate(Cell[,] dungeon){
        InitializeGrid(dungeon);
    }

    private void InitializeGrid(Cell[,] dungeon){
        walkers = new List<WalkerObject>();

        Vector3Int tileCenter = new Vector3Int(dungeon.GetLength(0) / 2, dungeon.GetLength(1) / 2, 0);

        WalkerObject curWalker = new WalkerObject(tileCenter, GetDirection(), .5f);
        dungeon[tileCenter.x, tileCenter.y].type = Cell.Type.Empty;
        walkers.Add(curWalker);

        tileCount++;

        //StartCoroutine();
        CreateEmptys(dungeon);
        CreateWalls(dungeon);
        CreateExit(dungeon);
    }
    
    private Vector3Int GetDirection(){
        int choice = Random.Range(0, 4);

        switch(choice){
            case 0:
                return Vector3Int.down;
            case 1:
                return Vector3Int.left;
            case 2:
                return Vector3Int.right;
            case 3:
                return Vector3Int.up;
            default:
                return Vector3Int.zero; 
        }
    }

    private void CreateEmptys(Cell[,] dungeon){
        SetEmpty(dungeon[dungeon.GetLength(0) / 2, dungeon.GetLength(1) / 2]);
        
        while((float)tileCount / (float)dungeon.Length < fillPercentage){
            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition(dungeon);
            
            //bool hasCreatedEmpty = false;
            foreach(WalkerObject curWalker in walkers){
                Vector3Int curPos = curWalker.position;

                if(dungeon[curPos.x, curPos.y].type != Cell.Type.Empty){
                    SetEmpty(dungeon[curPos.x, curPos.y]);
                    tileCount++;
                    //hasCreatedEmpty = true;
                }
            }
            
            // if(hasCreatedEmpty){
            //     yield return new WaitForSeconds(waitTime);
            // }
        }

        //Broaden(dungeon);
    }

    private void ChanceToRemove(){
        int updatedCount = walkers.Count;
        for(int i = 0; i < updatedCount; i++){
            if(UnityEngine.Random.value < walkers[i].chanceToChange && walkers.Count > 1){
                walkers.RemoveAt(i);
                break;
            }
        }
    }

    private void ChanceToRedirect(){
        for(int i = 0; i < walkers.Count; i++){
            if(UnityEngine.Random.value < walkers[i].chanceToChange){
                WalkerObject curWalker = walkers[i];
                curWalker.direction = GetDirection();
                walkers[i] = curWalker;
            }
        }
    }

    private void ChanceToCreate(){
        int updateCount = walkers.Count;
        for(int i = 0; i < updateCount; i++){
            if(UnityEngine.Random.value < walkers[i].chanceToChange && walkers.Count < maxWalkers){
                Vector3Int newDirection = GetDirection();
                Vector3Int newPosition = walkers[i].position;

                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, .5f);
                walkers.Add(newWalker);
            }
        }
    }

    private void UpdatePosition(Cell[,] dungeon){
        for(int i = 0; i < walkers.Count; i++){
            WalkerObject foundWalker = walkers[i];
            foundWalker.position += foundWalker.direction;
            foundWalker.position.x = Mathf.Clamp(foundWalker.position.x, 2, dungeon.GetLength(0) - 3);
            foundWalker.position.y = Mathf.Clamp(foundWalker.position.y, 2, dungeon.GetLength(0) - 3);
            walkers[i] = foundWalker;
        }
    }

    private void Broaden(Cell[,] dungeon){
        CreateWalls(dungeon);
        for(int x = 0; x < dungeon.GetLength(0) - 1; x++){
            for(int y = 0; y < dungeon.GetLength(1) - 1; y++){
                if(dungeon[x, y].type == Cell.Type.Wall){
                    SetEmpty(dungeon[x, y]);
                }
            }
        }
    }

    private void SetEmpty(Cell cell){
        cell.type = Cell.Type.Empty;
        cell.revealed = tileReavel;
    }

    private void CreateWalls(Cell[,] dungeon){
        for(int x = 0; x < dungeon.GetLength(0) - 1; x++){
            for(int y = 0; y < dungeon.GetLength(1) - 1; y++){
                if(dungeon[x, y].type == Cell.Type.Empty){
                    SetWall(dungeon[x + 1, y]);
                    SetWall(dungeon[x + 1, y + 1]);
                    SetWall(dungeon[x, y + 1]);
                    SetWall(dungeon[x - 1, y + 1]);
                    SetWall(dungeon[x - 1, y]);
                    SetWall(dungeon[x - 1, y - 1]);
                    SetWall(dungeon[x, y - 1]);
                    SetWall(dungeon[x + 1, y - 1]);
                }
            }
        }
    }

    private void SetWall(Cell cell){
        if(cell.type == Cell.Type.Void){
            cell.type = Cell.Type.Wall;
        }
    }

    private void CreateExit(Cell[,] dungeon){
        int index = Random.Range(0, walkers.Count - 1);
        Vector3Int pos = walkers[index].position;
        dungeon[pos.x, pos.y].type = Cell.Type.Exit;
        Debug.Log("Exit Generate");
    }
}