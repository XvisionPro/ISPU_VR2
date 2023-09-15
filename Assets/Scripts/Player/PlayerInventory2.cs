using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory2 : MonoBehaviour
{
    /*[SerializeField] private Transform weaponParent2;
    [SerializeField] private GameObject weaponSlotPrefab2;
    private List<Weapon> weapons2 = new List<Weapon>();
    private int selectedWeaponIndex = 0;

    private void Start()
    {
        if (weaponParent2 == null)
        {
            weaponParent2 = GameObject.Find("GunSpawn").GetComponent<Transform>();
        }
        // Create weapon slots for each weapon in the inventory
        for (int i = 0; i < weapons2.Count; i++)
        {
            GameObject weaponSlot = Instantiate(weaponSlotPrefab2, weaponParent2);
            weaponSlot.GetComponent<WeaponSlot>().SetWeapon(weapons2[i]);
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        //weapons2.Add(weapon);

        // Create a new weapon slot for the added weapon
        GameObject weaponSlot = Instantiate(weaponSlotPrefab2, weaponParent2,false);
        //weaponSlot.GetComponent<WeaponSlot>().SetWeapon(weapon);
    }

    public void SelectWeapon(int index)
    {
        if (index >= 0 && index < weapons2.Count)
        {
            selectedWeaponIndex = index;

            // Select the corresponding weapon slot
            for (int i = 0; i < weaponParent2.childCount; i++)
            {
                WeaponSlot weaponSlot = weaponParent2.GetChild(i).GetComponent<WeaponSlot>();
                if (weaponSlot != null)
                {
                    weaponSlot.Select(i == selectedWeaponIndex);
                }
            }
        }
    }

    public Weapon GetSelectedWeapon()
    {
        if (selectedWeaponIndex >= 0 && selectedWeaponIndex < weapons2.Count)
        {
            return weapons2[selectedWeaponIndex];
        }
        else
        {
            return null;
        }
    }*/
}