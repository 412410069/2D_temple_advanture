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

    public Tile Grid_whenMouseOver;
    public Tile Grid_WhenMouseDown;

    
    
    private void Awake(){
        Tilemap = GetComponent<Tilemap>();
        board = GetComponentInChildren<Board>();
        game = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>();
        width = game.width;
        height = game.height;
    }


    public void setCellPosition(Vector3Int cellPosition){
        if(cellPosition.x < width && cellPosition.y < height){
            cellPositionX = cellPosition.x;
            cellPositionY = cellPosition.y;
        }
    }

    public void glow(Vector3Int cellPosition){
        // Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Vector3Int cellPosition = board.Tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Cell cell = game.GetCell(cellPosition.x,cellPosition.y); 
        setCellPosition(cellPosition);
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
