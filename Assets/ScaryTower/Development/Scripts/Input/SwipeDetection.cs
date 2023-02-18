using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float MinDist = 0.2f;
    [SerializeField] private float MaxTime = 1.0f;
    [SerializeField, Range(0, 1)] private float DirThreshold = 0.9f;

    private Vector2 startPos;
    private Vector2 endPos;
    private float startTime;
    private float endTime;

    private void OnEnable()
    {
        InputManager.Instance.OnStartTouch += OnStartTouch;
        InputManager.Instance.OnEndTouch += OnEndTouch;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnStartTouch -= OnStartTouch;
        InputManager.Instance.OnEndTouch -= OnEndTouch;
    }

    private void OnStartTouch(Vector2 pos, float time)
    {
        startPos = pos;
        startTime = time;
    }
    private void OnEndTouch(Vector2 pos, float time)
    {
        endPos = pos;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if(Vector3.Distance(endPos, startPos) > MinDist
            && endTime - startTime <= MaxTime )
        {
            Debug.Log("Swiped");
            Debug.DrawLine(startPos, endPos, Color.red, 3f);
            Vector3 dir = endPos - startPos;
            Vector2 dir2D = new Vector3(dir.x, dir.y).normalized;
            SwipeDirection(dir2D);
        }
    }

    private void SwipeDirection(Vector2 direction) {
        if(Vector2.Dot(Vector2.right, direction) > DirThreshold)
        {
            Debug.Log("Swipe right");
        }
        else if(Vector2.Dot(Vector2.left, direction) > DirThreshold)
        {
            Debug.Log("Swipe left");
        }
        else if(Vector2.Dot(Vector2.down, direction) > DirThreshold)
        {
            Debug.Log("Swipe down");
        }
        else if(Vector2.Dot(Vector2.up, direction) > DirThreshold)
        {
            Debug.Log("Swipe up");
        }
    }
}
