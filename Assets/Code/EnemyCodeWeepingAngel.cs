using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCodeWeepingAngel : MonoBehaviour
{
    public float moveDistance;
    public float moveSpeed;
    public float coneAngle;
    public float maxRaycastDistance;
    public LayerMask gazeLayerMask;
    private Vector3 startPosition;

    public enum WeepingAngelStates {Walking, Frozen};
    public WeepingAngelStates State;

    public Light Flashlight;
    public AudioSource Footsteps;
    [SerializeField] private AudioClip[] Sounds;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Check if the player is looking at the enemy within the vision cone
        if ((IsPlayerLookingAtEnemy() && GameMasterCode.FacilityLightsOn == true) || IsInFlashlightBeam())
        {
            SetState(WeepingAngelStates.Frozen);
        }
        else
        {
            SetState(WeepingAngelStates.Walking);
            Moving();
        }
    }

    public void SetState(WeepingAngelStates st)
    {
        if (State == st) return;
        State = st;
        if (State == WeepingAngelStates.Walking)
        {
            //Walking Yippee
        }
        if (State == WeepingAngelStates.Frozen)
        {
            Footsteps.Stop();
            Footsteps.PlayOneShot(Sounds[1]);
        }
    }

    bool IsPlayerLookingAtEnemy()
    {
        // Get the direction from the camera to the enemy
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 toEnemy = transform.position - cameraPosition;

        // Calculate the angle between the camera's forward vector and the vector to the enemy
        float angle = Vector3.Angle(cameraForward, toEnemy);

        // Check if the enemy is within the cone's angle and within the max distance
        if (angle <= coneAngle && toEnemy.magnitude <= maxRaycastDistance)
        {
            return true;  // Player is looking at the enemy
        }
        return false; // Player is not looking at the enemy
    }

    bool IsInFlashlightBeam()
    {
        if (PlayerCode.FlashLightOn == false) return false;

        // Raycast from the flashlight towards the enemy's position
        Vector3 flashlightPosition = Flashlight.transform.position;
        Vector3 flashlightDirection = Flashlight.transform.forward;

        // Check if the enemy is within the flashlight's cone of light
        Vector3 toEnemy = transform.position - flashlightPosition;
        float distanceToEnemy = toEnemy.magnitude;

        // Perform a raycast to check if the enemy is within the flashlight's reach
        if (distanceToEnemy <= Flashlight.range) // Check if the enemy is within the light's range
        {
            // Check if the enemy is within the flashlight's cone (angle check)
            float angle = Vector3.Angle(flashlightDirection, toEnemy);

            if (angle <= Flashlight.spotAngle / 2)
            {
                return true; // The enemy is in the flashlight's beam
            }
        }
        return false; // The enemy is not in the flashlight's beam
    }

    void Moving()
    {
        // Calculate the movement based on a sine wave to make it oscillate
        float movement = Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        // Move the enemy back and forth from the start position
        transform.position = startPosition + new Vector3(movement, 0f, 0f);

        // Play the footstep sound while moving (only if it's not already playing)
        if (!Footsteps.isPlaying)
        {
            Footsteps.PlayOneShot(Sounds[0]);
        }
    }
}
