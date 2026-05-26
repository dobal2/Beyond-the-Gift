
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int stage;
    public float[] limitTime = { 150, 100 };
    public int[] stageStartCookie = { 1000, 1500 };
    public Sprite[] gifts;
    public GiftReinitializer giftReinitializer;
    public GameObject wrapperUi;
    public GameObject stickerUi;
    public GameObject letterUi;
    public GameObject draw;
    public GameObject shopUi;
    public GameObject resultUi;

    private string cookieSaveKey = "cookie";
    [SerializeField] private int cookie;
    [SerializeField] private float elapsedTime;
    [SerializeField] private float penaltyCounter;
    private const float Stage2GameOverDelay = 250f;
    private const float Stage3GameOverDelay = 150f;
    private const float Stage2PenaltyInterval = 10f;
    private const float Stage3PenaltyInterval = 5f;
    private const int Stage2PenaltyCookie = 100;
    private const int Stage3PenaltyCookie = 150;

    private bool isTimeTracking;
    private int beforeStage;

    public ProductView productView;

    public MovingSled movingSled;

    private RollingUI rollingUI;

    public float gameTime;
    public bool isGameStated;

    public bool isSled;

    public Button startButton;
    public Button letterButton;
    public Button stickerButton;
    public Button wrapperButton;

    public TextMeshProUGUI limitTimeDisplayer;

    private Order order;
    private OrderDisplay orderDisplay;
    

    private void Start()
    {
        orderDisplay = FindObjectOfType<OrderDisplay>();
        order = FindObjectOfType<Order>();
        rollingUI = FindObjectOfType<RollingUI>();
        productView = FindObjectOfType<ProductView>();
        LoadCookie();
        beforeStage = stage;
        elapsedTime = 0f;
        penaltyCounter = 0f;
        isTimeTracking = true;
        startButton.onClick.AddListener(StartGame);
        letterButton.onClick.AddListener(EnableLetterUi);
        stickerButton.onClick.AddListener(EnableStickerUi);
        wrapperButton.onClick.AddListener(EnableWrapperUi);
        
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
            AddCookie(9999);
        if (Input.GetKeyDown(KeyCode.R))
            PlayerPrefs.DeleteAll();
#endif

        if (isGameStated)
        {
            gameTime += Time.deltaTime;
        }
        
        if (stage >= 2 && isGameStated)
        {
            TrackTime();
            limitTimeDisplayer.gameObject.SetActive(true);
        }
        else
        {
            limitTimeDisplayer.gameObject.SetActive(false);
        }
        
        if (beforeStage != stage)
        {
            InitializeStage();
        }
        
        beforeStage = stage;
    }

    private void TrackTime()
    {
        if (!isTimeTracking) return;

        elapsedTime += Time.deltaTime;

        if (stage == 2)
        {
            limitTimeDisplayer.text = "제한 시간 : "+(limitTime[stage - 2] - elapsedTime).ToString("N0");
            if (elapsedTime - limitTime[stage - 2] > Stage2GameOverDelay)
            {
                cookie = 0;
                isTimeTracking = false;
            }
            else if (elapsedTime >= limitTime[stage - 2])
            {
                penaltyCounter += Time.deltaTime;
                if (penaltyCounter >= Stage2PenaltyInterval)
                {
                    MinusCookie(Stage2PenaltyCookie);
                    penaltyCounter = 0f;
                }
            }
        }
        else if (stage == 3)
        {
            if (elapsedTime - limitTime[stage - 2] > Stage3GameOverDelay)
            {
                cookie = 0;
                isTimeTracking = false;
            }
            else if (elapsedTime >= limitTime[stage - 2])
            {
                penaltyCounter += Time.deltaTime;
                if (penaltyCounter >= Stage3PenaltyInterval)
                {
                    MinusCookie(Stage3PenaltyCookie);
                    penaltyCounter = 0f;
                }
            }
        }
    }

    private void InitializeStage()
    {
        if (stage >= 1)
        {
            isTimeTracking = true;
            cookie = stageStartCookie[stage - 1];
        }
        elapsedTime = 0f;
        penaltyCounter = 0f;
    }

    private void LoadCookie()
    {
        cookie = PlayerPrefs.GetInt(cookieSaveKey, 1000);
    }

    private void SaveCookie()
    {
        PlayerPrefs.SetInt(cookieSaveKey, cookie);
        PlayerPrefs.Save();
    }

    public void AddCookie(int value)
    {
        cookie += value;
    }

    public void MinusCookie(int value)
    {
        cookie = Mathf.Max(0, cookie - value);
    }

    public int GetCookieCount()
    {
        return cookie;
    }

    public void ChooseFirstWrapper()
    {
        giftReinitializer.originalGift.GetComponent<Image>().sprite = gifts[0];
        giftReinitializer.newGift.GetComponent<Image>().sprite = gifts[0];
        DisableWrapperUi();
    }

    public void ChooseSecondWrapper()
    {
        giftReinitializer.originalGift.GetComponent<Image>().sprite = gifts[1];
        giftReinitializer.newGift.GetComponent<Image>().sprite = gifts[1];
        DisableWrapperUi();
    }

    public void ChooseThirdWrapper()
    {
        giftReinitializer.originalGift.GetComponent<Image>().sprite = gifts[2];
        giftReinitializer.newGift.GetComponent<Image>().sprite = gifts[2];
        DisableWrapperUi();
    }

    public void EnableWrapperUi()
    {
        if (giftReinitializer.originalGift.GetComponent<GiftDoubleClick>().IsGiftTransparent())
        {
            productView.ShowTextUI("선물을 먼저 선택하세요");
            return;
        }
        else if (!productView.m_presenter.m_product.isCandyReady)
        {
            productView.ShowTextUI("사탕을 먼저 선택하세요");
            return;
        }
       
        wrapperUi.SetActive(true);
    }

    public void DisableWrapperUi()
    {
        wrapperUi.SetActive(false);
    }

    public void EnableStickerUi()
    {
        if (giftReinitializer.originalGift.GetComponent<GiftDoubleClick>().IsGiftTransparent())
        {
            productView.ShowTextUI("선물을 먼저 선택하세요");
            return;
        }
        stickerUi.SetActive(true);
    }

    public void DisableStickerUi()
    {
        stickerUi.SetActive(false);
    }

    public void EnableLetterUi()
    {
        if (!productView.m_presenter.m_product.isGiftReady)
        {
            productView.ShowTextUI("선물을 먼저 선택하세요");
            return;
        }
        letterUi.SetActive(true);
        draw.SetActive(true);
    }

    public void DisableLetterUi()
    {
        letterUi.SetActive(false);
        draw.SetActive(false);

        DrawMesh drawMesh = draw.GetComponent<DrawMesh>();
        drawMesh.SaveCurrentMesh();
        drawMesh.DeleteAllDrawMesh();
    }

    public void EnableShopUi()
    {
        shopUi.SetActive(true);
    }

    public void DisableShopUi()
    {
        shopUi.SetActive(false);
    }

    public void EnableResultUi()
    {
        resultUi.SetActive(true);
        TimeSpan timeSpan = TimeSpan.FromSeconds(gameTime);
        
        string formattedTime = $"{timeSpan.Minutes}분 {timeSpan.Seconds}초";
        
        resultUi.transform.Find("GameTimeText").GetComponent<TextMeshProUGUI>().text = "걸린시간 : " + formattedTime;
        resultUi.transform.Find("CookieText").GetComponent<TextMeshProUGUI>().text = "쿠키 : "+ cookie.ToString("N0");
        isGameStated = false;
    }

    public void DisableResultUi()
    {
        resultUi.SetActive(false);
    }

    public void NextStage()
    {
        if (stage >= 3)
        {
            Debug.Log("Clear");
            return;
        }
        stage++;
        order.ResetOrder();
        orderDisplay.ResetDisplay();
        DisableResultUi();
        EnableShopUi();
        gameTime = 0;
        
    }
    
    private void OnApplicationQuit()
    {
        SaveCookie();
    }


    public void StartGame()
    {
        movingSled.MoveOrderUI();
        rollingUI.MoveProductUI();
        DisableShopUi();
        isGameStated = true;
    }
}
