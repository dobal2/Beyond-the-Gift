using System;

public class CandyDecorator : CondimentDecorator
{
    GiftData m_candyType;

    public CandyDecorator(Gift pGift, GiftData pCandyData)
    {
        m_gift = pGift;
        m_candyType = pCandyData;
    }

    public override void GetGiftData()
    {
        base.GetGiftData();
        m_wantedGift.Add(m_candyType);
    }
}

