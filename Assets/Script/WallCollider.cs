using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    public Game game;
    public GameObject wall;
    private Vector2 position;

    void Awake(){
        game = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>();
    }

    public void WallColliderInitial(){
        for(int x = 0; x < game.width; x++){
            for(int y = 0; y < game.height; y++){
                if(game.state[x, y].type == Cell.Type.Wall){
                    position = new Vector3(x, y, 0); 
                    GameObject clone = Instantiate(wall, position, Quaternion.identity);
                }
            }
        }
    }
}
