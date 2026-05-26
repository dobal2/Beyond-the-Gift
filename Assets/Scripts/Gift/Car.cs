public class Car : Gift
{
    public Car()
    {
        m_giftData = GiftData.Car;
    }
    public override void GetGiftData()
    {
        m_wantedGift.Add(m_giftData);
    }
}