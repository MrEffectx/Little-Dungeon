using UnityEngine;
using System.Linq;

public class LoadAndSaveData : MonoBehaviour
{
    public bool isMainMenu = false;
    public static LoadAndSaveData instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de LoadAndSaveData dans la scène");
            return;
        }

        instance = this;
    }

    void Start()
    {
        if (!isMainMenu)
        {
            Inventory.instance.coinsCount = PlayerPrefs.GetInt("coinsCount", 0);
            Inventory.instance.UpdateTextUI();

            /* int currentHealth = PlayerPrefs.GetInt("playerHealth", PlayerHealth.instance.maxHealth);
            PlayerHealth.instance.currentHealth = currentHealth;
            PlayerHealth.instance.healthBar.SetHealth(currentHealth); */

            string[] itemsSaved = PlayerPrefs.GetString("inventoryItems", "").Split(',');

            for (int i = 0; i < itemsSaved.Length; i++)
            {
                if (itemsSaved[i] != "")
                {
                    int id = int.Parse(itemsSaved[i]);
                    Item currentItem = ItemsDataBase.instance.allItems.Single(Matrix4x4 => Matrix4x4.id == id);
                    Inventory.instance.content.Add(currentItem);
                    //Debug.Log("Item chargé : " + itemsSaved[i]);
                }

            }

            Inventory.instance.UpdateInventoryUI();
        }

    }

    public void SaveData() //franchissement de la porte
    {
        PlayerPrefs.SetInt("coinsCount", Inventory.instance.coinsCount);
        //PlayerPrefs.SetInt("playerHealth", PlayerHealth.instance.currentHealth);

        if(CurrentSceneManager.instance.levelToUnlock > PlayerPrefs.GetInt("levelReached", 1))
        {
            PlayerPrefs.SetInt("levelReached", CurrentSceneManager.instance.levelToUnlock);
        }

        string itemsInInventory = string.Join(",", Inventory.instance.content.Select(x => x.id));
        PlayerPrefs.SetString("inventoryItems", itemsInInventory);
        Debug.Log("Les items sauvegardés sont : " + itemsInInventory);

   
    }
}
