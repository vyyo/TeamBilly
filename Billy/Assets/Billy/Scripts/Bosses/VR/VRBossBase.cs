using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VRBossBase : MonoBehaviour
{
    [SerializeField] VRBattleManager vrBattleManager; //only difference between vr and keyboard

    //Text
    [SerializeField] TextMeshProUGUI bossText;

    //Boss output
    string bossStance = "";
    string bossPose = "";

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
        bossStance = stances[Random.Range(0,stances.Length)];
        bossPose = poses[Random.Range(0,poses.Length)];
    }

    void BossOutput()
    {
        //hint
        bossText.text = "Non hai possibilità contro la mia" + bossStance + " " + bossPose + "!";

        //output
        vrBattleManager.bossStance = bossStance;
        vrBattleManager.bossPose = bossPose;
    }
}
