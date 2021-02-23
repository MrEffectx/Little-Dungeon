using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDataBase : MonoBehaviour
{
    public Item[] allItems;

    public static ItemsDataBase instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de ItemsDataBase dans la scène");
            return;
        }

        instance = this;
    }
}
