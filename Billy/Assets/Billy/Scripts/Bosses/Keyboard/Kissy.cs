using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Kissy : MonoBehaviour
{
    [SerializeField] BattleManager battleManager; //only difference between vr and keyboard


    //Text
    //[SerializeField] TextMeshProUGUI bossText;
    [SerializeField] DialogueTrigger dialogueTrigger;

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
        stanceIndex = Random.Range(0,stances.Length);
        poseIndex = Random.Range(0,poses.Length);
        anStance = anStances[stanceIndex];
        anPose = anPoses[poseIndex];
        anBody = anBodies[stanceIndex];
        bossStance = stances[stanceIndex];
        bossPose = poses[poseIndex];
    }

    void BossOutput()
    {
        //hint
        dialogueTrigger.PlayDialogue(0);
        // = "Non hai possibilità contro la mia" + bossStance + " " + bossPose + "!"

        //output
        battleManager.bossStance = bossStance;
        battleManager.bossPose = bossPose;
        battleManager.anStance = anStance;
        battleManager.anPose = anPose;
        battleManager.anBody = anBody;
    }
}
