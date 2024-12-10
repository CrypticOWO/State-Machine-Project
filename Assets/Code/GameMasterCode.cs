using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterCode : MonoBehaviour
{
    static public GameMasterCode instance;
    public static bool FacilityLightsOn;
    public GameObject FacilityLights;
    public static int OnComputers;

    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        FacilityLightsOn = true;
        FacilityLights.SetActive(true);
        OnComputers = 9;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (FacilityLightsOn == true)
            {
                FacilityLights.SetActive(false);
                FacilityLightsOn = false;
            }
            else
            {
                FacilityLights.SetActive(true);
                FacilityLightsOn = true;
            }
        }
    }

    IEnumerator LightFlicker()
    {
        FacilityLights.SetActive(false);
        FacilityLightsOn = false;
        yield return new WaitForSeconds(0.2f);
        FacilityLights.SetActive(true);
        FacilityLightsOn = true;
        yield return new WaitForSeconds(0.2f);
        FacilityLights.SetActive(false);
        FacilityLightsOn = false;
        yield return new WaitForSeconds(0.2f);
        FacilityLights.SetActive(true);
        FacilityLightsOn = true;
    }

    public static void DoLightFlicker()
    {
        instance.StartCoroutine(instance.LightFlicker());
    }
}