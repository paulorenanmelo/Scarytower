using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class STButtonPowerup : MonoBehaviour
{
    Button btn;
    [SerializeField] Inventory.Powerup powerup;
    
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