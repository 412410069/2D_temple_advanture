using UnityEngine;

public class Monster1Initial : MonoBehaviour
{
    public Game Game;
    public GameObject monster1; 
    public int numberOfClones = 5;

    void Awake(){
        Game = GetComponent<Game>();
    }

    void GenerateMonster1()
    {
        for (int i = 0; i < numberOfClones; i++)
        {
            float x = (int)Random.Range(0, Game.width);
            float y = (int)Random.Range(0, Game.height);



            Vector2 position = new Vector2(x, y); 
            GameObject clone = Instantiate(monster1, position, Quaternion.identity);

            
        }
    }
}

