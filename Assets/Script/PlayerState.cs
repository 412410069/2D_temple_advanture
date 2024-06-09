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
    public bool meetMine;
    public bool meetMonster;
    public bool isShieldOpen = false;
    public bool gameOver;
    public bool isValidMonsterMovement;
    public bool spellCooldown;
}
