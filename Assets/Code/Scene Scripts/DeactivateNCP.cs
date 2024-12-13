using UnityEngine;
using System.Collections.Generic;

public class DeactivateNCP : MonoBehaviour
{
    struct DeactivatedNPC
    {
        public Vector2 position;
        public bool flies;
        public int health;
        public StateManager.NPCCharacteristics characteristics;
    }
    private GameObject _player;
    private List<DeactivatedNPC> npcList;
    private GameObject enemy;
    void Start()
    {  
        npcList = new List<DeactivatedNPC>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("npc");
        
        DeactivatedNPC deactivatedNPC = new DeactivatedNPC();

        foreach (GameObject enemy in enemies)
        {
            Debug.Log($"NPC Starting Position: {enemy.GetComponent<StateManager>().getStartingPosition()}");

            if (!enemy.GetComponent<StateManager>().getFound())
            {
                enemy.GetComponent<StateManager>().setFound(true);
                
                deactivatedNPC.characteristics = enemy.GetComponent<StateManager>().GetAllCharacteristics();

                deactivatedNPC.flies = enemy.GetComponent<StateManager>().getFlies();
                deactivatedNPC.health = enemy.GetComponent<StateManager>().getHealth();
                deactivatedNPC.position = enemy.GetComponent<StateManager>().getStartingPosition();

                npcList.Add(deactivatedNPC);
                
                enemy.SetActive(false); // Desactivar el NPC inicialmente
            }
        }

        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        foreach (DeactivatedNPC deactivatedNPC in npcList)
        {
            if (playerIsNear(deactivatedNPC.position))
            {
                if (deactivatedNPC.flies)
                    enemy = ObjectPooling.Instance.requestInstance("Enemy2");
                else
                    enemy = ObjectPooling.Instance.requestInstance("Enemy1");

                if (enemy != null)
                {
                    enemy.SetActive(true);
                    StateManager npc = enemy.GetComponent<StateManager>();

                    // Restaurar las características del npc
                    npc.SetAllCharacteristics(deactivatedNPC.characteristics);

                    enemy.transform.position = deactivatedNPC.position;
                    npc.setHealth(deactivatedNPC.health);
                    npc.setFlies(deactivatedNPC.flies);

                    npc.setPrevstate(npc.deactivatedState);
                    npc.SwitchState(npc.idleState);

                    // Marcar el enemigo original como activo para evitar duplicados
                    enemy.gameObject.SetActive(true);
                }
            }
        }
    }

    private bool playerIsNear(Vector2 _lastPosition)
    {
        return Mathf.Abs(_player.transform.position.x - _lastPosition.x) < 30 &&
               Mathf.Abs(_player.transform.position.y - _lastPosition.y) < 30;
    }
}