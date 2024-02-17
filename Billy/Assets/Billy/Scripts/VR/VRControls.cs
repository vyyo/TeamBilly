using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VRControls : MonoBehaviour
{
KeyboardControls keyboardControls;
    [SerializeField] VRBattleManager vrBattleManager;

    //Input flags
    bool handStance = false;
    bool handPose = false;

    //Stances/Poses arrays
    string[] stances = new string[]{"Null", "Defensive", "Neutral", "Offensive"}; //will be replaced by scriptable object
    string[] poses = new string[]{"Null", "Spock", "Peace", "Marcello", "Gun", "Ok"}; //will be replaced by scriptable object

    //Current stance/pose
    string currentStance = "";
    string currentPose = "";

    //Input confirmation timer
    [SerializeField] float attackConfirmationTimer = 3f;
    float heldTime = 0f;

    //Text
    [SerializeField] TextMeshProUGUI inputText;

    void Awake()
    {
       keyboardControls = new KeyboardControls();
       keyboardControls.Keyboard.Enable();
       heldTime = attackConfirmationTimer;
       inputText.text = "";
    }

    void Update()
    {
        if(handStance && handPose)
        {
            heldTime -= Time.deltaTime;
            Debug.Log("Updating timer");
            inputText.text = currentStance + " " + currentPose + "attack confirmed in " + heldTime.ToString("F2") + "seconds!";
        }
        else
        {
            heldTime = attackConfirmationTimer;
            Debug.Log("Timer reset!");
            inputText.text = "";
        }

        if(heldTime <= 0f)
        {
            Attack();
        }
    }

    public void vrStanceInput(int newStance)
    {

        if(newStance == 0)
        {
            handStance = false;
        }
        else
        {
            handStance = true;
        }
        currentStance = stances[newStance];

        Debug.Log(currentStance + " Stance!");
    }

    public void vrPoseInput(int newPose)
    {

        if(newPose == 0)
        {
            handPose = false;
        }
        else
        {
            handPose = true;
        }
        currentPose = poses[newPose];

        Debug.Log(currentPose + " pose!");
    }

    public void vrPoseReset()
    {
        currentPose = poses[0];
        handPose = false;
        //Debug.Log("POSE RESET: " + handPose);
    }

    public void vrStanceReset()
    {
        currentStance = stances[0];
        handStance = false;
        //Debug.Log("STANCE RESET: " + handStance);
    }

    void Attack()
    {
        if(handStance && handPose)
        {
            Debug.Log("Attack confirmed!" + currentStance + " " + currentPose + "!");
            inputText.text = "Attack confirmed!" + currentStance + " " + currentPose + "!";
            heldTime = attackConfirmationTimer;
            vrBattleManager.DmgCalc(currentStance, currentPose);
        }
        else
        {
            Debug.Log("Attack deleted!");
            inputText.text = "Attack deleted!";
        }
    }
}
