
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public  class GiftDragger : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    public Order order;

    private Camera mainCamera;
    private Transform originalParent;
    public Vector2 originalAnchoredPosition;
    private GraphicRaycaster graphicRaycaster;

    private RectTransform rectTransform;
    private Canvas canvas;

    public bool isInSled;
    
    void Start()
    {
        canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
        mainCamera = Camera.main;
        originalParent = transform.parent;

        rectTransform = GetComponent<RectTransform>();
        
        originalAnchoredPosition = rectTransform.anchoredPosition;
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(GameManager.Instance.giftReinitializer.originalGift.GetComponent<GiftDoubleClick>().IsGiftTransparent())
            return;
        if(isInSled)
            return;
        
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, 
                eventData.position, 
                mainCamera, 
                out worldPoint))
        {
            rectTransform.position = worldPoint;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(GameManager.Instance.giftReinitializer.originalGift.GetComponent<GiftDoubleClick>().IsGiftTransparent())
            return;
        if(isInSled)
            return;
        
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);

        foreach (RaycastResult result in results)
        {
            
            RectTransform sledRectTransform = result.gameObject.GetComponent<RectTransform>();
            
            if (result.gameObject.CompareTag("Sled"))
            {
                isInSled = true;
                rectTransform.anchoredPosition = result.gameObject.transform.Find("SledSetPos").GetComponent<RectTransform>().anchoredPosition;
                order.CheckGift();
                return;
            }
        }

        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalAnchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
    
    
}