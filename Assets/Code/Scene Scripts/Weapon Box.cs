using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponBox : MonoBehaviour
{
   
   [SerializeField] private BoxCollider2D boxCollider;

    private void Start()
    {
        //messageText.SetActive(false);  // Oculta el mensaje al inicio
    }

    /*private void OnMouseDown()
    {
        if (!GameManager.Instance.hasWeapon)
        {
            GameManager.Instance.hasWeapon = true;  // El jugador obtiene el arma
            StartCoroutine(DisplayMessage());
        }
    }*/

    void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.Instance == null)
            Debug.LogWarning("Falta el GameManager");

        GameManager.Instance.hasWeapon = true;  // El jugador obtiene el arma
        StartCoroutine(DisplayMessage());
    }

    private IEnumerator DisplayMessage()
    {
        //messageText.SetActive(true);
        yield return new WaitForSeconds(3);  // Muestra el mensaje durante 3 segundos
        //messageText.SetActive(false);
    }
}


