using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSilvio : MonoBehaviour
{
    [SerializeField] VRBattleManager vrBattleManager; //only difference between vr and keyboard
    
    //Text
    //[SerializeField] TextMeshProUGUI bossText;
    [SerializeField] DialogueTrigger dialogueTrigger;

    //Boss Music
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip[] songs;

    //Battle Variables
    [SerializeField] int phaseSwitchHP;
    [SerializeField] int finaleHP = 8;
    bool finaleHeal = false;
    int currentPhase = 1;
    float roll = 0;

    //Boss output
    string bossStance = "";
    string bossPose = "";
    [SerializeField] List<string> poseCombo = new List<string>();
    [SerializeField] List<string> anPoseCombo = new List<string>();

    int stanceIndex = 0;
    int poseIndex = 0;
    string[] anStances = new string[]{"StanceDifensivo", "StanceNeutrale", "StanceOffensivo"}; //will be replaced by scriptable object
    string[] anPoses = new string[]{"PoseSpock", "PosePace", "PoseMarcello", "PosePistola", "PoseBuchino"}; //will be replaced by scriptable object
    string[] anBodies = new string[]{"BodyNeutrale", "BodyHidden"}; //will be replaced by scriptable object
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
        if(vrBattleManager.moveSent == false)
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
        vrBattleManager.moveSent = true;
    }

    void DataUpdate()
    {
        oldplayerStance =  vrBattleManager.oldplayerStance;
        oldPlayerPose = vrBattleManager.oldPlayerPose;
        oldBossStance = vrBattleManager.oldBossStance;
        oldbossPose = vrBattleManager.oldbossPose;
        bossHealth = vrBattleManager.bossHealth;
        playerHealth = vrBattleManager.playerHealth;
        currentTurn = vrBattleManager.currentTurn;
    }

    void BossLogic()
    {
        finaleHeal = false;
        anPoseCombo.Clear();
        poseCombo.Clear();
        if(finaleHeal)
        {
            vrBattleManager.phaseSwitch = true;
            vrBattleManager.bossHealth = 10;
            finaleHeal = false;
            return;
        }
        if(vrBattleManager.firstMove)
        {
            //inkIndex = 0;
            vrBattleManager.firstMove = false;
            return;
        }
        if(bossHealth <= 0)
        {
            inkIndex = 9;
            //audio vittoria?
            return;
        }
        if(currentPhase == 1 && bossHealth <= phaseSwitchHP)
        {
            currentPhase = 2;
            vrBattleManager.phaseSwitch = true;
            return;
        }
        if(bossHealth <= finaleHP)
        {
            finaleHeal = true;
            return;
        }
        if(vrBattleManager.phaseSwitch)
        {
            inkIndex = 8;
            if(bossHealth >= phaseSwitchHP)
            {
                inkIndex = 0;
            }
            vrBattleManager.phaseSwitch = false;
            musicSource.clip = songs[1];
            musicSource.Play();
            return;
        }
        

        roll = Random.value;
        PatternCalculation(roll, currentPhase);
        
        anStance = anStances[stanceIndex];
        if(currentPhase == 1)
        {
            anBody = anBodies[0];
        }
        else if(currentPhase == 2)
        {
            anBody = anBodies[1];
        }

        if(vrBattleManager.invisible)
        {
            vrBattleManager.invisibleCounter = 2;
        }
        
        bossStance = stances[stanceIndex];
        if(!vrBattleManager.ongoingCombo)
        {
            anPose = anPoses[poseIndex];
            bossPose = poses[poseIndex];
        }
        if(bossHealth <= finaleHP)
        {
            stanceIndex = 2;
            bossStance = stances[stanceIndex];
        }
    }

    void PatternCalculation(float roll, int currentPhase)
    {
        if(currentPhase == 1)
        {
            if(roll <= 0.1f)
            {
                inkIndex = 1;
                stanceIndex = 1;
                poseIndex = 2;
            }
            else if(0.1f < roll && roll <= 0.2f)
            {
                inkIndex = 1;
                stanceIndex = 1;
                poseIndex = 3;
            }
            else if(0.2f < roll && roll <= 0.3f)
            {
                inkIndex = 1;
                stanceIndex = 1;
                poseIndex = 1;
            }
            else if(0.3f < roll && roll <= 0.45f)
            {
                inkIndex = 2;
                stanceIndex = 1;
                poseIndex = 4;
            }
            else if(0.45f < roll && roll <= 0.6f)
            {
                inkIndex = 2;
                stanceIndex = 1;
                poseIndex = 0;
            }
            else if(0.6f < roll && roll <= 0.8f)
            {
                inkIndex = 3;
                stanceIndex = 3;
                poseIndex = 0;
                vrBattleManager.invisible = true;
            }
            else
            {
                inkIndex = 4;
                stanceIndex = 2;
                poseCombo.Add(poses[3]);
                poseCombo.Add(poses[2]);
                poseCombo.Add(poses[0]);
                anPoseCombo.Add(anPoses[3]);
                anPoseCombo.Add(anPoses[2]);
                anPoseCombo.Add(anPoses[0]);
                vrBattleManager.ongoingCombo = true;
            }
        }
        else if(currentPhase == 2)
        {
            if(roll <= 0.1f)
            {
                inkIndex = 5;
                stanceIndex = 1;
                poseIndex = 2;
            }
            else if(0.1f < roll && roll <= 0.2f)
            {
                inkIndex = 5;
                stanceIndex = 1;
                poseIndex = 3;
            }
            else if(0.2f < roll && roll <= 0.3f)
            {
                inkIndex = 5;
                stanceIndex = 1;
                poseIndex = 1;
            }
            else if(0.3f < roll && roll <= 0.4f)
            {
                inkIndex = 5;
                stanceIndex = 0;
                poseIndex = 1;
            }
            else if(0.4f < roll && roll <= 0.5f)
            {
                inkIndex = 6;
                stanceIndex = 1;
                poseIndex = 4;
            }
            else if(0.5f < roll && roll <= 0.6f)
            {
                inkIndex = 6;
                stanceIndex = 1;
                poseIndex = 0;
            }
            else if(0.6f < roll && roll <= 0.7f)
            {
                inkIndex = 6;
                stanceIndex = 2;
                poseIndex = 4;
            }
            else if(0.7f < roll && roll <= 0.8f)
            {
                inkIndex = 6;
                stanceIndex = 0;
                poseIndex = 0;
            }
            else if(0.8f < roll && roll <= 0.9f)
            {
                inkIndex = 7;
                stanceIndex = 0;
                poseIndex = 3;
            }
            else
            {
                inkIndex = 7;
                stanceIndex = 2;
                poseIndex = 1;
            }
        }
        //Debug.Log("Ink index: " + inkIndex + "Stance index: " + stanceIndex + "Pose index: " + poseIndex);
    }

    void BossOutput()
    {
        //hint
        dialogueTrigger.PlayDialogue(inkIndex);
        // = "Non hai possibilità contro la mia" + bossStance + " " + bossPose + "!"

        //output
        vrBattleManager.bossStance = bossStance;
        vrBattleManager.bossPose = bossPose;
        vrBattleManager.anStance = anStance;
        vrBattleManager.anPose = anPose;
        vrBattleManager.anBody = anBody;
        vrBattleManager.poseCombo = poseCombo;
        vrBattleManager.anPoseCombo = anPoseCombo;
    }
}
