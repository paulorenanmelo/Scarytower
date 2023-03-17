using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class STStateMachine : STMonoBehaviour
{
    [SerializeField] STStateMachineElement[] elements;
    [SerializeField] float fadeDuration;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Runtime.state++;
        }
        if(Runtime.goToState != STState.None)
        {
            if(Runtime.goToState == Runtime.state)
            {
                Runtime.goToState = STState.None;
                return;
            }
            Exit(Runtime.state);
            Enter(Runtime.goToState);
            Runtime.state = Runtime.goToState;
            Runtime.goToState = STState.None;
        }
        //_state = Runtime.state;
    }

    private void Exit(STState state)
    {
        var found = elements.FirstOrDefault(e => e.state == state);
        if (found != null)
        {
            for (int i = 0; i < found.elements.Length; i++)
            {
                var cg = found.elements[i].GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.DOFade(0, fadeDuration);
                }
                else
                {
                    found.elements[i].SetActive(false);
                }
            }
        }
    }

    private void Enter(STState state)
    {
        var found = elements.FirstOrDefault(e => e.state == state);
        if (found != null)
        {
            for (int i = 0; i < found.elements.Length; i++)
            {
                var cg = found.elements[i].GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.DOFade(1, fadeDuration);
                }
                else
                {
                    found.elements[i].SetActive(true);
                }
            }
        }
        // tapped play in menu
        if(state == STState.Playing && Runtime.state != STState.Paused)
        {
            SceneManager.LoadScene("Game");
        }
        // go to menu from ingame
        else if (state == STState.StartMenu && (Runtime.state == STState.Playing || Runtime.state == STState.Paused))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}

[Serializable]
public class STStateMachineElement
{
    public STState state;
    public GameObject[] elements;
}