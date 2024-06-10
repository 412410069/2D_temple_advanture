using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterDeathEffect : MonoBehaviour
{   
    public GameObject DeathParticle;
    
    public void DeathEffect(int n){
        for(int i = 0; i < n; i++){
            GameObject Bacon = Instantiate(DeathParticle, transform.position + new Vector3(.5f, .5f, 0), Quaternion.identity);
        }
    }
}
