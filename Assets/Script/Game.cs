using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Game : MonoBehaviour
{
    public int width = 32;
    public int height = 32;
    public int mineNum = 80;

    private Board board;
    private Cell[,] state;
    public PlayerState playerMovement;
    public GameObject player;

    private void Awake(){
        board = GetComponentInChildren<Board>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start(){
        NewGame();
    }

    private void NewGame(){
        state = new Cell[width, height];
        
        GenerateCells();
        GenerateDungeon();
        GenerateMines();
        GeneratePlayer();
        GenerateNumbers();

        board.Draw(state);
    }

    private void GenerateCells(){
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                Cell cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Void;
                cell.revealed = false;
                state[x, y] = cell;
            }
        }
    }

    private void GenerateDungeon(){
        for(int x = width / 4 - 1; x < width / 4 * 3 + 1; x++){
            for(int y = height / 4 - 1; y < height / 4 * 3 + 1; y++){
                state[x ,y].type = Cell.Type.Wall;
                state[x ,y].revealed = true;
            }
        }
        for(int x = width / 4; x < width / 4 * 3; x++){
            for(int y = height / 4; y < height / 4 * 3; y++){
                state[x ,y].type = Cell.Type.Empty;
            }
        }    
    }

    private void GeneratePlayer(){
        int x = 16;
        int y = 16;
        GenerateSafeZone(x, y);
        playerMovement.position = new Vector3Int(x, y, 0);
        player.transform.position = new Vector3Int(x, y, 0);
    }

    private void GenerateSafeZone(int playerX, int playerY){
        state[playerX, playerY].revealed = true;
        for(int adjacentX = -1; adjacentX <=  1; adjacentX++){
            for(int adjacentY = -1; adjacentY <= 1; adjacentY++){
                int x = playerX + adjacentX;
                int y = playerY + adjacentY;

                if(state[x, y].type == Cell.Type.Mine){
                    state[x, y].type = Cell.Type.Empty;
                }
            }
        }
    }

    private void GenerateMines(){
        for(int i = 0; i < mineNum; i++){
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            
            if(state[x, y].type == Cell.Type.Empty){
                state[x, y].type = Cell.Type.Mine;
            }
        }
    }

    private void GenerateNumbers(){
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                Cell cell = state[x, y];

                if(cell.type != Cell.Type.Empty){
                    continue;
                }

                cell.number = CountMines(x, y);

                if(cell.number > 0){
                    state[x, y].type = Cell.Type.Number;
                }
            }
        }
    }

    private int CountMines(int cellX, int cellY){
        int count = 0;

        for(int adjacentX = -1; adjacentX <=  1; adjacentX++){
            for(int adjacentY = -1; adjacentY <= 1; adjacentY++){
                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                if(state[x, y].type == Cell.Type.Mine){
                    count++;
                }
            }
        }

        return count;
    }

    void Update()
    {
        Reavel();
        PlayerMeetMine();
        
    }

    private void PlayerMeetMine(){
        //hey
    }
    private void Reavel(){
        int x = playerMovement.position.x;
        int y = playerMovement.position.y;
        Cell cell = state[x, y];
        
        if(cell.type == Cell.Type.Empty){
            Flood(cell);
        }
        //hey hey
        cell.revealed = true;
        state[x, y] = cell;
        board.Draw(state);
    }

    private void Flood(Cell cell){
        if(cell.revealed) return;
        if(cell.type == Cell.Type.Mine || cell.type == Cell.Type.Wall) return;

        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        if(cell.type == Cell.Type.Empty){
            Flood(state[cell.position.x + 1, cell.position.y]);
            Flood(state[cell.position.x - 1, cell.position.y]);
            Flood(state[cell.position.x, cell.position.y + 1]);
            Flood(state[cell.position.x, cell.position.y - 1]);
        }
    }
}
