using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class DeactivateNCP : MonoBehaviour
{
    struct DeactivatedNPC
    {
        public int health;
        public Vector2 position;
        public bool flies;
    }

    GameObject _player;

    List<DeactivatedNPC> npcList = new List<DeactivatedNPC>();

    void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
        foreach (DeactivatedNPC deactivatedNPC in npcList)
        {
            if(playerIsNear(deactivatedNPC.position))
            {
                GameObject enemy = ObjectPooling.Instance.requestInstance("enemy");

                if (enemy != null)
                {
                    enemy.SetActive(true);
                    StateManager npc = enemy.GetComponent<StateManager>();

                    enemy.transform.position = deactivatedNPC.position;
                    enemy.GetComponent<StateManager>().setHealth(deactivatedNPC.health);
                    enemy.GetComponent<StateManager>().setFlies(deactivatedNPC.flies);
            
                    npc.setPrevstate(npc.deactivatedState);
                    npc.SwitchState(npc.idleState);
                }
            }
        }
    }

    private bool playerIsNear(Vector2 _lastPosition)
    {
        if (Math.Abs(_player.transform.position.x - _lastPosition.x) >= 30 ||
            Math.Abs(_player.transform.position.y - _lastPosition.y) >= 30)
        {
            return false;
        }
        return true;
    }
}
