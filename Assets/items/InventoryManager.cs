using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated = false;
    public ItemSlot[] itemSlot;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }

        else if (Input.GetKeyDown(KeyCode.B) && !menuActivated)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        Debug.Log("itemName=" + itemName + "quantity=" + quantity + "itemSprite=" + itemSprite);
    }
}
