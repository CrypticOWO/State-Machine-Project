using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : MonoBehaviour
{
    public GameObject SoundSliders;
    public GameObject StartingText;
    
    public void OnButtonClick()
    {
        SoundSliders.SetActive(true);
        StartingText.SetActive(false);
    }
}
