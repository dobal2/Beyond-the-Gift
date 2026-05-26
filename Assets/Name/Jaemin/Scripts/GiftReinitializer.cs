
using UnityEngine;
using UnityEngine.UI;

public class GiftReinitializer : MonoBehaviour
{
    public GameObject originalGift; // 원본 Gift
    public GameObject newGift;      // 새로 생성할 Gift
    public Sprite transparentImage;
    private ProductView productView;
    private OrderDisplay orderDisplay;

    private Order order;

    private void Start()
    {
        order = FindObjectOfType<Order>();
        productView = FindObjectOfType<ProductView>();
        orderDisplay = FindObjectOfType<OrderDisplay>();
    }

    public void Complete()
    {
        RectTransform originalGiftRect = originalGift.GetComponent<RectTransform>();

        originalGift.GetComponent<Image>().sprite = transparentImage;

        // 새 Gift 복제
        GameObject copiedNewGift = Instantiate(newGift, originalGift.transform.parent);

        copiedNewGift.AddComponent<Button>();
        copiedNewGift.AddComponent<GiftDoubleClick>();
        copiedNewGift.GetComponent<GiftDoubleClick>().transparentImage = transparentImage;

        productView.m_productGiftImage = copiedNewGift.GetComponent<Image>();
        
        RectTransform copiedRect = copiedNewGift.GetComponent<RectTransform>();

        // 스케일 및 위치 설정
        copiedRect.localScale = new Vector3(0.4f, 0.4f, 1f);
        copiedRect.position = originalGiftRect.position;

        // 원본의 LetterPos에서 Letter 복사
        Transform originalLetterPos = originalGift.transform.Find("LetterPos");
        if (originalLetterPos != null)
        {
            Transform originalLetter = originalGift.GetComponentInChildren<LetterCheckingScript>()?.gameObject.transform;
           
            if (originalLetter != null)
            {
                // 새 Gift의 LetterPos 위치 찾기
                Transform copiedLetterPos = copiedNewGift.transform.Find("LetterPos");
                if (copiedLetterPos != null)
                {
                    // Letter 복제 및 추가
                    GameObject copiedLetter = Instantiate(originalLetter.gameObject, copiedLetterPos);
                    copiedLetter.transform.localPosition = Vector3.zero;
                    copiedLetter.transform.localScale = originalLetter.localScale;

                    // Raycast 비활성화
                    Image letterImage = copiedLetter.GetComponent<Image>();
                    if (letterImage != null)
                    {
                        letterImage.raycastTarget = false;
                    }
                }
            }
        }

        // 자식 스티커 처리
        for (int i = copiedRect.childCount - 1; i >= 0; i--)
        {
            Transform childTransform = copiedRect.GetChild(i);
            GameObject child = childTransform.gameObject;

            // 이미지 raycast 비활성화
            Image imageComponent = child.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.raycastTarget = false;
            }

            // 스티커 상태 완료 처리
            Sticker sticker = child.GetComponent<Sticker>();
            if (sticker != null)
            {
                sticker.isCompleted = true;
            }
        }

        // GiftDragger 컴포넌트 추가
        copiedNewGift.AddComponent<GiftDragger>();
        copiedNewGift.GetComponent<GiftDragger>().order = order;
        orderDisplay.m_productGiftImage = copiedNewGift.GetComponent<Image>();

        // 원본 삭제 및 새 Gift 할당
        GameManager.Instance.DisableStickerUi();
        Destroy(originalGift);
        originalGift = copiedNewGift;
    }
}
