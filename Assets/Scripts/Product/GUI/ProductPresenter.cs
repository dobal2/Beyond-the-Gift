
using System.Collections.Generic;
using UnityEngine;

public class ProductPresenter : MonoBehaviour
{

    private IProductView m_productView;
    public ProductModel m_product;

    private void Awake()
    {
        m_product = new ProductModel();
    }

    public void Bind(IProductView pView)
    {
        m_productView = pView;
    }

    public void MakeGame()
    {
        m_product.ProductGame();
    }

    public void MakeCar()
    {
        m_product.ProductCar();
    }

    public void MakeBook()
    {
        m_product.ProductBook();
    }

    public void MakeMoney()
    {
        m_product.ProductMoney();
    }

    public void MakeDoll()
    {
        m_product.ProductDoll();
    }


    public void DecoCandy(GiftData pGiftData)
    {
        m_product.DecoCandy(pGiftData);
    }

    public void DecoWrapper(GiftData pGiftData)
    {
        m_product.DecoWrapper(pGiftData);
    }

    public void DecoSnack()
    {

    }

    public void WriteLetter()
    {

    }
    
    public void DecoSticker()
    {

    }

    public List<GiftData> GetGiftData()
    {
        return m_product.GetGiftData();
    }

}
