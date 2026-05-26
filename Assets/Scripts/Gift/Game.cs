using System.Collections.Generic;

public class Game : Gift
{
    public Game()
    {
        m_giftData = GiftData.Game;
    }
    public override void GetGiftData()
    {
        m_wantedGift.Add(m_giftData);
    }
}