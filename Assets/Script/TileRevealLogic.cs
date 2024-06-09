using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRevealLogic : MonoBehaviour
{
    public void Reavel(PlayerState playerState, Cell[, ] state, Board board){
        int x = playerState.position.x;
        int y = playerState.position.y;
        Cell cell = state[x, y];
        
        if(cell.type == Cell.Type.Empty){
            Flood(cell, state);
        }
        //hey hey
        cell.revealed = true;
        state[x, y] = cell;
        board.Draw(state);
    }


    private void Flood(Cell cell, Cell[, ] state){
        if(cell.revealed) return;
        if(cell.type == Cell.Type.Mine || cell.type == Cell.Type.Wall) return;

        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        if(cell.type == Cell.Type.Empty){
            Flood(state[cell.position.x + 1, cell.position.y], state);
            Flood(state[cell.position.x + 1, cell.position.y + 1], state);
            Flood(state[cell.position.x, cell.position.y + 1], state);
            Flood(state[cell.position.x - 1, cell.position.y + 1], state);
            Flood(state[cell.position.x - 1, cell.position.y], state);
            Flood(state[cell.position.x - 1, cell.position.y - 1], state);
            Flood(state[cell.position.x, cell.position.y - 1], state);
            Flood(state[cell.position.x + 1, cell.position.y - 1], state);
        }
    }
}
