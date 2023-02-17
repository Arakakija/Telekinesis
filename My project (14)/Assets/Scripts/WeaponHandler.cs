using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponLogics;

    public void EnableWeapon()
    {
        foreach (var weaponLogic in weaponLogics)
        {
            weaponLogic.SetActive(true);
        }
    }
    
    public void DisableWeapon()
    {
        foreach (var weaponLogic in weaponLogics)
        {
            weaponLogic.SetActive(false);
        }
    }
}
