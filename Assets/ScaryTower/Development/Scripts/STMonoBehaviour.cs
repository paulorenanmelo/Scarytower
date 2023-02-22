using System;
using UnityEngine;

/// <summary>
/// This is a mono behaviour managed by <see cref="GameManager"/>. ST = Scary Tower
/// </summary>
public abstract class STMonoBehaviour : MonoBehaviour
{
    public delegate void AspectRatioChangedDelegate();
    public static AspectRatioChangedDelegate aspectRatioChanged;
    protected bool verbose => GameConfigManager.Instance.gameSettings.logVerbose;
    protected STRuntime Runtime => GameConfigManager.Instance.gameRuntime;
    protected STSettings Settings => GameConfigManager.Instance.gameSettings;

    /// <summary>
    /// Use this to run logic that should only happen whan the aspect ratio is changed.<br></br>
    /// This method requires calling base.<see cref="Init"/>
    /// </summary>
    public virtual void AspectRatioChanged()
    {
        
    }

    /// <summary>
    /// Use this instead of Start so it can be called in the correct order of execution by <see cref="GameManager"/>. ST = Scary Tower<br></br>
    /// Call base.Init(); in order for the <see cref="AspectRatioChanged"/> method to be called automatically.
    /// </summary>
    public virtual void Init()
    {
        aspectRatioChanged += AspectRatioChanged;
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
        aspectRatioChanged -= AspectRatioChanged;
    }
}