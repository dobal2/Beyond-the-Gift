
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProductModel
{
    Gift m_productGift;
    public Boolean isGiftReady = false;
    public Boolean isCandyReady = false;
    public Boolean isWrapperFinished = false;


    //선물 생산
    public void ProductGame()
    {
        m_productGift = new Game();
        isGiftReady = true;
    }

    public void ProductCar()
    {
        m_productGift = new Car();
        isGiftReady = true;
    }

    public void ProductBook()
    {
        m_productGift = new Book();
        isGiftReady = true;
    }

    public void ProductMoney()
    {
        m_productGift = new Money();
        isGiftReady = true;
    }

    public void ProductDoll()
    {
        m_productGift = new Doll();
        isGiftReady = true;
    }



    //선물 포장
    public void DecoCandy(GiftData pGiftData)
    {
        if (!isGiftReady)
        {
            Debug.Log("선물이 준비되지 않았습니다.");
            return;
        }

        if (isWrapperFinished||isCandyReady)
        {
            Debug.Log("이미 포장되었거나 이미 추가되었거나");
            return;
        }

        m_productGift = new CandyDecorator(m_productGift,pGiftData);
        Debug.Log("사탕 추가");
        isCandyReady = true;
    }

    public void DecoWrapper(GiftData pGiftData)
    {
        if (!isGiftReady)
        {
            Debug.Log("선물이 준비되지 않았습니다.");
            return;
        }

        if (isWrapperFinished)
        {
            Debug.Log("이미 포장되었어요");
            return;
        }

        m_productGift = new WrapperDecorator(m_productGift, pGiftData);
        Debug.Log("포장지 추가");
        SetGiftData();
        isWrapperFinished = true;

    }

    //선물 정보
    public void SetGiftData()
    {
        m_productGift.GetGiftData();
        Debug.Log($"준비된 선물: {string.Join(", ", m_productGift.m_wantedGift)}");
    }

    public List<GiftData> GetGiftData()
    {
        if (isWrapperFinished)
            return m_productGift.m_wantedGift;

        Debug.Log("아직 포장 안함");
        GameManager.Instance.productView.ShowTextUI("포장을 안했습니다 다시 만드세요!");
        return null;
    }

    public void Init()
    {
        isGiftReady = false;
        isCandyReady = false;
        isWrapperFinished = false;
    }

}
