using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using TMPro;
using Oculus.Interaction.PoseDetection;

public class PoseDetector : MonoBehaviour
{
    public List<ActiveStateSelector> aSymbols;
    public List<ActiveStateSelector> aStances;
    [SerializeField] VRControls vrControls;
    public TextMeshProUGUI text;
    string newPose;
    string newStance;

    string[] stances = new string[]{"Null", "Difensiva", "Neutrale", "Offensiva"}; //will be replaced by scriptable object
    string[] poses = new string[]{"Null", "Spock", "Pace", "Marcello", "Gun", "Buchino"}; //will be replaced by scriptable object

    void Start()
    {
        foreach(var item in aSymbols)
        {
            /*item.WhenSelected += () => SetTextToPoseName(item.gameObject.name);
            item.WhenUnselected += () => SetTextToPoseName("");*/

            /*item.WhenSelected += () => newPose = item.gameObject.name;
            Debug.Log("THIS IS THE NEW POSE AAAAAAAAAAAAAH: " + newPose);
            item.WhenUnselected += () => newPose = "";
            SetTextToPoseName(newPose);*/
            //item.WhenSelected += () => poseCalc(item.gameObject.name);

            //vrControls.vrPoseInput(newPose);
        }

        foreach(var item in aStances)
        {
            /*item.WhenSelected += () => SetTextToPoseName(item.gameObject.name);
            item.WhenUnselected += () => SetTextToPoseName("");*/

            /*item.WhenSelected += () => newStance = item.gameObject.name;
            item.WhenUnselected += () => newStance = "";
            SetTextToPoseName(newStance);*/
            //item.WhenSelected += () => stanceCalc(item.gameObject.name);

            //vrControls.vrPoseInput(newStance);
        }
    }

    void SetTextToPoseName(string newText) //debug feature
    {
        text.text = newText;
    }

    void poseCalc(string newPose)
    {
        int i;
        for(i = 0; i == poses.Length; i++)
        {
            if( newPose == poses[i])
            {
                Debug.Log("Found pose" + i);
                vrControls.vrPoseInput(i);
            }
        }
    }

    void stanceCalc(string newStance)
    {
        int i;
        for(i = 0; i == stances.Length; i++)
        {
            if( newStance == poses[i])
            {
                Debug.Log("Found stance" + i);
                vrControls.vrPoseInput(i);
            }
        }
    }
}
