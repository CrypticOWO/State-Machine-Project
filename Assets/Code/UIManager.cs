using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI LookingAtText;
    public GameObject CompLight;
    public GameObject CompText;

    public GameObject Angel;
    public GameObject Player;

    public AudioSource UISounds;
    [SerializeField] private AudioClip[] Sounds;
    
    public GameObject UIWhole;
    public static GameObject SprintMeter;
    public static GameObject BatteryMeter;

    // Start is called before the first frame update
    void Start()
    {
        LookingAtText.text = " ";
        Angel.SetActive(false);
        SprintMeter = UIWhole.transform.Find("Stamina")?.gameObject;
        BatteryMeter = UIWhole.transform.Find("Battery")?.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            //LookingAtText.SetActive(false);
        }
        
        if(CameraCode.LookingAt == "Desk")
        {
            CompLight = CameraCode.LookingAtObject.transform.Find("ScreenLight")?.gameObject;
            CompText = CameraCode.LookingAtObject.transform.Find("ComputerText")?.gameObject;

            if (GameMasterCode.OnComputers < 9 && CompLight.activeSelf)
            {
                LookingAtText.text = "Press E to turn off";
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
                    Player.transform.position = new Vector3(0,10,0);
                    UISounds.PlayOneShot(Sounds[1]);
                }
            }
        }
        else
        {
            LookingAtText.text = " ";
        }

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
            UISounds.PlayOneShot(Sounds[0]);
        }
    }
}
    

