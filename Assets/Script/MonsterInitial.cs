using UnityEngine;

public class MonsterInitial : MonoBehaviour
{
    public Game Game;
    public GameObject monster1; 
    public int numberOfClones = 5;

    void Awake(){
        Game = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>();
    }

    public void GenerateMonster1()
    {
        for (int i = 0; i < numberOfClones-1; i++)
        {
            float x = Random.Range(8, 24);
            float y = Random.Range(8, 24);

            Vector2 position = new Vector2((int)x, (int)y); 
            GameObject clone = Instantiate(monster1, position, Quaternion.identity);
        }
        
        float x_prefab = Random.Range(8, 24);
        float y_prefab = Random.Range(8, 24);

        Vector2 position_prefab = new Vector2((int)x_prefab, (int)y_prefab);

        monster1.transform.position=position_prefab;
    }
}

