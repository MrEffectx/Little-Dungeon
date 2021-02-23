using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    private bool isInRange;
    public bool isForPlayer;
    public bool isAutomatic = false;
    public bool isPlayerDialogueTrigger = false;

    private Text interactUI;

    private void Awake()
    {
        if(!isForPlayer)
        {
            interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<Text>();
        }
    }

    void Update()
    {
        if (!isForPlayer && ! isAutomatic)
        {
            if (isInRange && Input.GetKeyDown(KeyCode.E))
            {
                TriggerDialogue();
            }
        }
        else
        {
            if (isInRange && !isPlayerDialogueTrigger & !isAutomatic)
            {
                TriggerDialogue();
                isPlayerDialogueTrigger = true;
            }
        }

        if (isAutomatic && Input.GetKeyDown(KeyCode.Mouse0))
        {
            TriggerDialogue();
        }
        
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isForPlayer)
        {
            if (collision.CompareTag("Player"))
            {
                isInRange = true;
                interactUI.enabled = true;
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                isInRange = true;
            }
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isForPlayer)
        {
            if (collision.CompareTag("Player"))
            {
                isInRange = false;
                interactUI.enabled = false;
                DialogueManager.instance.EndDialogue();
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                isInRange = false;
                DialogueManager.instance.EndDialogue();
                isPlayerDialogueTrigger = false;
            }
        }

        
       
    }

    void TriggerDialogue()
    {
            Debug.Log("Dialogue triggered");
            DialogueManager.instance.StarDialogue(dialogue);    
    }
}
