using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifacctBehavious : MonoBehaviour
{
    private Game game;
    private CircleCollider2D collision;
    public GameObject artifact;
    void Awake(){
        game = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>();
        collision = GetComponent<CircleCollider2D>();
    }

    void Update(){
        IsReavel();
    }

    private void IsReavel(){
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        if(game.state[x, y].revealed){
            artifact.SetActive(true);
        }
        else{
            artifact.SetActive(false);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision2D){
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
