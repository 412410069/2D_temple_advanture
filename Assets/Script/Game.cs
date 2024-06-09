using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

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
    public WallCollider wall;
    public GameObject player;
    public MainMenu mainMenu;
    public GameOverLogic gameOverLogic; //因為在一開始時他不是開的，我們沒辦法用程式找到他，一定要在inspector中拉進去
    public ItemShield shield;//
    public GlowGrid glowGrid;
    public PlayerMeetMineLogic playerMeetMineLogic;
    public float shieldOpenTime;//
    public float defultShieldOpenTime = 3;//
    public float shieldTimer;//
    public float secondRate = 1;//
    public float searchMineTimer;
    public float searchMineTime;
    public float defultSearchMineTime = 10; 
    public float glowsecond = 0;

    private void Awake(){
        board = GetComponentInChildren<Board>();
        walkerGeneration = GetComponent<WalkerGeneration>();
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        player = GameObject.FindGameObjectWithTag("Player");
        monster = GameObject.FindGameObjectWithTag("monster").GetComponent<MonsterInitial>();
        glowGrid = GetComponentInChildren<GlowGrid>();
        shield = GameObject.FindGameObjectWithTag("Player").gameObject.transform.GetChild(0).gameObject.GetComponent<ItemShield>();
        wall = GameObject.FindGameObjectWithTag("grid").GetComponent<WallCollider>();
        playerMeetMineLogic = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMeetMineLogic>();
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
        wall.WallColliderInitial();
        monster.GenerateMonster1();
        monster.GenerateMonsterWithView();
        GenerateArtifact();
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

    private void GenerateArtifact(){
        bool spawned = false;
        while(!spawned){
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            
            if(state[x, y].type == Cell.Type.Empty){
                Vector3Int pos = new(x, y, 0);
                GetComponent<ArtifactList>().InstantiateArtifact(pos);
                spawned = true;
            }
        }
    }   

    void Update()
    {
        glowGrid.eraseGlow();
        Reavel();
        playerMeetMineLogic.PlayerMeetMine(playerState, state);
        // PlayerMeetMonster();
        shield.itemShield(playerState);
        isValidSearchMine();
        forceMonster();
        glowGrid.glow(board.Tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
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
        if (!playerState.gameOver && searchMineTime <= 0 && !playerState.spellCooldown){
            searchMine();
        }
        // Debug.Log(searchMineTime);
    }

    private void searchMine(){
        UnityEngine.Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.Tilemap.WorldToCell(WorldPosition);
        Cell cell = GetCell(cellPosition.x,cellPosition.y); 
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

    public Cell GetCell(int x, int y){
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
        UnityEngine.Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.Tilemap.WorldToCell(WorldPosition);
        if (!playerState.gameOver && Input.GetKeyDown(KeyCode.E)){
            playerState.isValidMonsterMovement = true;
            Debug.Log("EEEEEE");
        }
    }
}
    
