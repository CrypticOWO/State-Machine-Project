using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterCode : MonoBehaviour
{

    public static bool FacilityLightsOn;

    // Start is called before the first frame update
    void Start()
    {
        FacilityLightsOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (FacilityLightsOn == true)
            {
                //FacilityLights.SetActive(false);
                FacilityLightsOn = false;
            }
            else
            {
                //FacilityLights.SetActive(true);
                FacilityLightsOn = true;
            }
        }
    }
}
