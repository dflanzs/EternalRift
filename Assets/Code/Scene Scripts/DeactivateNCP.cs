using UnityEngine;
using System.Collections.Generic;

namespace DeactivatedNPCns
{
    public class DeactivateNCP : MonoBehaviour
    {
        public class DeactivatedNPCclass
        {
            public Vector2 position;
            public bool flies;
            public int health;
            public bool isActivated;
            public StateManager.NPCCharacteristics characteristics;
        }

        private GameObject _player;
        private List<DeactivatedNPCclass> npcList;
        private GameObject enemy;
        void Start()
        {  
            npcList = new List<DeactivatedNPCclass>();
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("npc");
            
            DeactivatedNPCclass deactivatedNPC = new DeactivatedNPCclass();

            foreach (GameObject enemy in enemies)
            {
                Debug.Log($"NPC Starting Position: {enemy.GetComponent<StateManager>().getStartingPosition()}");

                if (!enemy.GetComponent<StateManager>().getFound())
                {
                    enemy.GetComponent<StateManager>().setFound(true);
                    
                    //deactivatedNPC.characteristics = enemy.GetComponent<StateManager>().GetAllCharacteristics();

                    deactivatedNPC.flies = enemy.GetComponent<StateManager>().getFlies();
                    deactivatedNPC.health = enemy.GetComponent<StateManager>().getHealth();
                    deactivatedNPC.position = enemy.GetComponent<StateManager>().getStartingPosition();

                    deactivatedNPC.isActivated = false;
                    
                    npcList.Add(deactivatedNPC);
                    enemy.SetActive(false); // Desactivar el NPC inicialmente
                }
            }

            _player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            for (int i = 0; i < npcList.Count; i++)
            {
                DeactivatedNPCclass deactivatedNPC = npcList[i];
                if (playerIsNear(deactivatedNPC.position) && !deactivatedNPC.isActivated)
                {
                    if (deactivatedNPC.flies){
                        Debug.Log("Enemy2");
                        enemy = ObjectPooling.Instance.requestInstance("Enemy2");
                    }
                    else{
                        Debug.Log("Enemy1");
                        enemy = ObjectPooling.Instance.requestInstance("Enemy1");
                    }

                    if (enemy != null)
                    {
                        StateManager npc = enemy.GetComponent<StateManager>();

                        // Restaurar las características del npc
                        //npc.SetAllCharacteristics(deactivatedNPC.characteristics);

                        enemy.transform.position = deactivatedNPC.position;
                        npc.setHealth(deactivatedNPC.health);
                        npc.setFlies(deactivatedNPC.flies);

                        npc.deactivatedNPC = deactivatedNPC;

                        npc.setPrevstate(npc.deactivatedState);
                        npc.SwitchState(npc.idleState);

                        // Marcar el enemigo original como activo para evitar duplicados
                        enemy.SetActive(true);
                        deactivatedNPC.isActivated = true;
                        npcList[i] = deactivatedNPC;
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
}