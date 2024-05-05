using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{   
    public enum Facing{
        Up,
        Down,
        Left,
        Right
    }
    public Facing facing;
    public Vector3Int position;
    public bool isMoving;
}