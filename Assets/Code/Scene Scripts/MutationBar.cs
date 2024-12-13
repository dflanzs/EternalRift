using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationBar : MonoBehaviour
{
    public float currentCharge = 0f;
    public float maxCharge = 100f;

    [SerializeField] private Player player; // Arrastra tu jugador desde el Inspector
    public MutationBarUi mutationBarUi; // Referencia a la UI

    private static MutationBar instance; // Singleton para preservar el estado

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Este objeto no se destruirá al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }

    void Start()
    {
        mutationBarUi.UpdateBar(currentCharge, maxCharge); // Asegura que la UI refleje el progreso actual
    }

    public void AddCharge(float amount)
    {
        currentCharge += amount;
        mutationBarUi.UpdateBar(currentCharge, maxCharge);

        if (currentCharge >= maxCharge)
        {
            currentCharge = maxCharge;

            if (player != null)
            {
                player.ActivateMutation();
            }
        }
    }
}
