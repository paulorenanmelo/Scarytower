using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.Profiling;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] STMonoBehaviour cameraAspectRatio;
    [SerializeField] STMonoBehaviour setPlayerPosFromCameraAspectRatio;
    [SerializeField] STMonoBehaviour player;
    [SerializeField] STMonoBehaviour bgManager;
    [SerializeField] STMonoBehaviour horizontalMove;
    
    private LogType logType;
    // Start is called before the first frame update
    void Start()
    {
        logType = Debug.unityLogger.filterLogType;
        Debug.unityLogger.filterLogType = GameConfigManager.Instance.gameSettings.logType;
        
        // todo: load info using a neat boostrap plugin or equivalent
        
        cameraAspectRatio.Init();
        //setPlayerPosFromCameraAspectRatio.Init();
        player.Init();
        bgManager.Init();
        //horizontalMove.Init();
    }

    // Update is called once per frame
    void Update()
    {
        cameraAspectRatio.Tick();
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