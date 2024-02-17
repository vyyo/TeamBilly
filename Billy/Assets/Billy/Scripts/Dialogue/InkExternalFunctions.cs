using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.Playables;

public class InkExternalFunctions
{
    public void Bind(Story story, Animator[] animators, AudioSource audioSource, AudioClip[] audioClips/*, PlayableDirector[] currentCutscenes*/)
    {
        story.BindExternalFunction("playAnimation", (int animatorIndex, string animationName) => PlayAnimation(animatorIndex, animationName, animators));
        story.BindExternalFunction("playAudioClip", (int clipIndex) => PlayAudioClip(clipIndex, audioSource, audioClips));
        //story.BindExternalFunction("playCutscene", (int cutsceneIndex) => PlayCutscene(cutsceneIndex, currentCutscenes));
    }

    public void Unbind(Story story) 
    {
        story.UnbindExternalFunction("playAnimation");
    }

    public void PlayAnimation(int animatorIndex, string animationName, Animator[] animators)
    {
        if (animators[animatorIndex] != null) 
        {
            animators[animatorIndex].Play(animationName);
        }
        else 
        {
            Debug.LogWarning("Tried to play animation, but animation animator was "
                + "not initialized when entering dialogue mode.");
        }
    }

    public void PlayAudioClip(int clipIndex, AudioSource audioSource, AudioClip[] audioClips)
    {
        if (audioClips[clipIndex] != null) 
        {
            audioSource.PlayOneShot(audioClips[clipIndex]);
        }
        else 
        {
            Debug.LogWarning("Tried to play audioClip, but audioClip was "
                + "not initialized when entering dialogue mode.");
        }
    }

    /*public void PlayCutscene(int cutsceneIndex, PlayableDirector[] currentCutscenes)
    {
        currentCutscenes[cutsceneIndex].Play();
    }*/
}
