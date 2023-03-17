using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
//using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum STState
{
    None = -1,
    Loading = 0,
    Intro = 1,
    StartMenu = 2,
    Playing = 3,
    Paused = 4,
    ShoppingMenu = 5,
    LiftMenu = 6,
    CharacterMenu = 7,
    PowerupMenu = 8,
    MissionMenu = 9,
    CreditsMenu = 10,
    GameOver = 11
    // highscore ?
}

[DefaultExecutionOrder(-2)]
public class GameManager : STMonoBehaviour
{
    [SerializeField] STMonoBehaviour cameraAspectRatio;
    [SerializeField] STMonoBehaviour setPlayerPosFromCameraAspectRatio;
    [SerializeField] STMonoBehaviour player;
    [SerializeField] STMonoBehaviour bgManager;
    [SerializeField] STMonoBehaviour horizontalMove;

    // Game Movement Speed Two-step Formula
    // Visualized in https://www.geogebra.org/graphing/m647vgmh
    private const float K = 0.2f;
    private const float AMPLITUDE = 0.7f; // amplitude of each step
    private const float B = 200f;
    private const float E = 1.15f;//2.718f;//Number E (Euler's)
    private const float SHIFT = 0.5f; // starting speed
    private const float STEP = 0.999999f;

    private LogType logType;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        logType = Debug.unityLogger.filterLogType;
        Debug.unityLogger.filterLogType = Settings.logType;

        // todo: load info using a neat boostrap plugin or equivalent
        GameConfigManager.Instance.gameRuntime = Resources.Load<STRuntime>("Runtime/STRuntime");
        ResetRuntime();

        if (cameraAspectRatio) cameraAspectRatio.Init();
        //setPlayerPosFromCameraAspectRatio.Init();
        if(player) player.Init();
        if(bgManager) bgManager.Init();
        //horizontalMove.Init();
    }

    private void ResetRuntime()
    {
        Runtime.gameSpeed = Settings.startingSpeed;
        Runtime.stopwatch = 0;
        Runtime.step = false;
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            Runtime.state = STState.None;
            Runtime.goToState = STState.None;
            if (PlayerPrefs.HasKey("Intro") && PlayerPrefs.GetInt("Intro") == 0)
                Runtime.goToState = STState.StartMenu;
            else
                Runtime.goToState = STState.Intro;
        }
        else
        {
            var p = player as Player;
            p.character = Runtime.inventory.character;
            p.elevator = Runtime.inventory.elevator;
        }
    }

    void Update()
    {
        cameraAspectRatio.Tick();

        GameMovementSpeed();
    }

    private void GameMovementSpeed()
    {
        if (Time.timeScale > 0 && Runtime.state == STState.Playing)
        {
            if (Runtime.step) // second step
            {
                Runtime.gameSpeed = (float)System.Math.Round(Runtime.SHIFT2 + (AMPLITUDE / (1 + (B * Mathf.Pow((E), (-K * (float)Runtime.stopwatch))))), 8);
                //Runtime.enemySpacing = Runtime.gameSpeed;
            }
            else // first step
            {
                Runtime.gameSpeed = (float)System.Math.Round(SHIFT + (AMPLITUDE / (1 + (B * Mathf.Pow((E), (-K * (float)Runtime.stopwatch))))), 8);
                //Runtime.enemySpacing = Runtime.gameSpeed;
            }

            //counter++;
            Runtime.stopwatch += Time.deltaTime;
        }
        if (!Runtime.step)
        {
            if (Runtime.gameSpeed > (SHIFT + AMPLITUDE) * STEP)
            {
                Runtime.step = true;
                //counter = 0;
                Runtime.stopwatch = 0;
                Runtime.SHIFT2 = Runtime.gameSpeed;
            }
        }
    }

    public override void Discard()
    {
        // restore logtype set in the editor
        Debug.unityLogger.filterLogType = logType;

        cameraAspectRatio.Discard();
        //setPlayerPosFromCameraAspectRatio.Discard();
        player.Discard();
        bgManager.Discard();
        //horizontalMove.Discard();
    }
}