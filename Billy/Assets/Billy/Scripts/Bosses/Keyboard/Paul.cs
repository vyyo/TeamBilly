using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paul : MonoBehaviour
{
    [SerializeField] BattleManager battleManager; //only difference between vr and keyboard


    //Text
    //[SerializeField] TextMeshProUGUI bossText;
    [SerializeField] DialogueTrigger dialogueTrigger;

    //Boss Music
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip[] songs;

    //Battle Variables
    int currentPhase = 1;
    float roll = 0;

    //Boss output
    string bossStance = "";
    string bossPose = "";

    int stanceIndex = 0;
    int poseIndex = 0;
    string[] anStances = new string[]{"StanceDifensivo", "StanceNeutrale", "StanceOffensivo"}; //will be replaced by scriptable object
    string[] anPoses = new string[]{"PoseSpock", "PosePace", "PoseMarcello", "PosePistola", "PoseBuchino"}; //will be replaced by scriptable object
    string[] anBodies = new string[]{"BodySpock", "BodyPace", "BodyMarcello", "BodyHidden", "BodyBuchino"}; //will be replaced by scriptable object
    string anStance = "";
    string anPose = "";
    string anBody = "";

    int inkIndex;

    //Data
    string oldplayerStance = "";
    string oldPlayerPose = "";
    string oldBossStance= "";
    string oldbossPose = "";
    int bossHealth;
    int playerHealth;
    int currentTurn;

    string[] stances = new string[]{"Defensive", "Neutral", "Offensive"}; //will be replaced by scriptable object
    string[] poses = new string[]{"Spock", "Peace", "Marcello", "Gun", "Ok"}; //will be replaced by scriptable object

    void Start()
    {
        musicSource.loop = true;
        musicSource.clip = songs[0];
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(battleManager.moveSent == false)
        {
            BossTurn();
        }
    }

    void BossTurn()
    {
        DataUpdate();
        BossLogic();
        BossOutput();
        //Stop calc
        battleManager.moveSent = true;
    }

    void DataUpdate()
    {
        oldplayerStance =  battleManager.oldplayerStance;
        oldPlayerPose = battleManager.oldPlayerPose;
        oldBossStance = battleManager.oldBossStance;
        oldbossPose = battleManager.oldbossPose;
        bossHealth = battleManager.bossHealth;
        playerHealth = battleManager.playerHealth;
        currentTurn = battleManager.currentTurn;
    }

    void BossLogic()
    {
        if(battleManager.firstMove)
        {
            inkIndex = 6;
            battleManager.firstMove = false;
            return;
        }
        if(bossHealth <= 0)
        {
            inkIndex = 0;
            //audio vittoria?
            return;
        }
        if(battleManager.invisible)
        {
            currentPhase = 1;
        }
        if(!battleManager.invisible)
        {
            currentPhase = 2;
        }

        roll = Random.value;
        PatternCalculation(roll, currentPhase);
        
        anStance = anStances[stanceIndex];
        anPose = anPoses[poseIndex];
        if(battleManager.invisible)
        {
            anBody = anBodies[3];
        }
        else
        {
            anBody = anBodies[poseIndex];
        }
        bossStance = stances[stanceIndex];
        bossPose = poses[poseIndex];
    }

    void PatternCalculation(float roll, int currentPhase)
    {
        if(currentPhase == 1)
        {
            if(roll <= 0.2f)
            {
                inkIndex = 0;
                stanceIndex = 0;
                poseIndex = 2;
            }
            else if(0.2f < roll && roll <= 0.4f)
            {
                inkIndex = 0;
                stanceIndex = 0;
                poseIndex = 4;
            }
            else if(0.4f < roll && roll <= 0.6f)
            {
                inkIndex = 0;
                stanceIndex = 0;
                poseIndex = 2;
            }
            else if(0.6f < roll && roll <= 0.8f)
            {
                inkIndex = 0;
                stanceIndex = 0;
                poseIndex = 1;
            }
            else if(0.8f < roll && roll <= 0.85f)
            {
                inkIndex = 1;
                stanceIndex = 1;
                poseIndex = 1;
            }
            else if(0.85f < roll && roll <= 0.9f)
            {
                inkIndex = 1;
                stanceIndex = 1;
                poseIndex = 4;
            }
            else if(0.9f < roll && roll <= 0.95f)
            {
                inkIndex = 1;
                stanceIndex = 1;
                poseIndex = 1;
            }
            else
            {
                inkIndex = 1;
                stanceIndex = 1;
                poseIndex = 2;
            }
        }
        else if(currentPhase == 2)
        {
            if(roll <= 0.2f)
            {
                inkIndex = 2;
                stanceIndex = 2;
                poseIndex = 4;
            }
            else if(0.2f < roll && roll <= 0.4f)
            {
                inkIndex = 3;
                stanceIndex = 2;
                poseIndex = 0;
            }
            else if(0.4f < roll && roll <= 0.7f)
            {
                inkIndex = 4;
                stanceIndex = 2;
                poseIndex = 2;
            }
            else
            {
                inkIndex = 5;
                stanceIndex = 2;
                poseIndex = 1;
            }
        }
        Debug.Log("Ink index: " + inkIndex + "Stance index: " + stanceIndex + "Pose index: " + poseIndex);
    }

    void BossOutput()
    {
        //hint
        dialogueTrigger.PlayDialogue(inkIndex);
        // = "Non hai possibilità contro la mia" + bossStance + " " + bossPose + "!"

        //output
        battleManager.bossStance = bossStance;
        battleManager.bossPose = bossPose;
        battleManager.anStance = anStance;
        battleManager.anPose = anPose;
        battleManager.anBody = anBody;
    }
}
