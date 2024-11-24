using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MutationBarUi : MonoBehaviour
{
    [SerializeField] private Image mutationBarFill;
    [SerializeField] private MutationBar mutationBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void UpdateBar(float currentCharge, float maxCharge)
    {
        if (mutationBarFill != null)
        {
            Debug.Log("AUMENTeE");
            float fillAmount = currentCharge / maxCharge;
            mutationBarFill.fillAmount = fillAmount; // Actualiza el progreso visual
        }
    }
}
