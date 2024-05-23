using Unity.VisualScripting;
using UnityEngine;

public class MonsterInitial : MonoBehaviour
{
    public Game game;
    public GameObject monster1;
    public GameObject player; 
    public int numberOfClones = 5;
    private Vector2 position;
    private Vector2 position_prefab;
    void Awake(){
        game = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>();
    }

    public void GenerateMonster1()
    {
        for (int i = 0; i < numberOfClones-1;)
        {
            float x = Random.Range(0, game.width);
            float y = Random.Range(0, game.height);

            if(game.state[(int)x, (int)y].type != Cell.Type.Void && game.state[(int)x, (int)y].type != Cell.Type.Wall){
                position = new Vector2((int)x, (int)y); 
                GameObject clone = Instantiate(monster1, position, Quaternion.identity);
                ++i;
            }
        }
        
        while(true){
            float x_prefab = Random.Range(0, game.width);
            float y_prefab = Random.Range(0, game.height);

            if(game.state[(int)x_prefab, (int)y_prefab].type != Cell.Type.Void || game.state[(int)x_prefab, (int)y_prefab].type != Cell.Type.Wall){
                position_prefab = new Vector2((int)x_prefab, (int)y_prefab);
                monster1.transform.position=position_prefab;
                break;
            }
        }
    }
}

