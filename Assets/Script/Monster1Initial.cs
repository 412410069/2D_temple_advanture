using UnityEngine;

public class Monster1Initial : MonoBehaviour
{
    public Game Game;
    public GameObject monster1; 
    public int numberOfClones = 5;

    void Awake(){
        Game = GameObject.FindGameObjectWithTag("grid").GetComponent<Game>();
    }

    public void GenerateMonster1()
    {
        for (int i = 0; i < numberOfClones; i++)
        {
            float x = Random.Range(8, 23);
            float y = Random.Range(8, 23);

            Vector2 position = new Vector2((int)x, (int)y); 
            GameObject clone = Instantiate(monster1, position, Quaternion.identity);
        }
    }
}

