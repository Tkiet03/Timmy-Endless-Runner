using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public int ItemID;
    public Text PriceTxt;
    public Text QuantityTxt;
    public GameObject ShopManager;



    void Update()
    {
        PriceTxt.text ="Price: $"+ShopManager.GetComponent<ShopManagerScript>().shopItems[2,ItemID].ToString();
        QuantityTxt.text = ShopManager.GetComponent<ShopManagerScript>().shopItems[2, ItemID].ToString();
    }
}
