using UnityEngine;

public class PlayerCode : MonoBehaviour
{
    public Transform TargetCamera;

    void Start()
    {

    }

    void Update()
    {
        transform.position = TargetCamera.transform.position + new Vector3(0,-1f, 0);
    }
}
