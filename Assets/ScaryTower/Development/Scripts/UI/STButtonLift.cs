using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class STButtonLift : MonoBehaviour
{
    Button btn;
    [SerializeField] Elevator.Type type;

    private void OnEnable()
    {
        if (!btn) btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // lift purchased
        if(PlayerPrefs.HasKey(type.ToString()) && PlayerPrefs.GetInt(type.ToString()) == 1)
        {
            // equip
            GameConfigManager.Instance.gameRuntime.inventory.elevator = type;
        }
        // lift not purchased
        else
        {
            // try to buy - enough coins (execute purchase)
            // try to buy - not enough coins (offer purchasing non-expiring coins with money)
        }
    }
}
