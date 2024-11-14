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

    public AudioSource Footsteps;
    [SerializeField] private AudioClip[] Sounds;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Check if the player is looking at the enemy within the vision cone
        if (IsPlayerLookingAtEnemy())
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

    // Method to make the enemy move back and forth
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
