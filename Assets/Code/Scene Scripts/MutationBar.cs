using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationBar : MonoBehaviour
{
    public float maxCharge = 100f;

    [SerializeField] private Player player; // Arrastra tu jugador desde el Inspector
    private PlayerData playerData;
    public MutationBarUi mutationBarUi; // Referencia a la UI

    void Awake()
    {
    }

    void Start()
    {
        playerData = player.playerData;
        // Asegura que la carga se resetee al iniciar la escena
        //ResetCharge();
        mutationBarUi.UpdateBar(playerData.currentCharge, maxCharge); // Asegura que la UI refleje el progreso actual
        AddCharge(0);
    }

    public void AddCharge(float amount)
    {
        Debug.LogWarning($"{playerData.currentCharge} + {amount}");
        playerData.currentCharge += amount;
        mutationBarUi.UpdateBar(playerData.currentCharge, maxCharge);

        if (playerData.currentCharge >= maxCharge)
        {
            playerData.currentCharge = maxCharge;

            if (player != null)
            {
                player.ActivateMutation();
            }
        }
    }

    public void ResetCharge()
    {
        playerData.currentCharge = 0f;
        mutationBarUi.UpdateBar(playerData.currentCharge, maxCharge);
    }
}
