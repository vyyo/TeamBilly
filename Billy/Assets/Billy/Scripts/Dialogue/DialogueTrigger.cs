using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Ink.Runtime;

public class DialogueTrigger : MonoBehaviour
{

    [Header("Animators(Body, Pose, Stance)")]
    [SerializeField] private Animator[] animators;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] audioClips;

    /*[Header("Cutscenes")]
    [SerializeField] private PlayableDirector[] cutscenes;*/

    [Header("Ink JSON")]
    [SerializeField] private TextAsset[] inkJSON;

    private void Awake() 
    {
    }

    private void Update() 
    {
    }

    public void PlayDialogue(int dialogueIndex)
    {
        DialogueManager.GetInstance().PlayStory(inkJSON[dialogueIndex], animators, audioClips);
    }
}
