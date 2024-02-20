using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fantocci : MonoBehaviour
{
    [SerializeField] BattleManager battleManager; //only difference between vr and keyboard


    //Text
    //[SerializeField] TextMeshProUGUI bossText;
    [SerializeField] DialogueTrigger dialogueTrigger;

    //Boss Music
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip[] songs;

    //Boss output
    string bossStance = "";
    string bossPose = "";

    int stanceIndex = 0;
    int poseIndex = 0;
    string[] anStances = new string[]{"StanceDifensivo", "StanceNeutrale", "StanceOffensivo"}; //will be replaced by scriptable object
    string[] anPoses = new string[]{"PoseSpock", "PosePace", "PoseMarcello", "PosePistola", "PoseBuchino"}; //will be replaced by scriptable object
    string[] anBodies = new string[]{"BodyDifensivo", "BodyNeutrale", "BodyOffensivo"}; //will be replaced by scriptable object
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
            inkIndex = 0;
            battleManager.firstMove = false;
            return;
        }
        if(bossHealth <= 0)
        {
            inkIndex = 4;
            //audio vittoria?
            return;
        }
        
        stanceIndex = 1;
        poseIndex = Random.Range(1, 4);
        anStance = anStances[stanceIndex];
        anPose = anPoses[poseIndex];
        anBody = anBodies[1];
        bossStance = stances[stanceIndex];
        bossPose = poses[poseIndex];

        switch(poseIndex)
        {
            case 1:
            inkIndex = 1;
            break;
            case 2:
            inkIndex = 2;
            break;
            case 3:
            inkIndex = 3;
            break;
            default:
            inkIndex = 0;
            break;
        }
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
