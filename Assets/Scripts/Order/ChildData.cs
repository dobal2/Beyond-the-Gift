using System;
using UnityEngine;

public class ChildData
{
    public Gift WantedGift { get; private set; }
    public bool IsCompleted { get; private set; } = false;
    
    

    public void WantGift()
    {
        GiftData randomGiftData = (GiftData)UnityEngine.Random.Range(6, 11);
        GiftData randomCandy = (GiftData)UnityEngine.Random.Range(0, 3);
        GiftData randomPaper = (GiftData)UnityEngine.Random.Range(3, 6);

        WantedGift = CreateGift(randomGiftData);
        WantedGift = new CandyDecorator(WantedGift, randomCandy);
        WantedGift = new WrapperDecorator(WantedGift, randomPaper);

        WantedGift.GetGiftData();

        Debug.Log($"원하는 선물: {string.Join(", ", WantedGift.m_wantedGift)}");
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true; // 의뢰 완료로 표시
    }

    private Gift CreateGift(GiftData pGiftData)
    {
        return pGiftData switch
        {
            GiftData.Doll => new Doll(),
            GiftData.Car => new Car(),
            GiftData.Book => new Book(),
            GiftData.Game => new Game(),
            GiftData.Money => new Money(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}