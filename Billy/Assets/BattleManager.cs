using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //Player Controller
    [SerializeField] PlayerKeyboard playerKeyboard;

    //Stances/Poses arrays
    string[] stances = new string[]{"Null", "Defensive", "Neutral", "Offensive"}; //will be replaced by scriptable object
    string[] poses = new string[]{"Null", "Spock", "Peace", "Marcello", "Gun", "Ok"}; //will be replaced by scriptable object

    //Player and Boss input
    string playerStance = "";
    string oldplayerStance = "";
    string playerPose = "";
    string oldPlayerPose = "";
    string bossStance = "";
    string oldBossStance= "";
    string bossPose = "";
    string oldbossPose = "";

    //Health
    [SerializeField] int playerHealth = 30;
    [SerializeField] int bossHealth = 30;
    [SerializeField] int baseDMG = 2;
    int healStreak = 0;

    //Turns
    int currentTurn = 0;
    [SerializeField] int turnLimit = 10; //number of turns after which the player starts taking damage

    void Awake()
    {
        ClearInputs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //memorizes the turn's moves(in case boss needs them to calculate better moves), then clears the slots for current moves
    void ClearInputs()
    {
        oldplayerStance = playerStance;
        oldbossPose = playerPose;
        oldBossStance = bossStance;
        oldbossPose = bossPose;

        playerStance = "";
        playerPose = "";
        bossStance = "";
        bossPose = "";
    }

    void TurnUpdate()
    {
        currentTurn = currentTurn ++;
        if(currentTurn > turnLimit)
        {
            playerHealth = playerHealth - (currentTurn - turnLimit);
        }
        ClearInputs();
        Debug.Log("Player health: " + playerHealth);
        Debug.Log("Boss health: " + bossHealth);
    }

    void InitiateTurn()
    {
        playerKeyboard.enabled = true;
    }

    public void DmgCalc(string currentStance, string currentPose)
    {
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
        if(playerStance == stances[1] && bossStance == stances[1])
        {
            Heal();
        }
        else if(playerStance == stances[1])
        {
            currentDMG = currentDMG/2;
            DamageAssignment();
        }
        else if(playerStance == stances[3])
        {
            currentDMG = currentDMG*2;
            DamageAssignment();
        }
        else if(bossStance == stances[1])
        {
            currentDMG = currentDMG/2;
            DamageAssignment();
        }
        else if(bossStance == stances[3])
        {
            currentDMG = currentDMG*2;
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