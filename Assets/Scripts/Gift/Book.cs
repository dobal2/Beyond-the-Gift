public class Book : Gift
{
    public Book()
    {
        m_giftData = GiftData.Book;
    }
    public override void GetGiftData()
    {
        m_wantedGift.Add(m_giftData);
    }
}