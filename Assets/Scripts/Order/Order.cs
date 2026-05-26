using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Order : MonoBehaviour
{
    [SerializeField] private ProductPresenter m_productPresenter;
    [SerializeField] private OrderGiftScriptable[] m_scriptableObjects;
    [SerializeField] private OrderDisplay m_orderDisplay;

    public List<OrderGiftScriptable> m_wantedGift;
    public List<OrderGiftScriptable> m_wantedCandy;
    public List<OrderGiftScriptable> m_wantedWrapper;

    private List<ChildData> m_children = new List<ChildData>();

    private const int CHILD_COUNT = 5;

    public void Start()
    {
        StartOrder();
    }

    
    public void StartOrder()
    {
        ResetOrder();
    }
    
    public void ResetOrder()
    {
        // 모든 데이터 초기화
        m_children.Clear();
        m_wantedGift = new List<OrderGiftScriptable>();
        m_wantedCandy = new List<OrderGiftScriptable>();
        m_wantedWrapper = new List<OrderGiftScriptable>();


        // 새로운 의뢰 생성
        MakeNewOrder();

       
        bool isDuplicateFound = true; 
        while (isDuplicateFound)
        {
            isDuplicateFound = false;

            for (int i = 0; i < CHILD_COUNT - 1; i++)
            {
                List<GiftData> wantedGift1 = m_children[i].WantedGift.m_wantedGift;

                for (int j = i + 1; j < CHILD_COUNT; j++)
                {
                    List<GiftData> wantedGift2 = m_children[j].WantedGift.m_wantedGift;

                  
                    bool isDuplicate = wantedGift1.All(gift1 => wantedGift2.Contains(gift1));

                    if (isDuplicate)
                    {
                        m_children.Clear(); 
                        MakeNewOrder();
                        isDuplicateFound = true; 
                        break; 
                    }
                }

                if (isDuplicateFound)
                    break; 
            }
        }

        // ScriptableObject와 매칭
        EnumToSO();

        Debug.Log("의뢰가 초기화되었습니다.");
    }

    public void EnumToSO()
    {
        m_wantedGift = new List<OrderGiftScriptable>();
        m_wantedCandy = new List<OrderGiftScriptable>();
        m_wantedWrapper = new List<OrderGiftScriptable>();

        for (int i = 0; i < CHILD_COUNT; i++)
        {
            List<GiftData> giftType = m_children[i].WantedGift.m_wantedGift;
            OrderGiftScriptable matchedSOGift = m_scriptableObjects.FirstOrDefault(so => so.name == giftType[0].ToString());
            OrderGiftScriptable matchedSOCandy = m_scriptableObjects.FirstOrDefault(so => so.name == giftType[1].ToString());
            OrderGiftScriptable matchedSOWrapper = m_scriptableObjects.FirstOrDefault(so => so.name == giftType[2].ToString());

            m_wantedGift.Add(matchedSOGift);
            m_wantedCandy.Add(matchedSOCandy);
            m_wantedWrapper.Add(matchedSOWrapper);
        }
    }

    public void CheckGift()
    {
        List<GiftData> productGift = m_productPresenter.GetGiftData();

        if (productGift == null)
        {
            GameManager.Instance.productView.ShowTextUI("포장이나 사탕을 추가하세요!");
            m_orderDisplay.m_movingSled.MoveSledUI();
            m_orderDisplay.NewGift();
            return;
        }

        bool isAnySatisfied = false;
        int satisfiedIndex = -1;

        for (int i = 0; i < m_children.Count; i++)
        {
            var child = m_children[i];
            List<GiftData> wantedGifts = child.WantedGift.m_wantedGift;

            bool isSatisfied = wantedGifts.All(gift => productGift.Contains(gift));

            if (isSatisfied)
            {
                isAnySatisfied = true;
                satisfiedIndex = i;
                child.MarkAsCompleted(); // 의뢰 완료 표시
                break;
            }
        }

        m_orderDisplay.Check(isAnySatisfied, satisfiedIndex);

        // 모든 의뢰 완료 여부 확인
        if (m_children.All(child => child.IsCompleted))
        {
            Debug.Log("모든 의뢰가 완료되었습니다!");
            GameManager.Instance.EnableResultUi();
        }
    }

    void MakeNewOrder()
    {
        for (int i = 0; i < CHILD_COUNT; i++)
        {
            ChildData child = new ChildData();
            child.WantGift();
            m_children.Add(child);
        }
    }
}
