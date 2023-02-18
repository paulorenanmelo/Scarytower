using System.Collections;
using System.Collections.Generic;
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

    private void PerformedPrimaryTouch(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Waiting ||
            ctx.phase == InputActionPhase.Started ||
            ctx.phase == InputActionPhase.Performed)
            return;
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
        if (OnStartTouch != null) OnStartTouch(PrimaryPosition(), (float)ctx.startTime);
    }
    private void EndPrimaryTouch(InputAction.CallbackContext ctx)
    {
        if (OnEndTouch != null) OnEndTouch(PrimaryPosition(), (float)ctx.time);
    }

    public Vector2 PrimaryPosition()
    {
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
