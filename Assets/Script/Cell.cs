//地磚的性質
using UnityEngine;

public class Cell
{
    public enum Type{
        Empty,
        Mine,
        Number,
        Wall,
        Void,
        Exit,
        Exploded
    }

    public Vector3Int position;
    public Type type;
    public int number;
    public bool revealed;
    public bool flagged;
    public bool exploded;
}
