using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

// Unmanaged systems based on ISystem can be Burst compiled, but this is not yet the default.
// So we have to explicitly opt into Burst compilation with the [BurstCompile] attribute.
// It has to be added on BOTH the struct AND the OnCreate/OnDestroy/OnUpdate functions to be
// effective.
[BurstCompile]
partial struct STPlayerSystem : ISystem
{
    private PlayerScript player;

    public PlayerScript.Character character;
    public Elevator.Type elevator;
    private bool isPlaying;

    // Every function defined by ISystem has to be implemented even if empty.
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        player = new PlayerScript();

        //assign which character/elevator is selected
        player.character = character; //Player.Character.Mom;
        player.elevator.type = elevator; //Elevator.Type.Wood;
                                         //TODO: read playerprefs

        //Load the correct images from the folder
        LoadSprite();

        // animator has a default state which is playing on awake
        isPlaying = true;
    }

    // Every function defined by ISystem has to be implemented even if empty.
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    // See note above regarding the [BurstCompile] attribute.
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!state.Enabled) return;
    }

    private void LoadSprite()
    {

    }

    private enum PlayerStatus { Paused, Animated }
    private void SetStatus(PlayerStatus status)
    {
        switch (status)
        {
            case PlayerStatus.Paused:
                SetEnabled(false);
                break;
            case PlayerStatus.Animated:
                SetEnabled(true);
                break;
            default:
                break;
        }
    }

    private void SetEnabled(bool enabled)
    {
        //anim.enabled = enabled;
        //horizontalMove.enabled = enabled;
        //idleMove.enabled = enabled;
        isPlaying = enabled;
    }
}