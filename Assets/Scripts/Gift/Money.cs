
public class Money : Gift
{
    public Money()
    {
        m_giftData = GiftData.Money;
    }
    public override void GetGiftData()
    {
        m_wantedGift.Add(m_giftData);
    }
}