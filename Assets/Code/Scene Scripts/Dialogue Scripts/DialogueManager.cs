using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Referencia al componente de texto para mostrar los diálogos
    public GameObject dialogueBox; // Referencia a la caja de diálogo (GameObject)
    public GameObject player; // Referencia al objeto jugador (para deshabilitar su movimiento)

    private Queue<string> sentences; // Cola para almacenar las oraciones del diálogo
    private bool isDialogueActive = false; // Indica si el diálogo está activo

    private void Start()
    {
        sentences = new Queue<string>();

        // Asegurarse de que la caja de diálogo esté desactivada al iniciar
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
    }

    private void Update()
    {
        // Detectar si se presiona Enter para pasar al siguiente diálogo
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextSentence();
        }
    }

    // Método para iniciar el diálogo
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Comenzando diálogo con: " + dialogue.name);

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(true); // Activa la caja de diálogo
        }

        isDialogueActive = true;

        // Pausar el juego y deshabilitar movimiento del jugador
        Time.timeScale = 0;
        DisablePlayerMovement(true);

        sentences.Clear(); // Limpia las oraciones previas

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence); // Añade todas las oraciones del diálogo
        }

        DisplayNextSentence(); // Muestra la primera oración
    }

    // Método para mostrar la siguiente oración
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue(); // Termina el diálogo si no quedan más oraciones
            return;
        }

        string sentence = sentences.Dequeue(); // Saca la siguiente oración de la cola
        StopAllCoroutines(); // Detiene cualquier animación de texto anterior
        StartCoroutine(TypeSentence(sentence)); // Inicia la animación de escritura del texto
    }

    // Corutina para mostrar el texto letra por letra
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = ""; // Limpia el texto actual
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; // Añade letra por letra
            yield return null; // Espera un frame entre cada letra
        }
    }

    // Método para terminar el diálogo
    public void EndDialogue()
    {
        Debug.Log("Diálogo terminado.");
        isDialogueActive = false;

        // Reanudar el juego y habilitar movimiento del jugador
        Time.timeScale = 1;
        DisablePlayerMovement(false);

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false); // Desactiva la caja de diálogo
        }
    }

    // Método para deshabilitar o habilitar el movimiento del jugador
    private void DisablePlayerMovement(bool disable)
    {
        // Si el jugador tiene un componente de movimiento, deshabilitarlo
        var playerMovement = player.GetComponent<Player>(); // Reemplaza Player por el nombre de tu script de movimiento
        if (playerMovement != null)
        {
            playerMovement.enabled = !disable; // Desactiva el movimiento si 'disable' es true, lo habilita si es false
        }
    }
}
