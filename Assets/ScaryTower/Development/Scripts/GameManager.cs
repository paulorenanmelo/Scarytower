using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.Profiling;
using UnityEditorInternal.VersionControl;
using UnityEngine;

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
        // todo: load info using a neat boostrap plugin or equivalent
        cameraAspectRatio.Init();
        logType = Debug.unityLogger.filterLogType;
        Debug.unityLogger.filterLogType = GameConfigManager.Instance.gameSettings.logType;
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
    }
}

/// <summary>
/// This is a mono behaviour managed by <see cref="GameManager"/>. ST = Scary Tower
/// </summary>
public abstract class STMonoBehaviour : MonoBehaviour
{
    /// <summary>
    /// Use this instead of Start so it can be called in the correct order of execution by <see cref="GameManager"/>. ST = Scary Tower
    /// </summary>
    public virtual void Init()
    {

    }

    /// <summary>
    /// Use this instead of Update so it can be called in the correct order of execution by <see cref="GameManager"/>. ST = Scary Tower
    /// </summary>
    public virtual void Tick()
    {

    }

    /// <summary>
    /// Use this instead of OnDestroy so it can be called in the correct order of execution by <see cref="GameManager"/>. ST = Scary Tower
    /// </summary>
    public virtual void Discard()
    {

    }
}