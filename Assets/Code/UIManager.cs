using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI LookingAtText;
    public GameObject CompLight;
    public GameObject CompText;

    public GameObject Angel;
    public GameObject Player;
    public static bool InGame;
    public static bool GameOverBad;
    public static bool GameOverGood;

    public AudioSource UISounds;
    [SerializeField] private AudioClip[] Sounds;
    
    public GameObject StartMenu;
    public GameObject ComputerRatio;
    public TextMeshProUGUI ComputerRatioText;
    public GameObject ResetButton;

    public GameObject StaminaWhole;
    public GameObject BatteryWhole;
    public static Image SprintMeter;
    public static Image BatteryMeter;

    // Start is called before the first frame update
    void Start()
    {
        LookingAtText.text = " ";
        Angel.SetActive(false);

        StartMenu.SetActive(true);
        StaminaWhole.SetActive(false);
        BatteryWhole.SetActive(false);
        ResetButton.SetActive(false);
        ComputerRatio.SetActive(false);

        SprintMeter = StaminaWhole.transform.Find("Stamina")?.GetComponent<Image>();
        BatteryMeter = BatteryWhole.transform.Find("Battery")?.GetComponent<Image>();
        InGame = false;
        GameOverBad = false;
        GameOverGood = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InGame == true)
        {
            StartMenu.SetActive(false);    
            StaminaWhole.SetActive(true);
            BatteryWhole.SetActive(true);
            ComputerRatio.SetActive(true);
            ComputerRatioText.text = "Computers Off: " + GameMasterCode.OffComputers + " / 9";

            if (GameMasterCode.OnComputers == 0)
            {
                ComputerRatioText.text = "Exit Unlocked";
            }
        }
        
        if(CameraCode.LookingAt == "Desk")
        {
            CompLight = CameraCode.LookingAtObject.transform.Find("ScreenLight")?.gameObject;
            CompText = CameraCode.LookingAtObject.transform.Find("ComputerText")?.gameObject;

            if (GameMasterCode.OnComputers < 9 && CompLight.activeSelf)
            {
                LookingAtText.text = "Press E to turn off";
            }
            else if (GameMasterCode.OnComputers == 9 && CompLight.activeSelf && InGame == true)
            {
                TurnComputerOff();
            }
            else
            {
                LookingAtText.text = " ";
            }

            if (Input.GetKey(KeyCode.E) && CompLight.activeSelf)
            {
                TurnComputerOff();
            }
        }
        else if (CameraCode.LookingAt == "StartingDoor")
        {
            LookingAtText.text = "Press E to begin game";

            if (Input.GetKeyDown(KeyCode.E))
            {
                CameraCode.LookingAtObject.SetActive(false);
                Angel.SetActive(true);
                UISounds.PlayOneShot(Sounds[1]);
            }
        }
        else if (CameraCode.LookingAt == "ExitDoor")
        {
            if(GameMasterCode.OnComputers > 0)
            {
                LookingAtText.text = "Locked";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    UISounds.PlayOneShot(Sounds[2]);
                }
            }
            else
            {
                LookingAtText.text = "Press E to leave facility";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Angel.SetActive(false);
                    Player.transform.position = new Vector3(0,50,0);
                    UISounds.PlayOneShot(Sounds[1]);
                    
                    InGame = false;
                    GameOverGood = true;
                }
            }
        }
        else
        {
            LookingAtText.text = " ";
        }

        if (InGame == false && GameOverBad == true)
        {
            ResetButton.SetActive(true);
        }

        if (InGame == false && GameOverGood == true)
        {
            ResetButton.SetActive(true);
        }

        //SUPER CHEAT
        if (Input.GetKey(KeyCode.M))
        {
            GameMasterCode.OnComputers = 0;
        }
    }

    public void TurnComputerOff()
    {
        {
            CompLight.SetActive(false);
            CompText.SetActive(false);
            GameMasterCode.OnComputers--;
            GameMasterCode.OffComputers++;
            UISounds.PlayOneShot(Sounds[0]);
        }
    }
}
    

