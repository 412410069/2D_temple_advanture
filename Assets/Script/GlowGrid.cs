using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GlowGrid : MonoBehaviour
{
    private int width = 32;
    private int height = 32;
    private int cellPositionX;
    private int cellPositionY;
    public Tilemap Tilemap {get; private set;}
    private Board board;

    public Tile Grid;
    public Tile Grid_WhenMouseDown;
    
    private void Awake(){
        Tilemap = GetComponent<Tilemap>();
        board = GetComponentInChildren<Board>();
    }

    public void setCellPosition(Vector3Int cellPosition){
        if(cellPosition.x < width && cellPosition.y < height){
            cellPositionX = cellPosition.x;
            cellPositionY = cellPosition.y;
        }
    }
    public void glowGrid(){
        if(Input.GetMouseButton(0) == true){
            Tilemap.SetTile(new Vector3Int(cellPositionX, cellPositionY, 1), Grid_WhenMouseDown);
        }
        else{
            Tilemap.SetTile(new Vector3Int(cellPositionX, cellPositionY, 1), Grid);
        }
    }
    public void eraseGlowGrid(){
        Tilemap.SetTile(new Vector3Int(cellPositionX, cellPositionY, 1), null);
    }
    // public void OnMouseOver(){
    //     Tilemap.SetTile(new Vector3Int(cellPositionX, cellPositionY, 1), Grid);
    // }
    // public void OnMouseExit(){
    //     Tilemap.SetTile(new Vector3Int(cellPositionX, cellPositionY, 1), null);
    // }
    
}
