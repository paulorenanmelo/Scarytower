using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Intro : MonoBehaviour
{
    [SerializeField] IntroElement[] element;
    [SerializeField] float fadeDuration;

    private void OnEnable()
    {
        Reset();
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < element.Length; i++)
        {
            seq.AppendInterval(element[i].delay);
            seq.Append(element[i].cg.DOFade(0f, fadeDuration));
            if(i < element.Length - 1)
            {
                seq.Append(element[i + 1].cg.DOFade(1f, fadeDuration));
            }
        }
        seq.OnComplete(OnFinishedSequence);
    }

    private void OnFinishedSequence()
    {
        PlayerPrefs.SetInt("Intro", 0);
    }

    private void OnDisable()
    {
        Reset();
    }

    private void Reset()
    {
        for (int i = 0; i < element.Length; i++)
        {
            if (i == 0)
                element[i].cg.alpha = 1f;
            else
                element[i].cg.alpha = 0f;
        }
    }
}

[Serializable]
public class IntroElement
{
    public CanvasGroup cg;
    public float delay;
}