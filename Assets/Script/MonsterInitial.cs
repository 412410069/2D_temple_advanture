//生成怪物
using Unity.VisualScripting;
using UnityEngine;

public class MonsterInitial : MonoBehaviour
{
    public Game game;
    public GameObject monster1;
    public GameObject monsterWithView;
    public GameObject player; 
    public int numberOfMonster1Clones = 5;
    public int numberOfMonsterWithViewClones = 5;
    private Vector2 position;
    private Vector2 position_prefab;
    public int initialSafeDistance = 10;

    void Awake(){
        game = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>();
    }

    public void GenerateMonster1()
    {
        Vector2 player_position = player.transform.position;
        float player_x = player_position.x;
        float player_y = player_position.y;

        for (int i = 0; i < numberOfMonster1Clones-1;)
        {
            float x = Random.Range(0, game.width);
            float y = Random.Range(0, game.height);

            if(game.state[(int)x, (int)y].type != Cell.Type.Void && game.state[(int)x, (int)y].type != Cell.Type.Wall){
                if(Mathf.Abs(player_x - (int)x) + Mathf.Abs(player_y - (int)y) >= initialSafeDistance){
                    position = new Vector3((int)x, (int)y, 0); 
                    GameObject clone = Instantiate(monster1, position, Quaternion.identity);
                    ++i;
                }
            }
        }
        
        while(true){
            float x_prefab = Random.Range(0, game.width);
            float y_prefab = Random.Range(0, game.height);

            if(game.state[(int)x_prefab, (int)y_prefab].type != Cell.Type.Void && game.state[(int)x_prefab, (int)y_prefab].type != Cell.Type.Wall){
                if(Mathf.Abs(player_x - (int)x_prefab) + Mathf.Abs(player_y - (int)y_prefab) >= initialSafeDistance){
                    position_prefab = new Vector3((int)x_prefab, (int)y_prefab, 0);
                    monster1.transform.position=position_prefab;
                    break;
                }
            }
        }
    }

    public void GenerateMonsterWithView()
    {
        Vector2 player_position = player.transform.position;
        float player_x = player_position.x;
        float player_y = player_position.y;

        for (int i = 0; i < numberOfMonsterWithViewClones-1;)
        {
            float x = Random.Range(0, game.width);
            float y = Random.Range(0, game.height);

            if(game.state[(int)x, (int)y].type != Cell.Type.Void && game.state[(int)x, (int)y].type != Cell.Type.Wall){
                if(Mathf.Abs(player_x - (int)x) + Mathf.Abs(player_y - (int)y) >= initialSafeDistance){
                    position = new Vector3((int)x, (int)y, 0); 
                    GameObject clone = Instantiate(monsterWithView, position, Quaternion.identity);
                    ++i;
                }
            }
        }
        
        while(true){
            float x_prefab = Random.Range(0, game.width);
            float y_prefab = Random.Range(0, game.height);

            if(game.state[(int)x_prefab, (int)y_prefab].type != Cell.Type.Void && game.state[(int)x_prefab, (int)y_prefab].type != Cell.Type.Wall){
                if(Mathf.Abs(player_x - (int)x_prefab) + Mathf.Abs(player_y - (int)y_prefab) >= initialSafeDistance){
                    position_prefab = new Vector2((int)x_prefab, (int)y_prefab);
                    monsterWithView.transform.position=position_prefab;
                    break;
                }
            }
        }
    }
}

