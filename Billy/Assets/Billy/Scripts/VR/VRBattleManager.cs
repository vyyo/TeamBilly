using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VRBattleManager : MonoBehaviour
{
    //Player Controller
    [SerializeField] VRControls vrControls;

    //Text
    [SerializeField] TextMeshProUGUI gameStateText;

    //Stances/Poses arrays
    string[] stances = new string[]{"Null", "Defensive", "Neutral", "Offensive"}; //will be replaced by scriptable object
    string[] poses = new string[]{"Null", "Spock", "Peace", "Marcello", "Gun", "Ok"}; //will be replaced by scriptable object

    //Player and Boss input
    public string playerStance = "";
    public string oldplayerStance = "";
    public string playerPose = "";
    public string oldPlayerPose = "";
    public string bossStance = "";
    public string oldBossStance= "";
    public string bossPose = "";
    public string oldbossPose = "";

    //Health
    [SerializeField] public int playerHealth = 30;
    [SerializeField] public int bossHealth = 30;
    [SerializeField] public int baseDMG = 2;
    int healStreak = 0;

    //Turns
    public int currentTurn = 0;
    [SerializeField] int turnLimit = 10; //number of turns after which the player starts taking damage
    public bool moveSent = true;

    void Awake()
    {
        TurnUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //memorizes the turn's moves(in case boss needs them to calculate better moves), then clears the slots for current moves
    void ClearInputs()
    {
        oldplayerStance = playerStance;
        oldPlayerPose = playerPose;
        oldBossStance = bossStance;
        oldbossPose = bossPose;

        playerStance = stances[0];
        playerPose = poses[0];
        bossStance = stances[0];
        bossPose = poses[0];
    }

    void TurnUpdate()
    {
        if(currentTurn > turnLimit)
        {
            playerHealth = playerHealth - (currentTurn - turnLimit);
        }
        currentTurn++;
        ClearInputs();
        Debug.Log("Player health: " + playerHealth);
        Debug.Log("Boss health: " + bossHealth);
        UIUpdate();
        InitiateTurn();
    }

    void UIUpdate()
    {
        gameStateText.text = "Boss HP: " + bossHealth + " Your HP: " + playerHealth + " Turn: " + currentTurn;
        if(bossHealth <= 0)
        {
            gameStateText.text = "YOU WON!";
        }
        else if(playerHealth <= 0)
        {
            gameStateText.text = "YOU LOST!";
        }
    }

    void InitiateTurn()
    {
        Debug.Log("turno nuovooooooooooooooo");
        vrControls.enabled = true;
        moveSent = false;
    }

    public void DmgCalc(string currentStance, string currentPose)
    {
        vrControls.enabled = false;

        bool win = false;

        //receiving player input
        playerStance = currentStance;
        playerPose = currentPose;

        //calculates the winner
        if(playerPose == bossPose)
        {
            TurnUpdate();
            return;
        }
        else if(playerPose == poses[1] && (bossPose == poses[2] || bossPose == poses[4]))
        {
            win = true;
        }
        else if(playerPose == poses[2] && (bossPose == poses[3] || bossPose == poses[5]))
        {
            win = true;
        }
        else if(playerPose == poses[3] && (bossPose == poses[1] || bossPose == poses[4]))
        {
            win = true;
        }
        else if(playerPose == poses[4] && (bossPose == poses[2] || bossPose == poses[5]))
        {
            win = true;
        }
        else if(playerPose == poses[5] && (bossPose == poses[1] || bossPose == poses[3]))
        {
            win = true;
        }
        else
        {
            win = false;
        }

        //damage multiplier(stances)
        int currentDMG = baseDMG;
        
        
        if(playerStance == stances[1])
        {
            currentDMG = currentDMG/2;
        }
        else if(playerStance == stances[3])
        {
            currentDMG = currentDMG*2;
        }

        if(bossStance == stances[1])
        {
            currentDMG = currentDMG/2;
        }
        else if(bossStance == stances[3])
        {
            currentDMG = currentDMG*2;
        }

        if(playerStance == stances[1] && bossStance == stances[1])
        {
            Heal();
        }
        else
        {
            DamageAssignment();
        }

        //Updates the loser's health according to the previous calculations
        void DamageAssignment()
        {
            if(win)
            {
                bossHealth = bossHealth - currentDMG;
            }
            else
            {
                playerHealth = playerHealth - currentDMG;
            }
            healStreak = 0;
        }

        //As of now, the healStreak does not reset if a successful defense is followed by an opponent's successful defense
        //This effectively means the streak is carried over to the opponent, instead of being lost
        //Idk I find it to be a pretty cool thing
        void Heal()
        {
            currentDMG = 0;
            healStreak ++;
            if(win)
            {
                playerHealth = playerHealth + healStreak;
            }
            else
            {
                bossHealth = bossHealth + healStreak;
            }
        }

        TurnUpdate();
    }
}
