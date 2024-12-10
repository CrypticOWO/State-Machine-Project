using UnityEngine;

public class PlayerCode : MonoBehaviour
{
    public Transform TargetCamera;
    public GameObject Flashlight;

    public static bool FlashLightOn;

    void Start()
    {
        FlashLightOn = false;
        Flashlight.SetActive(false);
    }

    void Update()
    {
        transform.position = TargetCamera.transform.position + new Vector3(0,-1f, 0);

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (FlashLightOn == false)
            {
                Flashlight.SetActive(true);
                FlashLightOn = true;
            }
            else
            {
                Flashlight.SetActive(false);
                FlashLightOn = false;
            }
        }
    }
}
