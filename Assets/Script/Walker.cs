using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WalkerObject
{
    public Vector3Int position;
    public Vector3Int direction;
    public float chanceToChange;

    public WalkerObject(Vector3Int pos, Vector3Int dir, float chance){
        position = pos;
        direction = dir;
        chanceToChange = chance;
    }
}
