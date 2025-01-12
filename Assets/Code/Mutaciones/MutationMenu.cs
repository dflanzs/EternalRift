using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MutationMenu : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _panel;


    [SerializeField] private Button _salto;
    private bool b_salto = false;

    [SerializeField] private Button _cooldown;
    private bool b_cooldown = false;

    private float MutatedJumpForceMultiplier;

    // Start is called before the first frame update
    void Start()
    {  
        if(_player == null)
            Debug.LogAssertion("Necesitas asignar el jugador desde el editor en EleccionMutaciones");

        MutatedJumpForceMultiplier = _player.playerData.mutatedJumpMultiplier;

        Player.MutationBarEvent += HandleMutation;
        Player.MutationResetEvent += HandleMutationReset;
    }

    private void OnDestroy(){
        Player.MutationBarEvent -= HandleMutation;
        Player.MutationResetEvent -= HandleMutationReset;   
    }

    private void HandleMutation() {
        if(b_salto && b_cooldown)
            return;
            
        Time.timeScale = 0;

        _panel.SetActive(true);
    }

    private void HandleMutationReset() {
        _player.playerData.jumpVelocity = 20f;
        _player.playerData.cooldownWeapons = false;
    }

    public void Salto(){
        _panel.SetActive(false);

        if(!b_salto)
            _player.playerData.jumpVelocity *= MutatedJumpForceMultiplier;

        b_salto = true;
        _salto.gameObject.SetActive(false);

        Time.timeScale = 1.0f;
    }

    public void CooldownArmas(){
        _panel.SetActive(false);

        if(!b_cooldown)
            _player.playerData.cooldownWeapons = true;

        b_cooldown = true;
        _cooldown.gameObject.SetActive(false);

        Time.timeScale = 1.0f;
    }
}
