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
        //Esto para testear bien, pero si no le damos todas las armas al jugador de primeras
        //Weapon[] _weapons = Resources.LoadAll<Weapon>("Weapons");
        /*foreach(Weapon weapon in _weapons){
            weapons.Add(weapon);
        }*/
        
      if(weapons.Count > 0) // Si la lista no esta vacia
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