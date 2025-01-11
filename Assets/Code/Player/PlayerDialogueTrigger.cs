using UnityEngine;
using System.Collections;

public class PlayerDialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager; // Referencia al DialogueManager
    private bool hasTriggeredMessage = false; // Evitar que el mensaje se muestre varias veces
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si el objeto con el que colisiona tiene la etiqueta "MensajeCuidado"
        if (other.CompareTag("MensajeCuidado") && !hasTriggeredMessage)
        {
            // Llamamos al método para mostrar el primer diálogo
            TriggerMessageDialogueAviso();
            hasTriggeredMessage = true; // Asegura que el mensaje solo se muestre una vez
        }

        // Verificamos si el objeto con el que colisiona tiene la etiqueta "MensajeCuidado"
        if (other.CompareTag("MensajeFinal") && !hasTriggeredMessage)
        {
            // Llamamos al método para mostrar el primer diálogo
            TriggerMessageDialogueFin();
            hasTriggeredMessage = true; // Asegura que el mensaje solo se muestre una vez
        }

        if ((other.CompareTag("cristal") || other.CompareTag("cristal_tocho")) && !hasTriggeredMessage)
        {
            // Llamamos al método para mostrar el primer diálogo
            TriggerMessageDialogueCristal();
            hasTriggeredMessage = true; // Asegura que el mensaje solo se muestre una vez
        }

        // Verificamos si el objeto con el que colisiona tiene la etiqueta "DialogueTrigger"
        if (other.CompareTag("DialogueTrigger") && !hasTriggeredMessage)
        {
            // Llamamos al método para mostrar el primer diálogo con un retraso
            StartCoroutine(DelayedTriggerMessageDialogueInicio());
            hasTriggeredMessage = true; // Asegura que el mensaje solo se muestre una vez
        }
    }


    private void TriggerMessageDialogueCristal()
    {
        // Crear el diálogo que deseas mostrar cuando el jugador entra en contacto con el Tilemap
        Dialogue cristalDialogue = new Dialogue
        {
            name = "Cristal",
            sentences = new string[]
            {
                "¡Mira! Eso es un fragmento de meteorito.",
                "Despues de que impactara, miles de fragmentos fueron desperdigados, tienen mucha energía.",
                "Podrías recoger algunos y traérmelos al laboratorio cuando vuelvas, me gustaría investigarlos.",
                "Aunque ten cuidado, su radiación podría provocarte efectos secundarios..."
                
            }
        };

        // Inicia el diálogo
        dialogueManager.StartDialogue(cristalDialogue);

        
    }

    
    private void TriggerMessageDialogueFin()
    {
        // Crear el diálogo que deseas mostrar cuando el jugador entra en contacto con el Tilemap
        Dialogue finDialogue = new Dialogue
        {
            name = "Fin",
            sentences = new string[]
            {
                "Parece que por fin hemos llegado al laboratorio pero...",
                "Esta totalmente en ruinas.",
                "Quien sabe que nos vamos a encontrar, hay que ir con mucho cuidado.",
                "Tenemos... enco... ",
                "...",
                "Esto... perdi... conex...",
                "Debes... seg... solo."
                
            }
        };

        // Inicia el diálogo
        dialogueManager.StartDialogue(finDialogue);
    }
    private void TriggerMessageDialogueAviso()
    {
        // Crear el diálogo que deseas mostrar cuando el jugador entra en contacto con el Tilemap
        Dialogue cautionDialogue = new Dialogue
        {
            name = "Alerta",
            sentences = new string[]
            {
                "Mis sensores detectan movimiento en esta zona de la cueva.",
                "Mantente alerta, podrían aparecer enemigos en cualquier momento."
                
            }
        };

        // Inicia el diálogo
        dialogueManager.StartDialogue(cautionDialogue);
    }

    private IEnumerator DelayedTriggerMessageDialogueInicio()
    {
        // Espera 2 segundos antes de iniciar el primer diálogo
        yield return new WaitForSeconds(1f);

        // Crear el diálogo inicial
        Dialogue initialDialogue = new Dialogue
        {
            name = "Guía",
            sentences = new string[]
            {
                "¿Orion?\n(Pulsa Enter para continuar)",
                "¿Me recibes? Al habla la Doctora Evee Kross.",
                "Genial, recepción establecida. Podemos comenzar la misión.",
                "Te recuerdo que tu objetivo es lograr salir de esta cueva para poder infiltrarnos de manera subterránea en el laboratorio de la Facción del Sur.",
                "Solo esos malditos saben cómo solucionar la catástrofe provocada por la caída del meteorito, ¡y no quieren compartirlo con el mundo!.",
                "Necesitamos esa información Orion, muchas vidas dependen de ella.",
                "Vamos a dar comienzo a la misión. Calentemos un poco.",
                "Camina utilizando las teclas A y D, puedes correr añadiendo Shift."
            }
        };

        // Inicia el primer diálogo
        dialogueManager.StartDialogue(initialDialogue);

        // Inicia la corutina para mostrar los siguientes diálogos con retraso
        StartCoroutine(TriggerSubsequentDialogues());
    }

    private IEnumerator TriggerSubsequentDialogues()
    {
        // Esperamos 3 segundos antes de mostrar el segundo diálogo
        yield return new WaitForSeconds(5f);

        // Crear y mostrar el segundo diálogo
        Dialogue secondDialogue = new Dialogue
        {
            name = "Guía",
            sentences = new string[] { "¡Genial!. Ahora prueba a saltar (Espacio) y a impulsarte (E)" }
        };
        dialogueManager.StartDialogue(secondDialogue);

        // Esperamos 3 segundos antes de mostrar el tercer diálogo
        yield return new WaitForSeconds(5f);

        // Crear y mostrar el tercer diálogo
        Dialogue thirdDialogue = new Dialogue
        {
            name = "Guía",
            sentences = new string[]
            {
                "Perfecto, ya estas listo para comenzar.",
                "Veo que tienes equipada tu arma, el primer objetivo debería ser encontrar balas... No sabemos a qué peligros nos podemos estar enfrentando.",
                "Se dice que desde la caída del meteorito han estado apareciendo... criaturas.",
                "Aún no hay pruebas científicas de que sea cierto pero debemos ir con cuidado, no podemos correr riesgos.",
                "Ten cuidado también con el agua, no sabemos si ha sido infectada por culpa de la radiación.",
                "Ánimo y ten cuidado, la sociedad depende de ti."
            }
        };
        dialogueManager.StartDialogue(thirdDialogue);
    }
}
