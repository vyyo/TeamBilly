using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.Playables;

public class InkExternalFunctions
{
    public void Bind(Story story, Animator emoteAnimator/*, PlayableDirector[] currentCutscenes*/)
    {
        story.BindExternalFunction("playEmote", (string emoteName) => PlayEmote(emoteName, emoteAnimator));
        //story.BindExternalFunction("playCutscene", (int cutsceneIndex) => PlayCutscene(cutsceneIndex, currentCutscenes));
    }

    public void Unbind(Story story) 
    {
        story.UnbindExternalFunction("playEmote");
    }

    public void PlayEmote(string emoteName, Animator emoteAnimator)
    {
        if (emoteAnimator != null) 
        {
            emoteAnimator.Play(emoteName);
        }
        else 
        {
            Debug.LogWarning("Tried to play emote, but emote animator was "
                + "not initialized when entering dialogue mode.");
        }
    }

    /*public void PlayCutscene(int cutsceneIndex, PlayableDirector[] currentCutscenes)
    {
        currentCutscenes[cutsceneIndex].Play();
    }*/
}
