using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void OnButtonClick()
    {
        UIManager.InGame = true;
    }
}
