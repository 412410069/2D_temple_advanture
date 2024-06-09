//配合skills，方便玩家查看現在的滑鼠指到的是哪一個地磚
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GlowGrid : MonoBehaviour
{
    private int width;
    private int height;
    private int cellPositionX;
    private int cellPositionY;
    public Tilemap Tilemap {get; private set;}
    private Board board;
    private Game game;
    private GetCellLogic getCellLogic;

    public Tile Grid_whenMouseOver;
    public Tile Grid_WhenMouseDown;

    
    
    private void Awake(){
        Tilemap = GetComponent<Tilemap>();
        board = GetComponentInChildren<Board>();
        game = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>();
        getCellLogic = GameObject.FindGameObjectWithTag("grid").GetComponent<GetCellLogic>();
        width = game.width;
        height = game.height;
    }


    public void setCellPosition(Vector3Int cellPosition, Cell[, ] state){
        if(cellPosition.x < width && cellPosition.y < height){
            cellPositionX = cellPosition.x;
            cellPositionY = cellPosition.y;
        }
    }

    public void glow(Vector3Int cellPosition, Cell[, ] state){
        // Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Vector3Int cellPosition = board.Tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Cell cell = getCellLogic.GetCell(cellPosition.x,cellPosition.y, state); 
        setCellPosition(cellPosition, state);
        if(Input.GetMouseButton(0) == true){
            Tilemap.SetTile(new Vector3Int(cellPositionX, cellPositionY, 1), Grid_WhenMouseDown);
        }
        else{
            Tilemap.SetTile(new Vector3Int(cellPositionX, cellPositionY, 1), Grid_whenMouseOver);
        }
    }
    
    public void eraseGlow(){
        Tilemap.SetTile(new Vector3Int(cellPositionX, cellPositionY, 1), null);
    }

    // public void OnMouseOver(){
    //     Tilemap.SetTile(new Vector3Int(cellPositionX, cellPositionY, 1), Grid);
    // }
    // public void OnMouseExit(){
    //     Tilemap.SetTile(new Vector3Int(cellPositionX, cellPositionY, 1), null);
    // }
    
}
