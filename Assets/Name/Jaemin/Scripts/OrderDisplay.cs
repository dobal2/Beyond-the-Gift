using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class OrderDisplay : MonoBehaviour
{
    public OrderDisplayObject[] orderDisplayObjects;
    public Order order;

    [SerializeField] Sprite m_correctImage;
    [SerializeField] GameObject m_orderUI;
    [SerializeField] GameObject m_sledUI;
    [SerializeField] GameObject m_productGift;
    [SerializeField] private Sprite m_transparentImage;

    public Image m_productGiftImage;
    public Sprite paparSprite;
    private Vector2 originalAnchoredPosition;
    public MovingSled m_movingSled;
    RectTransform rectTransform;
    private bool startDisplay;

    private void Start()
    {
        rectTransform = m_productGift.GetComponent<RectTransform>();

        for (int i = 0; i < orderDisplayObjects.Length; i++)
        {
            orderDisplayObjects[i].wrapper = orderDisplayObjects[i].parent.transform.Find("Wrapper").GetComponent<Image>();
            orderDisplayObjects[i].gift = orderDisplayObjects[i].parent.transform.Find("Gift").GetComponent<Image>();
            orderDisplayObjects[i].candy = orderDisplayObjects[i].parent.transform.Find("Candy").GetComponent<Image>();
            orderDisplayObjects[i].paper = orderDisplayObjects[i].parent.transform.Find("Paper").GetComponent<Image>();
        }

        m_movingSled = m_sledUI.GetComponent<MovingSled>();
        StartCoroutine(WaitOrder());

        originalAnchoredPosition = m_productGift.transform.position;
        m_productGiftImage = m_productGift.GetComponent<Image>();
    }

    private void UpdateDisplay()
    {
        if (!startDisplay)
            return;

        for (int i = 0; i < orderDisplayObjects.Length; i++)
        {
            // Order 데이터만 갱신
            orderDisplayObjects[i].wrapper.sprite = order.m_wantedWrapper[i].icon;
            orderDisplayObjects[i].gift.sprite = order.m_wantedGift[i].icon;
            orderDisplayObjects[i].candy.sprite = order.m_wantedCandy[i].icon;
        }
    }

    public void ResetDisplay()
    {

        // UI 요소 다시 활성화 및 초기화
        foreach (var displayObject in orderDisplayObjects)
        {
            displayObject.wrapper.enabled = true;
            displayObject.gift.enabled = true;
            displayObject.candy.enabled = true;
            displayObject.paper.preserveAspect = true; // 종이 비율 유지
            displayObject.paper.sprite = paparSprite; // 초기화 스프라이트 설정
        }

        // 기타 UI 초기화
        m_productGiftImage.sprite = m_transparentImage;
    }


    private void Update()
    {
        UpdateDisplay();
    }

    IEnumerator WaitOrder()
    {
        while (order.m_wantedGift == null || order.m_wantedCandy == null || order.m_wantedWrapper == null)
            yield return null;

        startDisplay = true;
    }

    public void Check(Boolean pIsCorrect, Int32 pIndex)
    {
        GameManager.Instance.isSled = true;
        m_productGiftImage.sprite = m_transparentImage;

        if (pIsCorrect)
        {
            orderDisplayObjects[pIndex].paper.sprite = m_correctImage;
            orderDisplayObjects[pIndex].candy.enabled = false;
            orderDisplayObjects[pIndex].wrapper.enabled = false;
            orderDisplayObjects[pIndex].gift.enabled = false;
            GameManager.Instance.productView.ShowTextUI("성공");
        }
        else
        {
            GameManager.Instance.productView.ShowTextUI("틀림");
        }

        m_movingSled.MoveOrderUI();
        DeleteChild();
        Invoke("NewGift", 3f);
    }

    public void DeleteChild()
    {
        for (int i = m_productGiftImage.GetComponent<RectTransform>().childCount - 1; i >= 0; i--)
        {
            Transform childTransform = m_productGiftImage.GetComponent<RectTransform>().GetChild(i);
            GameObject child = childTransform.gameObject;

            Sticker sticker = child.GetComponent<Sticker>();

            if (m_productGiftImage.gameObject.transform.Find("LetterPos").GetComponentInChildren<Image>())
            {
                Destroy(m_productGiftImage.gameObject.transform.Find("LetterPos").GetComponentInChildren<Image>().gameObject);
            }

            if (sticker != null)
            {
                Destroy(sticker.gameObject);
            }
        }
    }

    public void NewGift()
    {
        GameManager.Instance.isSled = false;
        m_productGiftImage.gameObject.GetComponent<GiftDragger>().isInSled = false;
        GameManager.Instance.productView.m_presenter.m_product.Init();

        m_productGiftImage.gameObject.GetComponent<RectTransform>().anchoredPosition = m_productGiftImage.GetComponent<GiftDragger>().originalAnchoredPosition;
    }
    
}

[System.Serializable]
public class OrderDisplayObject
{
    public GameObject parent;
    public Image wrapper;
    public Image gift;
    public Image candy;
    public Image paper;
}

