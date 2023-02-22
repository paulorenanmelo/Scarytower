using UnityEngine;

[CreateAssetMenu(fileName = "STRuntime", menuName = "ScaryTower/STRuntime", order = 1)]
public class STRuntime : ScriptableObject
{
    public float gameSpeed;
    public bool step = false;
    public float SHIFT2;
    public double stopwatch; //works as a clock, the variable 't' (time) in the formula
    public STState state = STState.Playing;
}
