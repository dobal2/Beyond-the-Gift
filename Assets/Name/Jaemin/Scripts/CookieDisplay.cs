
using TMPro;
using UnityEngine;

public class CookieDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    void Update()
    {
        text.text = GameManager.Instance.GetCookieCount().ToString("N0");
    }
}
