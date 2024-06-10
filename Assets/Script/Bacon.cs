using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Bacon : MonoBehaviour
{   
    void Start(){
        float popForce = 10f;
        Vector2 popDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f));
        GetComponent<Rigidbody2D>().AddForce(popDirection * popForce, ForceMode2D.Impulse);
        Destroy(gameObject, 2f);
        Debug.Log("Bacon!!!!");
    }
}
