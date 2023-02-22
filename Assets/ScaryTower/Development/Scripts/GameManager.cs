using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
//using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public enum STState
{
    None = -1,
    Loading = 0,
    StartMenu = 1,
    Playing = 2,
    Paused = 3,
    LiftMenu = 4,
    CharacterMenu = 5,
    CreditsMenu = 6,
    // highscore ?
}

public class GameManager : MonoBehaviour
{
    [SerializeField] STMonoBehaviour cameraAspectRatio;
    [SerializeField] STMonoBehaviour setPlayerPosFromCameraAspectRatio;
    [SerializeField] STMonoBehaviour player;
    [SerializeField] STMonoBehaviour bgManager;
    [SerializeField] STMonoBehaviour horizontalMove;

    //Game Movement Speed Two-step Formula
    private const float K = 0.2f;
    private const float AMPLITUDE = 0.7f; // amplitude of each step
    private const float B = 200f;
    private const float E = 1.15f;//2.718f;//Number E (Euler's)
    private const float SHIFT = 0.5f; // starting speed
    private const float STEP = 0.999999f;
    private bool step = false;
    private float SHIFT2;
    //private int counter = 0;//work as a clock, the variable 't' in the formula
    private double stopwatch = 0;//work as a clock, the variable 't' in the formula

    private STState state = STState.Playing; // todo: UI Flow
    private LogType logType;

    void Start()
    {
        logType = Debug.unityLogger.filterLogType;
        Debug.unityLogger.filterLogType = GameConfigManager.Instance.gameSettings.logType;

        // todo: load info using a neat boostrap plugin or equivalent
        MainScript.gameSpeed = GameConfigManager.Instance.gameSettings.startingSpeed;
        stopwatch = 0;

        cameraAspectRatio.Init();
        //setPlayerPosFromCameraAspectRatio.Init();
        player.Init();
        bgManager.Init();
        //horizontalMove.Init();
    }

    void Update()
    {
        cameraAspectRatio.Tick();

        GameMovementSpeed();
    }

    private void GameMovementSpeed()
    {
        if (Time.timeScale > 0 && state == STState.Playing)
        {
            if (step)
            {
                MainScript.gameSpeed = (float)System.Math.Round(SHIFT2 + (AMPLITUDE / (1 + (B * Mathf.Pow((E), (-K * (float)stopwatch))))), 8);
                MainScript.enemySpacing = MainScript.gameSpeed;
            }
            else
            {
                MainScript.gameSpeed = (float)System.Math.Round(SHIFT + (AMPLITUDE / (1 + (B * Mathf.Pow((E), (-K * (float)stopwatch))))), 8);
                MainScript.enemySpacing = MainScript.gameSpeed;
            }

            //counter++;
            stopwatch += Time.deltaTime;
        }
        if (!step)
        {
            if (MainScript.gameSpeed > (SHIFT + AMPLITUDE) * STEP)
            {
                step = true;
                //counter = 0;
                stopwatch = 0;
                SHIFT2 = MainScript.gameSpeed;
            }
        }
        GameConfigManager.Instance.gameSettings.startingSpeed = MainScript.gameSpeed;
    }

    private void OnDestroy()
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