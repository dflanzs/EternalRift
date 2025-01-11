using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    [SerializeField] private Toggle fallDamage;
    [SerializeField] private Toggle autoShoot;

    private void Start(){
        fallDamage.isOn = GameManager.Instance.fallDamage; 
        autoShoot.isOn = GameManager.Instance.autoShoot;
    }

    public void ToggleAutoShoot(bool value){
        GameManager.Instance.autoShoot = value;
    }

    public void ToggleFallDamage(bool value){
        GameManager.Instance.fallDamage = value;
    }

}
