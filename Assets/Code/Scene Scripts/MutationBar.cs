using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationBar : MonoBehaviour
{
    public float currentCharge = 0f;
    public float maxCharge = 100f;

    [SerializeField] private Player player; // Arrastra tu jugador desde el Inspector
    public MutationBarUi mutationBarUi; // Referencia a la UI
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    public void AddCharge(float amount)
    {
      
        currentCharge += amount;
        
            mutationBarUi.UpdateBar(currentCharge, maxCharge);
        

        if (currentCharge >= maxCharge)
        {
            
            currentCharge = maxCharge;

            // Llama directamente a la función de mutación del jugador
            if (player != null)
            {
                player.ActivateMutation();
            }
        }
    }
}
