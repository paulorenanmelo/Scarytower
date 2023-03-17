using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class STButtonHyperlink : MonoBehaviour
{
    Button btn;
    [SerializeField] string Link;

    private void OnEnable()
    {
        if (!btn) btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Application.OpenURL(Link);
    }
}
