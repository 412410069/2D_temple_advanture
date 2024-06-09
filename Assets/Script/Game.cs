//處理有關遊戲初始化以及遊戲更新
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
    public Skill_Q skill_Q;
    public TileRevealLogic tileRevealLogic;
    
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
        skill_Q = GameObject.FindGameObjectWithTag("Player").GetComponent<Skill_Q>();
        tileRevealLogic = GetComponentInParent<TileRevealLogic>();
    }

    private void Start(){
        NewGame();
    }

    private void NewGame(){
        state = new Cell[width, height];    //state 紀錄的是地圖上所有地磚的狀態
        
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
        tileRevealLogic.Reavel(playerState, state, board);
        playerMeetMineLogic.PlayerMeetMine(playerState, state);
        // PlayerMeetMonster();
        shield.itemShield(playerState);
        skill_Q.isValidSearchMine(playerState, board, GameObject.Find("Grid").GetComponent<Game>(), state);
        forceMonster();
        glowGrid.glow(board.Tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)), state);
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
    
