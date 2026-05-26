

using System.Collections.Generic;

public abstract class CondimentDecorator : Gift
{
    protected Gift m_gift;

    public override void GetGiftData()
    {
        m_gift.GetGiftData();
        m_wantedGift.AddRange(m_gift.m_wantedGift);

    }
}