using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerKeyboard : MonoBehaviour
{
    KeyboardControls keyboardControls;
    [SerializeField] BattleManager battleManager;

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
        stanceInput();
        poseInput();
        //Debug.Log(handStance);
        //Debug.Log(handPose);

        //This makes it so the player has to hold the pose for x seconds before the input is accepted. Changing pose resets the timer
        if(handStance && handPose)
        {
            heldTime -= Time.deltaTime;
            Debug.Log("Updating timer");
            inputText.text = currentStance + " " + currentPose + "attack confirmed in " + heldTime + "seconds!";
        }
        else
        {
            heldTime = attackConfirmationTimer;
            Debug.Log("Timer reset!");
            inputText.text = "";
        }

        if(heldTime <= 0f)
        {
            Debug.Log("PORCODIOOOOOOOOOOOO");
            Attack();
        }
    }

    void stanceInput()
    {
        if(keyboardControls.Keyboard.StanceDef.IsPressed() && (currentStance == stances[0] || currentStance == stances[1]))
        {
            handStance = true;
            currentStance = stances[1];
            //Debug.Log(currentStance + " Stance!");
        }
        else if(keyboardControls.Keyboard.StanceNeut.IsPressed() && (currentStance == stances[0] || currentStance == stances[2]))
        {
            handStance = true;
            currentStance = stances[2];
            //Debug.Log(currentStance + " Stance!");
        }
        else if(keyboardControls.Keyboard.StanceAtk.IsPressed() && (currentStance == stances[0] || currentStance == stances[3]))
        {
            handStance = true;
            currentStance = stances[3];
            //Debug.Log(currentStance + " Stance!");
        }
        else
        {
            handStance = false;
            currentStance = stances[0];
        }

        Debug.Log(currentStance + " Stance!");
    }

    void poseInput()
    {
        if(keyboardControls.Keyboard.PoseSpock.IsPressed() && (currentPose == poses[0] || currentPose == poses[1]))
        {
            handPose = true;
            currentPose = poses[1];
            //Debug.Log(currentPose + "!");
        }
        else if(keyboardControls.Keyboard.PosePace.IsPressed() && (currentPose == poses[0] || currentPose == poses[2]))
        {
            handPose = true;
            currentPose = poses[2];
            //Debug.Log(currentPose + "!");
        }
        else if(keyboardControls.Keyboard.PoseMarcello.IsPressed() && (currentPose == poses[0] || currentPose == poses[3]))
        {
            handPose = true;
            currentPose = poses[3];
            //Debug.Log(currentPose + "!");
        }
        else if(keyboardControls.Keyboard.PosePistola.IsPressed() && (currentPose == poses[0] || currentPose == poses[4]))
        {
            handPose = true;
            currentPose = poses[4];
            //Debug.Log(currentPose + "!");
        }
        else if(keyboardControls.Keyboard.PoseOk.IsPressed() && (currentPose == poses[0] || currentPose == poses[5]))
        {
            handPose = true;
            currentPose = poses[5];
            //Debug.Log(currentPose + "!");
        }
        else
        {
            handPose = false;
            currentPose = poses[0];
        }

        Debug.Log(currentPose + "!");
    }

    void Attack()
    {
        if(handStance && handPose)
        {
            Debug.Log("Attack confirmed!" + currentStance + " " + currentPose + "!");
            inputText.text = "Attack confirmed!" + currentStance + " " + currentPose + "!";
            battleManager.DmgCalc(currentStance, currentPose);
        }
        else
        {
            Debug.Log("Attack deleted!");
            inputText.text = "Attack deleted!";
        }
        heldTime = attackConfirmationTimer;
        this.enabled = false;
    }
}
