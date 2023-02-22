using System;
using UnityEngine;

[Serializable]
public class STSettings
{
    public float startingSpeed = 0.5f;
    public float snapDistance = 0.0333f;
    public float minDragToChangeTrack = 0.0333f;
    public LogType logType = LogType.Log;
    public bool logVerbose = false;
}

[Serializable]
public class GameConfigManager : Singleton<GameConfigManager>
{
    public STSettings gameSettings;
    public STRuntime gameRuntime;
}
