using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductView : MonoBehaviour, IProductView
{
    
    [Header("Text UI")]
    [SerializeField] private GameObject m_textUI;
    [SerializeField] private TMP_Text m_text;

    [Header("Deco Buttons")]
    [SerializeField] private Button[] m_candyButtons;
    [SerializeField] private Button[] m_snackButtons;

    [SerializeField] private Button m_wrapperButton;
    [SerializeField] private Button m_stickerButton;
    [SerializeField] private Button m_letterButton;

    [Header("Gift Buttons")]
    [SerializeField] private Button[] m_bookButtons;
    [SerializeField] private Button[] m_carButtons;
    [SerializeField] private Button[] m_gameButtons;
    [SerializeField] private Button[] m_moneyButtons;
    [SerializeField] private Button[] m_dollButtons;

    [Header("Images")]
    [SerializeField] private Sprite[] m_giftImages; //선물 원형 이미지
    public Image m_productGiftImage; //제작 선물 이미지

    [Header("Wrap Buttons")]
    [SerializeField] private Button m_firstWrapper;
    [SerializeField] private Button m_secondWrapper;
    [SerializeField] private Button m_thirdWrapper;

    public ProductPresenter m_presenter;



    private void Awake()
    {
        m_presenter = GetComponent<ProductPresenter>();
        m_presenter.Bind(this);
    }


    private void Start()
    {

        for (Int32 i = 0; i< 3; i++)
        {
            m_bookButtons[i].onClick.AddListener(OnClickBookButton);
            m_carButtons[i].onClick.AddListener(OnClickCarButton);
            m_dollButtons[i].onClick.AddListener(OnClickDollButton);
            m_gameButtons[i].onClick.AddListener(OnClickGameButton);
            m_moneyButtons[i].onClick.AddListener(OnClickMoneyButton);
        }

        for (Int32 i = 0; i<2; i++)
        {
            m_candyButtons[i].onClick.AddListener(() => OnClickDecoCandyButton(GiftData.FirstCandy));
        }

        for (Int32 i = 2; i<4; i++)
        {
            m_candyButtons[i].onClick.AddListener(() => OnClickDecoCandyButton(GiftData.SecondCandy));
        }

        for (Int32 i = 4; i<6; i++)
        {
            m_candyButtons[i].onClick.AddListener(() => OnClickDecoCandyButton(GiftData.ThirdCandy));
        }


        for (Int32 i = 0; i < m_snackButtons.Length; i++)
        {
            m_snackButtons[i].onClick.AddListener(OnClickSnackButton);
        }

        m_firstWrapper.onClick.AddListener(() => OnClickDecoWrapperButton(GiftData.FirstWrapper));
        m_secondWrapper.onClick.AddListener(() => OnClickDecoWrapperButton(GiftData.SecondWrapper));
        m_thirdWrapper.onClick.AddListener(() => OnClickDecoWrapperButton(GiftData.ThirdWrapper));

        m_stickerButton.onClick.AddListener(OnClickStickerButton);
        m_letterButton.onClick.AddListener(OnClickLetterButton);
    }


    //버튼
    //이걸어떻게고칠까..
    public void OnClickGameButton()
    {
        if (GameManager.Instance.isSled)
        {
            ShowTextUI("설매가 다시 올 때까지 기다리세요!");
            return;
        }
        m_productGiftImage.sprite = m_giftImages[2];
        m_productGiftImage.gameObject.SetActive(true);
        GameManager.Instance.giftReinitializer.newGift.GetComponent<Image>().sprite = m_giftImages[2];
        ShowTextUI("선물 포장 준비가 완료되었어요!");
        m_presenter.MakeGame();
    }

    public void OnClickBookButton()
    {
        if (GameManager.Instance.isSled)
        {
            ShowTextUI("설매가 다시 올 때까지 기다리세요!");
            return;
        }
        m_productGiftImage.sprite = m_giftImages[0];
        m_productGiftImage.gameObject.SetActive(true);
        GameManager.Instance.giftReinitializer.newGift.GetComponent<Image>().sprite = m_giftImages[0];
        ShowTextUI("선물 포장 준비가 완료되었어요!");
        m_presenter.MakeBook();
    }

    public void OnClickCarButton()
    {
        if (GameManager.Instance.isSled)
        {
            ShowTextUI("설매가 다시 올 때까지 기다리세요!");
            return;
        }
        m_productGiftImage.sprite = m_giftImages[1];
        m_productGiftImage.gameObject.SetActive(true);
        GameManager.Instance.giftReinitializer.newGift.GetComponent<Image>().sprite = m_giftImages[1];
        ShowTextUI("선물 포장 준비가 완료되었어요!");
        m_presenter.MakeCar();
    }

    public void OnClickMoneyButton()
    {
        if (GameManager.Instance.isSled)
        {
            ShowTextUI("설매가 다시 올 때까지 기다리세요!");
            return;
        }
        m_productGiftImage.sprite = m_giftImages[3];
        m_productGiftImage.gameObject.SetActive(true);
        GameManager.Instance.giftReinitializer.newGift.GetComponent<Image>().sprite = m_giftImages[3];
        ShowTextUI("선물 포장 준비가 완료되었어요!");
        m_presenter.MakeMoney();
    }

    public void OnClickDollButton()
    {
        if (GameManager.Instance.isSled)
        {
            ShowTextUI("설매가 다시 올 때까지 기다리세요!");
            return;
        }
        m_productGiftImage.sprite = m_giftImages[4];
        m_productGiftImage.gameObject.SetActive(true);
        GameManager.Instance.giftReinitializer.newGift.GetComponent<Image>().sprite = m_giftImages[4];
        ShowTextUI("선물 포장 준비가 완료되었어요!");
        m_presenter.MakeDoll();
    }

    public void OnClickDecoCandyButton(GiftData pGiftData)
    {
        if (GameManager.Instance.isSled)
        {
            ShowTextUI("설매가 다시 올때까지 기다리세요!");
            return;
        }
        Debug.Log("candy");
        m_presenter.DecoCandy(pGiftData);
    }

    public void OnClickDecoWrapperButton(GiftData pGiftData)
    {
        m_presenter.DecoWrapper(pGiftData);
    }

    public void OnClickSnackButton()
    {
        if (GameManager.Instance.isSled)
        {
            ShowTextUI("설매가 다시 올때까지 기다리세요!");
            return;
        }
        m_presenter.DecoSnack();
    }

    public void OnClickStickerButton()
    {
        m_presenter.DecoSticker();
    }

    public void OnClickLetterButton()
    {
        m_presenter.WriteLetter();
    }


    //UI

    public void ShowTextUI(string pText)
    {
        m_text.text = pText;
        m_textUI.SetActive(true);
        
        CancelInvoke("HideTextUI");
        Invoke("HideTextUI", 2f);
    }

    public void HideTextUI()
    {
        m_textUI.SetActive(false);
    }

}


