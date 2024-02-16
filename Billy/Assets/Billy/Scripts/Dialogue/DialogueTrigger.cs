using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTrigger : MonoBehaviour
{

    [Header("Emote Animator")]
    [SerializeField] private Animator emoteAnimator;

    /*[Header("Cutscenes")]
    [SerializeField] private PlayableDirector[] cutscenes;*/

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] inkJSON;

    private bool playerInRange;

    private void Awake() 
    {
    }

    private void Update() 
    {
    }

    public void PlayDialogue(int dialogueIndex)
    {
        DialogueManager.GetInstance().PlayStory(inkJSON[dialogueIndex], emoteAnimator);
    }
}
