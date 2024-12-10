using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCodeWeepingAngel : MonoBehaviour
{
    public float moveDistance;
    public float moveSpeed;
    public float coneAngle;
    public float flashAngle;
    public float maxRaycastDistance;
    public LayerMask gazeLayerMask;
    private Vector3 startPosition;

    public enum WeepingAngelStates {Chasing, Frozen, Escaping, SuperChasing, Killing};
    public WeepingAngelStates State;

    public Light Flashlight;
    public AudioSource Footsteps;
    [SerializeField] private AudioClip[] Sounds;

    public Transform Player;
    public NavMeshAgent Agent;

    private float DistanceFromPlayer;
    public List<Transform> MovementNodes;

    public int Aggro;
    public float AggroTimer;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Aggro = 0;
    }

    void Update()
    {
        //Check Distance from Player
        DistanceFromPlayer = Vector3.Distance(Player.position, transform.position);

        // Check if the player is looking at the enemy within the vision cone
        if (State != WeepingAngelStates.Killing && (IsPlayerLookingAtEnemy() && GameMasterCode.FacilityLightsOn == true) || IsInFlashlightBeam())
        {
            SetState(WeepingAngelStates.Frozen);
        }
        else if(State == WeepingAngelStates.Killing)
        {
            Killing();
        }
        else
        {
            if (Aggro < 3 && (DistanceFromPlayer <= 15 || AggroTimer > 0))
            {
                SetState(WeepingAngelStates.Escaping);
                AggroTimer -= Time.deltaTime; 
            }
            else if (Aggro >= 3)
            {
                SetState(WeepingAngelStates.SuperChasing);
                AggroTimer -= Time.deltaTime; 
                Moving();
                if (AggroTimer <= 0)
                {
                    Aggro = 0;
                }
            }
            else
            {
                SetState(WeepingAngelStates.Chasing);
                Moving();
            }
        }
    }

    public void SetState(WeepingAngelStates st)
    {
        if (State == st) return;
        State = st;
        if (State == WeepingAngelStates.Chasing)
        {
            //Chasing Yippee
            
        }
        if (State == WeepingAngelStates.Frozen)
        {
            Agent.destination = transform.position;
            Aggro = 3;
            Footsteps.Stop();
            Footsteps.PlayOneShot(Sounds[1]);
        }
        if (State == WeepingAngelStates.Escaping)
        {
            GameMasterCode.DoLightFlicker();
            Aggro++;
            AggroTimer = GameMasterCode.OnComputers;
            RunAwayFromPlayer();
        }
        if (State == WeepingAngelStates.SuperChasing)
        {
            AggroTimer = 5;
        }
        if (State == WeepingAngelStates.Killing)
        {
            //ehe
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
            // Check for obstructions (like walls) between the player and the enemy
            RaycastHit hit;
            if (Physics.Raycast(cameraPosition, toEnemy.normalized, out hit, maxRaycastDistance, gazeLayerMask))
            {
                if (hit.collider.gameObject != gameObject)
                {
                    // There is an obstruction between the player and the enemy
                    return false;
                }
            }
            return true;  // Player is looking at the enemy with no obstructions
        }
        return false; // Player is not looking at the enemy
    }

    bool IsInFlashlightBeam()
    {
        if (PlayerCode.FlashLightOn == false) return false;

        // Get the direction from the camera to the enemy
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 toEnemy = transform.position - cameraPosition;

        // Calculate the angle between the camera's forward vector and the vector to the enemy
        float angle = Vector3.Angle(cameraForward, toEnemy);

        // Check if the enemy is within the cone's angle and within the max distance
        if (angle <= flashAngle && toEnemy.magnitude <= 10.475f)
        {
            // Check for obstructions (like walls) between the player and the enemy
            RaycastHit hit;
            if (Physics.Raycast(cameraPosition, toEnemy.normalized, out hit, 10.475f, gazeLayerMask))
            {
                if (hit.collider.gameObject != gameObject)
                {
                    // There is an obstruction between the player and the enemy
                    return false;
                }
            }
            return true;  // Player is looking at the enemy with no obstructions
        }
        return false; // Player is not looking at the enemy
    }

    void Moving()
    {
        // Calculate the movement based on a sine wave to make it oscillate
        Agent.destination = Player.position;

        // Play the footstep sound while moving (only if it's not already playing)
        if (!Footsteps.isPlaying)
        {
            Footsteps.PlayOneShot(Sounds[0]);
        }
    }

    void RunAwayFromPlayer()
    {
        float FurthestDistanceSoFar = 0;
        Vector3 FarNode = Vector3.zero;

        foreach (Transform Node in MovementNodes)
        {
            float CheckCurrentDistance = Vector3.Distance(Player.position, Node.position);
            if (CheckCurrentDistance > FurthestDistanceSoFar)
            {
                FurthestDistanceSoFar = CheckCurrentDistance;
                FarNode = Node.position;
            }
        }

        //Set the right destination for the furthest spot
        Agent.destination = FarNode;

        if (!Footsteps.isPlaying)
        {
            Footsteps.PlayOneShot(Sounds[0]);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
       if (other.gameObject.CompareTag("MainCamera"))
       {
            SetState(WeepingAngelStates.Killing);
            Agent.speed = 0;
            Agent.acceleration = 0;
       }
    }

    public void Killing()
    {
        
    }
}