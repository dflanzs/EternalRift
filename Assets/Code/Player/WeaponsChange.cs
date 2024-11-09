using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponsChange : MonoBehaviour
{
    [SerializeField] private Shoot scriptShoot;

    [SerializeField] private List<Weapon> weapons = new List<Weapon>();
    private int whichWeapon = 0;

    void Start(){
        Weapon[] _weapons = Resources.LoadAll<Weapon>("Weapons");
        foreach(Weapon weapon in _weapons){
            weapons.Add(weapon);
        }
        scriptShoot.Weapon = weapons[whichWeapon];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            whichWeapon = (whichWeapon + 1) % weapons.Count;
            scriptShoot.Weapon = weapons[whichWeapon];

        }
    }

}
