using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sticker : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Camera mainCamera;
    private Transform originalParent;
    private Vector2 originalAnchoredPosition;
    private GraphicRaycaster graphicRaycaster;

    private RectTransform rectTransform;

    private Canvas canvas;

    public bool isAlreadyBatched = false;

    public bool isCompleted = false;

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
        if(isCompleted || isAlreadyBatched)
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
        if(isCompleted)
            return;

        if (isAlreadyBatched == false)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Gift"))
                {
                    rectTransform.SetParent(result.gameObject.transform);
                    GameObject newObj = Instantiate(gameObject, transform.position, Quaternion.identity);
                    newObj.transform.parent = result.gameObject.transform;
                    newObj.transform.localScale = Vector3.one;
                    newObj.GetComponent<Sticker>().isAlreadyBatched = true;
                }
            }

            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalAnchoredPosition;
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
