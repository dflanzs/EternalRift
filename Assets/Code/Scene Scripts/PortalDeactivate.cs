using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDeactivate : MonoBehaviour
{
    [SerializeField] private GameObject door; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            door.SetActive(false); // Desactiva la puerta
            gameObject.SetActive(false); // Desactiva el bot�n
        }
    }
}
