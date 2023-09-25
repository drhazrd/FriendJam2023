using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;
     
    [Header("References")]
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public float textSpeed = 0.05f;

    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
    private bool isTyping = false;
    //private PlayerControls playerControls;

    
    void Awake(){
         if(DialogueSystem.instance == null){
            instance = this;
        } else if (DialogueSystem.instance != this && DialogueSystem.instance != null){
            Destroy(gameObject);
        }
        //playerControls = new PlayerControls();

    }
    
    void OnEnable(){
        //playerControls.Enable();
    }
    
    void OnDisable(){
        //playerControls.Disable();
    }
    
    void Start(){
        //playerControls.Player.Interact.performed += _ => ContinueDialogue();
        dialogueQueue = new Queue<DialogueLine>();
    }

    void ContinueDialogue(){
        if(!isTyping){
            if(dialogueQueue.Count > 0) {
                StartCoroutine(TypeDialogue(dialogueQueue.Dequeue()));
            }
            else 
            if(dialogueQueue.Count == 0){
                CompleteDialogue();
                return;
            } 
        }
    }
    void CompleteDialogue(){
        //End Dialogue
    }

    public void StartDialogue(DialogueSequence dialogue){
        dialogueQueue.Clear();
        foreach (var line in dialogue.lines){
            dialogueQueue.Enqueue(line);
        }
        StartCoroutine(TypeDialogue(dialogueQueue.Dequeue()));
    }

    IEnumerator TypeDialogue(DialogueLine line){
        isTyping = true;
        speakerText.text = line.speaker;
        dialogueText.text = "";
        
        if (line.speakingSFX != null) { 
            AudioManager.instance.PlayClip(line.speakingSFX);        }

        foreach (char c in line.text) {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false; 
    }
}