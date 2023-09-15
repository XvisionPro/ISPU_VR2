using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    /*[SerializeField] private Transform weaponParent;
    [SerializeField] private GameObject weaponSlotPrefab;
    private List<Weapon> weapons = new List<Weapon>();
    private int selectedWeaponIndex = 0;
    private void Start()
    {
        if (weaponParent==null)
        {
            weaponParent = GameObject.Find("GunSpawn").GetComponent<Transform>();
        }
        // Create weapon slots for each weapon in the inventory
        for (int i = 0; i < weapons.Count; i++)
        {
            //GameObject weaponSlot = Instantiate(weaponSlotPrefab, weaponParent);
            //weaponSlot.GetComponent<WeaponSlot>().SetWeapon(weapons[i]);
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        //weapons.Add(weapon);

        // Create a new weapon slot for the added weapon

        GameObject weaponSlot = Instantiate(weaponSlotPrefab, weaponParent, false);
        //weaponSlot.GetComponent<WeaponSlot>().SetWeapon(weapon);

    }

    public void SelectWeapon(int index)
    {
        if (index >= 0 && index < weapons.Count)
        {
            selectedWeaponIndex = index;

            // Select the corresponding weapon slot
            for (int i = 0; i < weaponParent.childCount; i++)
            {
                WeaponSlot weaponSlot = weaponParent.GetChild(i).GetComponent<WeaponSlot>();
                if (weaponSlot != null)
                {
                    weaponSlot.Select(i == selectedWeaponIndex);
                }
            }
        }
    }

    public Weapon GetSelectedWeapon()
    {
        if (selectedWeaponIndex >= 0 && selectedWeaponIndex < weapons.Count)
        {
            return weapons[selectedWeaponIndex];
        }
        else
        {
            return null;
        }
    }*/
}
