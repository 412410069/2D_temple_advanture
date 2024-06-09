//取得地圖地磚的資訊
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCellLogic : MonoBehaviour
{
    private int width;
    private int height;

    public void Awake(){
        width = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>().width;
        height = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>().height;
    }
    public Cell GetCell(int x, int y, Cell[, ] state){
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
}
