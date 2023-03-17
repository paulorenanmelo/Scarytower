using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class STButton : MonoBehaviour
{
    Button btn;
    [SerializeField] STState state = STState.None;

    private void OnEnable()
    {
        if(!btn) btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GameConfigManager.Instance.gameRuntime.goToState = state;
    }
}
