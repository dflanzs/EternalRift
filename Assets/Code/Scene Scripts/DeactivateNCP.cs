using UnityEngine;
using System.Collections.Generic;
using System.Threading;

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
            foreach (GameObject enemyFE in enemies)
            {
                if (!enemyFE.GetComponent<StateManager>().getFound())
                {
                    DeactivatedNPCclass deactivatedNPC = new DeactivatedNPCclass();

                    enemyFE.GetComponent<StateManager>().setFound(true);
                    
                    deactivatedNPC.characteristics = enemyFE.GetComponent<StateManager>().GetAllCharacteristics();

                    deactivatedNPC.flies = enemyFE.GetComponent<StateManager>().getFlies();
                    
                    deactivatedNPC.health = enemyFE.GetComponent<StateManager>().getHealth();
                    deactivatedNPC.position = enemyFE.GetComponent<StateManager>().getStartingPosition();

                    deactivatedNPC.isActivated = false;
                    
                    npcList.Add(deactivatedNPC);
                    enemyFE.SetActive(false); // Desactivar el NPC inicialmente
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
                    Debug.Log("playerNear");
                    if (deactivatedNPC.characteristics.flies)
                        enemy = ObjectPooling.Instance.requestInstance("Enemy2", deactivatedNPC.characteristics.hashCode);
                    if(!deactivatedNPC.characteristics.flies)
                        enemy = ObjectPooling.Instance.requestInstance("Enemy1", deactivatedNPC.characteristics.hashCode);

                    if (enemy != null)
                    {
                        Debug.Log("Requesting enemy");

                        StateManager npc = enemy.GetComponent<StateManager>();

                        // Restore NPC characteristics
                        npc.SetAllCharacteristics(deactivatedNPC.characteristics);

                        enemy.transform.position = deactivatedNPC.position;
                        npc.setHealth(deactivatedNPC.health); // Ensure health is set correctly
                        npc.setFlies(deactivatedNPC.flies);

                        npc.deactivatedNPC = deactivatedNPC;

                        npc.setPrevstate(npc.deactivatedState);
                        npc.SwitchState(npc.idleState);

                        // Mark the original enemy as active to avoid duplicates
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