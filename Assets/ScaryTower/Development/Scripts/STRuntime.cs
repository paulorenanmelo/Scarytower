using UnityEngine;

[CreateAssetMenu(fileName = "STRuntime", menuName = "ScaryTower/STRuntime", order = 1)]
public class STRuntime : ScriptableObject
{
    public float gameSpeed;
    public bool step = false;
    public float SHIFT2;
    public double stopwatch; //works as a clock, the variable 't' (time) in the formula
    /// <summary>
    /// Current game state
    /// </summary>
    public STState state = STState.Playing;
    /// <summary>
    /// Set this in order to transition to a different state, and set it to None when finished transitioning
    /// </summary>
    public STState goToState = STState.Playing;
}
