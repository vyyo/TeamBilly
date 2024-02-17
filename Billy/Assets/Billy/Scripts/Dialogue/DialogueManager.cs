using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]
    //[SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    //[SerializeField] private Animator portraitAnimator;
    //private Animator layoutAnimator;

    /*[Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;*/

    //CUTSCENES
    /*private PlayableDirector[] currentCutscenes;*/

    [Header("Audio")]
    [SerializeField] private DialogueAudioInfoSO defaultAudioInfo;
    [SerializeField] private DialogueAudioInfoSO[] audioInfos;
    [SerializeField] private bool makePredictable;
    private DialogueAudioInfoSO currentAudioInfo;
    private Dictionary<string, DialogueAudioInfoSO> audioInfoDictionary;
    private AudioSource audioSource;
    private AudioSource clipsAudioSource;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; } //dialogueIsPlaying can be read by other scripts, but not modified

    //private bool canContinueToNextLine;

    private Coroutine displayLineCoroutine;

    public static DialogueManager dialogueManagerInstance;

    private const string SPEAKER_TAG = "speaker";
    //private const string PORTRAIT_TAG = "portrait";
    //private const string LAYOUT_TAG = "layout";
    private const string AUDIO_TAG = "audio";

    //private DialogueVariables dialogueVariables;
    private InkExternalFunctions inkExternalFunctions;

    private void Awake()
    {
        //if there are no other DialogueManager instances, this is the instance to keep
        if(dialogueManagerInstance == null)
        {
            dialogueManagerInstance = this;
        }
        //if one instance has already been saved, destroy it, then keep this one
        else
        {
            Destroy(gameObject);
        }

        //dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        inkExternalFunctions = new InkExternalFunctions();

        audioSource = this.gameObject.AddComponent<AudioSource>();
        clipsAudioSource = this.gameObject.AddComponent<AudioSource>();
        currentAudioInfo = defaultAudioInfo;
    }

    public static DialogueManager GetInstance()
    {
        return dialogueManagerInstance;
    }

    private void Start()
    {
        /*dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);*/

        //get the layout animator
        //layoutAnimator = dialoguePanel.GetComponent<Animator>();

        //get all of the choices text
        /*choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }*/

        InitializeAudioInfoDictionary();
    }

    private void InitializeAudioInfoDictionary()
    {
        audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
        audioInfoDictionary.Add(defaultAudioInfo.id, defaultAudioInfo);
        foreach(DialogueAudioInfoSO audioInfo in audioInfos)
        {
            audioInfoDictionary.Add(audioInfo.id, audioInfo);
        }
    }

    private void SetCurrentAudioInfo(string id)
    {
        DialogueAudioInfoSO audioInfo = null;
        audioInfoDictionary.TryGetValue(id, out audioInfo);
        if(audioInfo != null)
        {
            this.currentAudioInfo = audioInfo;
        }
        else
        {
            Debug.LogWarning("Failed to find audio info for id: " + id);
        }
    }

    private void Update()
    {
        //return right away if dialogue isn't playing
        /*if(!dialogueIsPlaying)
        {
            return;
        }

        //handle continuing to the next line in the dialogue when submit is pressed
        if(canContinueToNextLine
        && currentStory.currentChoices.Count == 0 //this condition prevents the story from continuing before a choice is picked
        && Player.playerInstance.GetInteractPressed())
        {
            ContinueStory();
        }*/
    }

    /*public void EnterDialogueMode(TextAsset inkJSON, Animator emoteAnimator*//*, PlayableDirector[] cutscenes*//*)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        //currentCutscenes = cutscenes;

        //dialogueVariables.StartListening(currentStory);
        inkExternalFunctions.Bind(currentStory, emoteAnimator*//*, currentCutscenes*//*);

        //reset portrait, layout, and speaker
        nameText.text = "NONAME";
        portraitAnimator.Play("default");
        //layoutAnimator.Play("left");

        //ContinueStory();
    }*/

    /*private IEnumerator ExitDialogueMode()
    {
        //made into a coroutine, so that if any other action is mapped to the same
        //button as interacting, it won't be performed at the end of a dialogue
        yield return new WaitForSeconds(0.2f);

        //dialogueVariables.StopListening(currentStory);
        inkExternalFunctions.Unbind(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        //go back to default audio
        SetCurrentAudioInfo(defaultAudioInfo.id);
    }*/

    public void PlayStory(TextAsset inkJSON, Animator[] animators, AudioClip[] audioClips/*, PlayableDirector[] cutscenes*/)
    {
        //reset
        inkExternalFunctions.Unbind(currentStory);
        dialogueText.text = "";
        //go back to default audio
        SetCurrentAudioInfo(defaultAudioInfo.id);

        //enterdialogue() code
        currentStory = new Story(inkJSON.text);
        //dialogueIsPlaying = true;
        //dialoguePanel.SetActive(true);

        //currentCutscenes = cutscenes;

        //dialogueVariables.StartListening(currentStory);
        inkExternalFunctions.Bind(currentStory, animators, clipsAudioSource, audioClips/*, currentCutscenes*/);

        //reset portrait, layout, and speaker
        //nameText.text = "NONAME";
        //portraitAnimator.Play("default");
        //layoutAnimator.Play("left");

        //canContinueToNextLine = true;

        //continue() code
        while(currentStory.canContinue /*&& canContinueToNextLine*/)
        {
            Debug.Log("bububu" + currentStory.canContinue);
                //set text for the current dialogue line
                //dialogueText.text = currentStory.Continue(); this one makes everything appear all at once
                if(displayLineCoroutine != null)
                {
                    StopCoroutine(displayLineCoroutine); //prevents coroutine overlap
                }
                string nextLine = currentStory.Continue();
                //handle tags
                HandleTags(currentStory.currentTags);
                displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
        }

        /*else
        {
            StartCoroutine(ExitDialogueMode());
        }*/   
    }

    private IEnumerator DisplayLine(string line) //typewriter effect
    {
        //set the text to the full line, but set the visible characters to 0
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        //HideChoices();

        //canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        //display each letter one at a time
        foreach(char letter in line.ToCharArray())
        {
            //if the interact button is pressed, set visible characters to the line length right away
            /*if(Player.playerInstance.GetInteractPressed())
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }*/

            //check for rich text tag, if found, add it without waiting
            if(letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if(letter == '>') //end of tag
                {
                    isAddingRichTextTag = false;
                }
            }
            //else, show the next character and wait a small time
            else
            {
                PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        /*yield return new WaitForSeconds(5f);
        Debug.Log("gagugo");
        canContinueToNextLine = true;*/

        //display choices, if any, for this dialogue line
        //DisplayChoices();

        //canContinueToNextLine = true;
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        //set varialbes for the below based on our config
        AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
        int frequencyLevel = currentAudioInfo.frequencyLevel;
        float minPitch = currentAudioInfo.minPitch;
        float maxPitch = currentAudioInfo.maxPitch;
        bool stopAudioSource = currentAudioInfo.stopAudioSource;

        //play the sound based on the config
        if(currentDisplayedCharacterCount % frequencyLevel == 0)
        {
            if(stopAudioSource)
            {
                audioSource.Stop();
            }

            AudioClip soundClip = null;
            //create predictable audio from hashing
            if(makePredictable)
            {
                int hashCode = currentCharacter.GetHashCode();
                //sound clip
                int predictableIndex = hashCode % dialogueTypingSoundClips.Length;
                soundClip = dialogueTypingSoundClips[predictableIndex];
                //pitch
                int minPitchInt = (int) (minPitch * 100);
                int maxPitchInt = (int) (maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;
                //cannot divide by 0, so if there is no range then skip the selection
                if(pitchRangeInt == 0)
                {
                    audioSource.pitch = minPitch;
                }
                else
                {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predictablePitchInt / 100f;
                    audioSource.pitch = predictablePitch;
                }
            }
            //otherwise, randomize the audio
            else
            {
                //sound clip
                int randomIndex = Random.Range(0, dialogueTypingSoundClips.Length);
                soundClip = dialogueTypingSoundClips[randomIndex];
                //pitch
                audioSource.pitch = Random.Range(minPitch, maxPitch);
            }

            //playsound
            audioSource.PlayOneShot(soundClip);
        }
    }

    /*private void HideChoices()
    {
        foreach(GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }*/

    private void HandleTags(List<string> currentTags)
    {
        //loop through each tag and handle it accordingly
        foreach(string tag in currentTags)
        {
            //PARSE THE TAG

            //tag is split in two strings: 0. key(ex: portrait) and 1. value(characterName)
            string[] splitTag = tag.Split(":");
            //error log, in case there's more than two strings
            if(splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            //in case you forgot: trim removes empty spaces in a string
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //HANDLE THE TAG

            switch(tagKey)
            {
                case SPEAKER_TAG:
                    nameText.text = tagValue;
                    break;
                /*case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;*/
                /*case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;*/
                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in, but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    /*private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //checks if the UI can support the number of choices given
        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices (" + currentChoices.Count + ") were given than the UI can support.");
        }

        int index = 0;
        //enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        //go through the remaining choices the UI supports and make sure they're hidden
        for(int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }*/

    /*private IEnumerator SelectFirstChoice()
    {
        //forcibly sets the first choice due to EventSystem fuckery:
        //seems like ES first requires to be cleared, then a one-frame
        //wait before the currently selected object is set
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }*/

    /*public void MakeChoice(int choiceIndex)
    {
        if(canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            Player.playerInstance.GetInteractPressed();
            ContinueStory();
        }
    }*/

    /*public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if(variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }*/

    //temporary solution, called every time the app exits
    //eventually, do it somewhere else
    /*private void OnApplicationQuit()
    {
        if(dialogueVariables != null)
        {
            dialogueVariables.SaveVariables();
        }
    }*/
}
