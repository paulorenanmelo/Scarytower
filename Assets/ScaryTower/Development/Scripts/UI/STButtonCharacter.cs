using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class STButtonCharacter : MonoBehaviour
{
    Button btn;
    [SerializeField] PlayerScript.Character character;
    
    private void OnEnable()
    {
        if (!btn) btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        //todo
    }
}
