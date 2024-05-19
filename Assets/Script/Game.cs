using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public int width = 32;
    public int height = 32;
    public int mineNum = 80;
    

    private Board board;
    public Cell[,] state;
    public PlayerState playerState;
    public GameObject player;
    public MainMenu mainMenu;
    public GameObject exitScene;
    public GameObject shield;
    public float shieldOpenTime;
    public float defultShieldOpenTime = 3;
    public float shieldTimer;
    public float secondRate = 1;

    private void Awake(){
        board = GetComponentInChildren<Board>();
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
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
                cell.revealed = true;
                state[x, y] = cell;
            }
        }
    }

    private void GenerateDungeon(){
        for(int x = width / 4 - 1; x < width / 4 * 3 + 1; x++){
            for(int y = height / 4 - 1; y < height / 4 * 3 + 1; y++){
                state[x ,y].type = Cell.Type.Wall;
            }
        }
        for(int x = width / 4; x < width / 4 * 3; x++){
            for(int y = height / 4; y < height / 4 * 3; y++){
                state[x ,y].type = Cell.Type.Empty;
                state[x ,y].revealed = false;
            }
        }    
    }

    private void GeneratePlayer(){
        int x = 16;
        int y = 16;
        GenerateSafeZone(x, y);
        playerState.position = new Vector3Int(x, y, 0);
        player.transform.position = new Vector3Int(x, y, 0);
        playerState.meetMine = false;
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
        // PlayerMeetMonster();
        itemShield();
        searchMine();
    }

    private void Reavel(){
        int x = playerState.position.x;
        int y = playerState.position.y;
        Cell cell = state[x, y];
        
        if(cell.type == Cell.Type.Empty){
            Flood(cell);
        }
        //hey hey
        cell.revealed = true;
        state[x, y] = cell;
        board.Draw(state);
    }

    private void PlayerMeetMine(){
        int x = playerState.position.x;
        int y = playerState.position.y;
        if(!playerState.gameOver && !playerState.isMoving && !playerState.isShieldOpen){
            if(state[x, y].type == Cell.Type.Mine){
                Debug.Log("meet mine!");
                playerState.meetMine = true;
                playerState.gameOver = true;
                gameOver();             //還沒辦法找到ExitScene
            }
        }
    }
    // private void PlayerMeetMonster(){           //我還不知道怪物到底是怎麼寫出來
    //     int x = playerState.position.x;
    //     int y = playerState.position.y;
    //     if(!playerState.gameOver && !playerState.isMoving){
    //         if(state[x, y].type == Cell.Type.Monster){
    //             Debug.Log("meet Monster!");
    //             playerState.meetMonster = true;
    //             playerState.gameOver = true;
    //             gameOver();             //還沒辦法找到ExitScene
    //         }
    //     }
    // }

    private void itemShield(){
        if(!playerState.gameOver && Input.GetKeyDown(KeyCode.Keypad1) == true){
            openShield();
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
            closeShield();
        }
    }

    private void openShield(){      //開啟護盾的數秒內，碰到炸彈不會結束遊戲
        Debug.Log("Shield open!");
        playerState.isShieldOpen = true;
        shieldOpenTime = defultShieldOpenTime;
        shield.SetActive(true);
    }
    private void closeShield(){ 
        Debug.Log("Shield closed");
        playerState.isShieldOpen = false;
        shield.SetActive(false);
        shieldOpenTime = -1;
    }

    private void searchMine(){
        Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.Tilemap.WorldToCell(WorldPosition);
        Cell cell = GetCell(cellPosition.x,cellPosition.y); 
        if (!playerState.gameOver && Input.GetKeyDown(KeyCode.Q)){
            if (cell.type == Cell.Type.Empty){
                Flood(cell);
            }
            else if (cell.revealed){
                return;
            }
            cell.revealed = true;
            state[cellPosition.x,cellPosition.y] = cell;
            board.Draw(state);    
            
        }
    }

    private Cell GetCell(int x, int y){
        if (IsVaild(x,y)){
            return state[x,y];
        }
        else{
            return new Cell();
        }
    }

    private bool IsVaild(int x, int y){
        return x >= 0 && x < width && y >= 0 && y < height; 
    }

    private void Flood(Cell cell){
        if(cell.revealed) return;
        if(cell.type == Cell.Type.Mine || cell.type == Cell.Type.Wall) return;

        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        if(cell.type == Cell.Type.Empty){
            Flood(state[cell.position.x + 1, cell.position.y]);
            Flood(state[cell.position.x + 1, cell.position.y + 1]);
            Flood(state[cell.position.x, cell.position.y + 1]);
            Flood(state[cell.position.x - 1, cell.position.y + 1]);
            Flood(state[cell.position.x - 1, cell.position.y]);
            Flood(state[cell.position.x - 1, cell.position.y - 1]);
            Flood(state[cell.position.x, cell.position.y - 1]);
            Flood(state[cell.position.x + 1, cell.position.y - 1]);
        }
    }

    public void backToMainMenu(){
        SceneManager.LoadSceneAsync(0);
    }

    public void gameOver(){     //記得沒有辦法找到Scene
        exitScene.SetActive(true);
    }
}
