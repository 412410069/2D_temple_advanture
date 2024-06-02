using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public int width = 32;
    public int height = 32;
    public int mineNum = 150;

    private Board board;
    public Cell[,] state;
    private WalkerGeneration walkerGeneration;
    public PlayerState playerState;
    public MonsterInitial monster;
    public GameObject player;
    public MainMenu mainMenu;
    public GameObject exitScene;
    public GameObject shield;
    public GlowGrid glowGrid;
    public float shieldOpenTime;
    public float defultShieldOpenTime = 3;
    public float shieldTimer;
    public float secondRate = 1;
    public float searchMineTimer;
    public float searchMineTime;
    public float defultSearchMineTime = 30; 
    public float glowsecond = 0;

    private void Awake(){
        board = GetComponentInChildren<Board>();
        walkerGeneration = GetComponent<WalkerGeneration>();
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        player = GameObject.FindGameObjectWithTag("Player");
        monster = GameObject.FindGameObjectWithTag("monster").GetComponent<MonsterInitial>();
        glowGrid = GetComponentInChildren<GlowGrid>();
        shield = GameObject.FindGameObjectWithTag("Player").gameObject.transform.GetChild(0).gameObject;
    }

    private void Start(){
        NewGame();
    }

    private void NewGame(){
        state = new Cell[width, height];
        
        GenerateCells();
        walkerGeneration.Generate(state);
        GenerateMines();
        GeneratePlayer();
        GenerateNumbers();
        board.Draw(state);
        monster.GenerateMonster1();
        monster.GenerateMonsterWithView();
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
        eraseGlow();
        Reavel();
        PlayerMeetMine();
        // PlayerMeetMonster();
        itemShield();
        isValidSearchMine();
        forceMonster();
        glow();
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
    

    private void itemShield(){
        if(!playerState.gameOver && (Input.GetKeyDown(KeyCode.Keypad1) == true || Input.GetKeyDown(KeyCode.Alpha1) == true)){
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

    private void isValidSearchMine(){
        if (searchMineTimer < secondRate){
            searchMineTimer += Time.deltaTime;
        }
        else{
            if (searchMineTime > 0){
                searchMineTime -=1;
                searchMineTimer = 0;
            }
        }
        if (!playerState.gameOver && searchMineTime <= 0){
            searchMine();
        }
    }

    private void searchMine(){
        Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.Tilemap.WorldToCell(WorldPosition);
        Cell cell = GetCell(cellPosition.x,cellPosition.y); 
        if (!playerState.gameOver && Input.GetKeyDown(KeyCode.Q) && !playerState.spellCooldown){
            if (cell.revealed){
                playerState.spellCooldown = false;
                return;
            }
            cell.revealed = true;
            state[cellPosition.x,cellPosition.y] = cell;
            board.Draw(state);    
            searchMineTime = defultSearchMineTime;
            playerState.spellCooldown = true;
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

    private void forceMonster(){
        Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.Tilemap.WorldToCell(WorldPosition);
        if (!playerState.gameOver && Input.GetKeyDown(KeyCode.E)){
            playerState.isValidMonsterMovement = true;
            Debug.Log("EEEEEE");
        }
    }

    public void glow(){
        Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.Tilemap.WorldToCell(WorldPosition);
        Cell cell = GetCell(cellPosition.x,cellPosition.y); 
        glowGrid.setCellPosition(cellPosition);
        glowGrid.glowGrid();
    }
    public void eraseGlow(){
        glowGrid.eraseGlowGrid();
    }
    public void backToMainMenu(){
        SceneManager.LoadSceneAsync(0);
    }

    public void gameOver(){     //記得沒有辦法找到Scene
        exitScene.SetActive(true);
    }
}
