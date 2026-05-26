
using UnityEngine;
using UnityEngine.UI;

public class GiftDoubleClick : MonoBehaviour
{
    public Sprite transparentImage;
    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    private GiftReinitializer giftReinitializer;
    private RectTransform rectTransform;
    
    private Vector2 originalAnchoredPosition;
    private GiftDragger giftDragger;
    private Button targetButton;
    private Image image;

    private ProductView productView;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        giftReinitializer = FindObjectOfType<GiftReinitializer>();
        productView = FindObjectOfType<ProductView>();
        
        originalAnchoredPosition = rectTransform.anchoredPosition;
        targetButton = GetComponent<Button>();
        image = GetComponent<Image>();
        
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(OnSingleClick);
        }
    }

    private void OnSingleClick()
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= doubleClickThreshold)
        {
            OnDoubleClick();
        }

        lastClickTime = Time.time;
    }

    private void OnDoubleClick()
    {
        image.sprite = transparentImage;
        giftReinitializer.newGift.GetComponent<Image>().sprite = transparentImage;
        rectTransform.anchoredPosition = originalAnchoredPosition;
        for (int i = rectTransform.childCount - 1; i >= 0; i--)
        {
            Transform childTransform = rectTransform.GetChild(i);
            GameObject child = childTransform.gameObject;
            
            Sticker sticker = child.GetComponent<Sticker>();

            if (transform.Find("LetterPos").GetComponentInChildren<Image>())
            {
                Destroy(transform.Find("LetterPos").GetComponentInChildren<Image>().gameObject);
            }
            
            if (sticker != null)
            {
                Destroy(sticker.gameObject);
            }
        }
        for (int i = giftReinitializer.newGift.GetComponent<RectTransform>().childCount - 1; i >= 0; i--)
        {
            Transform childTransform = giftReinitializer.newGift.GetComponent<RectTransform>().GetChild(i);
            GameObject child = childTransform.gameObject;
            
            Sticker sticker = child.GetComponent<Sticker>();
            if (sticker != null)
            {
                Destroy(sticker.gameObject);
            }
        }
        productView.m_presenter.m_product.Init();
        GetComponent<GiftDragger>().isInSled = false;
        Debug.Log("선물 상자 더블클릭 초기화");
    }

    public bool IsGiftTransparent()
    {
        if (image.sprite == transparentImage)
        {
            return true;
        }

        return false;
    }
}
