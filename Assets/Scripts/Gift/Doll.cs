
public class Doll : Gift
{
    public Doll()
    {
        m_giftData = GiftData.Doll;
    }
    public override void GetGiftData()
    {
        m_wantedGift.Add(m_giftData);

    }
}
