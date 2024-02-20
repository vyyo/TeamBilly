using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VRBattleManager : MonoBehaviour
{
    //Player Controller
    [SerializeField] VRControls vrControls;
    //Audio
    [SerializeField] AudioSource damageSource;
    [SerializeField] AudioClip[] damageSFX;
    //Animators
    [Header("Animators(Body, Pose, Stance)")]
    [SerializeField] private Animator[] animators;
    public string  anBody;
    public string  anPose;
    public string anStance;

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
    public List<string> poseCombo = new List<string>();
    public List<string> anPoseCombo = new List<string>();

    //Health
    [SerializeField] public int playerHealth = 30;
    [SerializeField] public int bossHealth = 30;
    [SerializeField] public int baseDMG = 2;
    int healStreak = 0;

    //Turns
    public int currentTurn = 0;
    [SerializeField] int turnLimit = 10; //number of turns after which the player starts taking damage
    public bool moveSent = true;
    public bool firstMove = false;
    public bool phaseSwitch = false;
    public bool invisible = false;
    public bool ongoingCombo = false;
    int comboCounter = 0;
    [SerializeField] public int invisibleCounter = 0;
    [SerializeField] private float intervalloTurni = 3f;

    //Scene Management
    [SerializeField] int nextSceneIndex;

    void Awake()
    {
        firstMove = true;
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
        if(ongoingCombo)
        {
            Debug.Log("Player health: " + playerHealth);
            Debug.Log("Boss health: " + bossHealth);
            UIUpdate();
            StartCoroutine(Wait(intervalloTurni));
            return;
        }
        if(currentTurn > turnLimit)
        {
            playerHealth = playerHealth - (currentTurn - turnLimit);
        }
        currentTurn++;
        ClearInputs();
        Debug.Log("Player health: " + playerHealth);
        Debug.Log("Boss health: " + bossHealth);
        UIUpdate();
        if(firstMove)
        {
            InitiateTurn();
            //firstMove = false;
        }
        else
        {
            StartCoroutine(Wait(intervalloTurni));
        }
        //InitiateTurn();
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
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
        if(firstMove || phaseSwitch)
        {
            vrControls.enabled = false;
            moveSent = false;
            StartCoroutine(Wait(intervalloTurni * 3));
        }
        else if(ongoingCombo)
        {
            vrControls.enabled = false;
            DmgCalc(playerStance, playerPose);
            //StartCoroutine(Wait(intervalloTurni * 2));
        }
        else if(bossHealth <= 0)
        {
            moveSent = false;
            StartCoroutine(NextScene(intervalloTurni, nextSceneIndex));
        }
        else if(playerHealth <= 0)
        {
            StartCoroutine(NextScene(intervalloTurni, SceneManager.GetActiveScene().buildIndex)); //add gameover scene index
        }
        else
        {
            Debug.Log("WTF");
            vrControls.enabled = true;
            moveSent = false;
        }
    }

    IEnumerator NextScene(float time, int sceneIndex)
    {
        yield return new WaitForSeconds(time*2);
        SceneManager.LoadScene(sceneIndex);
    }

    public void DmgCalc(string currentStance, string currentPose)
    {
        
        animators[0].Play(anBody);
        //animators[1].Play(anPose);
        animators[2].Play(anStance);

        vrControls.enabled = false;

        bool win = false;
        int currentDMG;

        //receiving player input
        playerStance = currentStance;
        playerPose = currentPose;

        if(ongoingCombo && comboCounter < poseCombo.Count)
        {
            Debug.Log("combo");
            animators[1].Play(anPoseCombo[comboCounter]);
            bossPose = poseCombo[comboCounter];
            comboCounter ++;
            Calc();
        }
        else if(ongoingCombo && comboCounter >= poseCombo.Count)
        {
            Debug.Log("SAAAAAAAAAAAAAAAAAAS");
            poseCombo.Clear();
            anPoseCombo.Clear();
            comboCounter = 0;
            ongoingCombo = false;
            moveSent = false;
            ClearInputs();
            InitiateTurn();
            return;
        }
        else
        {
            animators[1].Play(anPose);
            Calc();
        }

        void Calc()
        {
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
            currentDMG = baseDMG;
            
            
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

            if(invisible && playerPose == poses[1])
            {
                invisibleCounter ++;
                if(invisibleCounter >=3)
                {
                    invisible = false;
                    animators[0].Play(anBody);
                }
                animators[0].Play("BodyFlicker");
            }
        }

       

        //Updates the loser's health according to the previous calculations
        void DamageAssignment()
        {
            if(win)
            {
                if(invisible && playerPose != poses[1])
                {
                    currentDMG = 0;
                }
                else
                {
                    bossHealth = bossHealth - currentDMG;
                    damageSource.PlayOneShot(damageSFX[1]);
                }
            }
            else
            {
                playerHealth = playerHealth - currentDMG;
                damageSource.PlayOneShot(damageSFX[0]);
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
