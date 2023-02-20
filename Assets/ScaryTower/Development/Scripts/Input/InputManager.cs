using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    public delegate void StartTouch(Vector2 pos, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 pos, float time);
    public event EndTouch OnEndTouch;

    private PlayerControls playerControls;
    private Camera cam;
    private bool primaryTouching = false;
    private InputActionPhase[] normalActions = new InputActionPhase[] { InputActionPhase.Started, InputActionPhase.Waiting, InputActionPhase.Performed };
    private bool verbose => GameConfigManager.Instance.gameSettings.logVerbose;

    public override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
        cam = Camera.main;
    }
    private void Start()
    {
        playerControls.Touch.PrimaryContact.started += StartPrimaryTouch;
        playerControls.Touch.PrimaryContact.canceled += EndPrimaryTouch;
        playerControls.Touch.PrimaryContact.performed += PerformedPrimaryTouch;
    }

    private void Update()
    {
        if (primaryTouching)
        {
            if (normalActions.Contains(playerControls.Touch.PrimaryContact.phase)) return;
            if (verbose) Debug.Log(playerControls.Touch.PrimaryContact.phase);
            EndPrimaryTouch(Time.time);
        }
    }

    private void PerformedPrimaryTouch(InputAction.CallbackContext ctx)
    {
        if (normalActions.Contains(ctx.phase)) return;
        if (verbose) Debug.Log("[InputManager]: " + ctx.phase);
        EndPrimaryTouch(ctx);
    }

    private void OnDestroy()
    {
        if (playerControls == null) return;
        if (playerControls.Touch.PrimaryContact == null) return;
        playerControls.Touch.PrimaryContact.started -= StartPrimaryTouch;
        playerControls.Touch.PrimaryContact.canceled -= EndPrimaryTouch;
        playerControls.Touch.PrimaryContact.performed -= PerformedPrimaryTouch;
    }

    private void StartPrimaryTouch(InputAction.CallbackContext ctx)
    {
        primaryTouching = true;
        if (OnStartTouch != null) OnStartTouch(PrimaryPosition(), (float)ctx.startTime);
    }
    private void EndPrimaryTouch(InputAction.CallbackContext ctx)
    {
        EndPrimaryTouch((float)ctx.time);
    }
    private void EndPrimaryTouch(float time)
    {
        primaryTouching = false;
        if (OnEndTouch != null) OnEndTouch(PrimaryPosition(), time);
    }

    public Vector2 PrimaryPosition()
    {
        //if(verbose) Debug.Log("[InputManager]: PrimaryPosition ");
        return Vector2Utils.ScreenToWorld(cam, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
}
