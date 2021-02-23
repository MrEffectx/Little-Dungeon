using UnityEngine;
using UnityEngine.UI;

public class SellButtonItem : MonoBehaviour
{
    public Image itemImage;
    public Text itemName;
    public Text itemPrice;

    public Item item;

    public void BuyItem()
    {

        Inventory inventory = Inventory.instance;
        if(inventory.coinsCount >= item.price)
        {
            inventory.content.Add(item);
            inventory.UpdateInventoryUI();
            inventory.coinsCount -= item.price;
            inventory.UpdateTextUI();
        }
        Debug.Log("Cet item est vendu");
    }
}
