
using System.Collections.Generic;

public abstract class Gift
{
    protected GiftData m_giftData;
    public GiftData GiftData => m_giftData;

    public List<GiftData> m_wantedGift = new List<GiftData>();
    public abstract void GetGiftData();
}
