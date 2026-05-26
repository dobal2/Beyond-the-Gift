
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutlineTriggerEvent : MonoBehaviour
{
    Image image;
    EventTrigger eventTrigger;
    Color defalutColor;

    [SerializeField] Material material;
    [SerializeField] Material material2;

    private void Awake()
    {
        image = GetComponent<Image>();
        eventTrigger = GetComponent<EventTrigger>();

        EventTrigger.Entry enterentry = new EventTrigger.Entry();
        enterentry.eventID = EventTriggerType.PointerEnter;
        enterentry.callback.AddListener((data) => { Outline(); });

        EventTrigger.Entry exitentry = new EventTrigger.Entry();
        exitentry.eventID = EventTriggerType.PointerExit;
        exitentry.callback.AddListener((data) => { NoOutline(); });

        eventTrigger.triggers.Add(enterentry);
        eventTrigger.triggers.Add(exitentry);
    }


    public void Outline()
    {
        defalutColor = image.color;
        Color color = image.color;
        color.a = 1f;
        image.color = color;
        image.material = material;
    }
    
    public void NoOutline()
    {
        Color color = defalutColor;
        image.color = color;
        image.material = material2;
    }
}
