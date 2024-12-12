using UnityEngine;
using System.Collections.Generic;

public class DeactivateNCP : MonoBehaviour
{
    private GameObject _player;
    private List<StateManager> npcList;

    void Start()
    {
        Debug.Log("Loading enemies...");
        ObjectPooling op = ObjectPooling.Instance;
        
        npcList = new List<StateManager>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("npc");
        
        foreach (GameObject enemy in enemies)
        {
            StateManager enemySM = enemy.GetComponent<StateManager>(); 
            if (!enemySM.getFound())
            {
                npcList.Add(enemySM);
                enemySM.setFound(true);
                enemy.SetActive(false); // Desactivar el NPC inicialmente
            }
        }

        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Debug.Log("Checking enemies near player...");
        foreach (StateManager enemySM in npcList)
        {
            if (playerIsNear(enemySM.getStartingPosition()) && !enemySM.gameObject.activeSelf)
            {
                Debug.Log("Requesting Instance...");

                GameObject enemy = ObjectPooling.Instance.requestInstance("Enemy");

                if (enemy != null)
                {
                    enemy.SetActive(true);
                    StateManager npc = enemy.GetComponent<StateManager>();

                    // Restaurar la posición inicial desde el diccionario
                    enemy.transform.position = enemySM.getStartingPosition();
                    npc.setHealth(enemySM.getHealth());
                    npc.setFlies(enemySM.getFlies());

                    npc.setPrevstate(npc.deactivatedState);
                    npc.SwitchState(npc.idleState);

                    // Marcar el enemigo original como activo para evitar duplicados
                    enemySM.gameObject.SetActive(true);
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