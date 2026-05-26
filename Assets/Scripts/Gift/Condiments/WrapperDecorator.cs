using System;

public class WrapperDecorator : CondimentDecorator
{
    private GiftData m_wrapperType;

    public WrapperDecorator(Gift pGift, GiftData pPaperData)
    {
        m_gift = pGift;
        m_wrapperType = pPaperData;
    }

    public override void GetGiftData()
    {
        base.GetGiftData();
        m_wantedGift.Add(m_wrapperType);
    }
}


