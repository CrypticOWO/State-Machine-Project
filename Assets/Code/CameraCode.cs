using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCode : MonoBehaviour
{
    public Rigidbody RB; // Rigidbody for the camera
    public GameObject TargetPlayer;
    public GameObject Angel;

    public float mouseSensitivity = 2f;
    float cameraVerticalRotation = 0f;
    float cameraHorizontalRotation = 0f;
    public float speed = 5f;

    public static string LookingAt;
    public static GameObject LookingAtObject;

    public enum PlayerStates {Menu, Idle, Walking, Crouching, Sprinting, Stunned};
    private PlayerStates State;

    public float StunnedTimer = 0;

    public AudioSource Footsteps;
    [SerializeField] private AudioClip[] Sounds;

    void Start()
    {
        mouseSensitivity = 2f;
        speed = 5f;
        RB.isKinematic = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        LookingAt = "Nothing";
        State = PlayerStates.Menu;  // Set State to Menu initially

        //transform.position = new Vector3(47, 2.2f, -3f);
        //transform.localEulerAngles = new Vector3(0, 0, 0);
        TargetPlayer.transform.position = transform.position + new Vector3(0, -3, 0);
        
        transform.position = new Vector3(47.135f, 2.08f, -1.89f);
        transform.localEulerAngles = new Vector3(0, -40, 0);
    }

    void Update()
    {
        if(State == PlayerStates.Menu)
        {
            MenuControls();
        }    
        else if(State == PlayerStates.Stunned)
        {
            StunnedControls();
        }
        else
        {
            NormalControls();
        }

    }

    void NormalControls()
    {
        RB.isKinematic = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        cameraHorizontalRotation += inputX;
        transform.localEulerAngles = new Vector3(cameraVerticalRotation, cameraHorizontalRotation, 0);

        CheckForInteractables();

        // Get input from the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Create a movement vector
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Adjust player's height based on their state
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.position = new Vector3(transform.position.x, 2, transform.position.z);
            SetState(PlayerStates.Crouching);
            ScaleSprintMeter();
            if (!Footsteps.isPlaying)
            {
                Footsteps.PlayOneShot(Sounds[0]);
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
            SetState(PlayerStates.Sprinting);
            ScaleSprintMeter();
            if (!Footsteps.isPlaying)
            {
                Footsteps.PlayOneShot(Sounds[1]);
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
            SetState(PlayerStates.Walking);
            ScaleSprintMeter();
            if (!Footsteps.isPlaying)
            {
                Footsteps.PlayOneShot(Sounds[2]);
            }
        }

        if (direction.magnitude >= 0.1f)
        {
            // Get the forward and right directions from the camera, ensuring y is zeroed
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;
            Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);

            // Calculate the final movement vector (normalized)
            Vector3 moveDirection = (forward * direction.z + right * direction.x).normalized;

            // Set the velocity directly to move the player with no gradual deceleration
            RB.velocity = new Vector3(moveDirection.x * speed, RB.velocity.y, moveDirection.z * speed);
        }
        else
        {
            // If no input, set the velocity to zero on the x and z axes, while maintaining the y velocity (for gravity)
            RB.velocity = new Vector3(0, RB.velocity.y, 0);

            if (!Input.GetKey(KeyCode.LeftControl))
            {
                transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
                SetState(PlayerStates.Idle);
            }
        }
    }


    IEnumerator PanToNewPosition(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        float time = 0f;

        // Hide cursor during cutscene
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }
    }

    void CheckForInteractables()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        LookingAt = "Nothing";

        if (Physics.Raycast(ray, out RaycastHit hit, 3))
        {
            LookingAtObject = hit.collider.gameObject;
            string HitTag = LookingAtObject.tag;
            LookingAt = HitTag;
        }
    }

    public void SetState(PlayerStates st)
    {
        if (State == st) return;
        State = st;
        if (State == PlayerStates.Menu)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (State == PlayerStates.Idle)
        {
            //Debug.Log(st);
            Footsteps.Stop();
        }
        if (State == PlayerStates.Walking)
        {
            //Debug.Log(st);
            speed = 5f;
            Footsteps.Stop();
        }
        if (State == PlayerStates.Crouching)
        {
            //Debug.Log(st);
            speed = 1.5f;
            Footsteps.Stop();
        }
        if (State == PlayerStates.Sprinting)
        {
            //Debug.Log(st);
            speed = 10f;
            Footsteps.Stop();
        }
        if (State == PlayerStates.Stunned)
        {
            //Debug.Log(st);
            StunnedTimer = 1;
            Footsteps.Stop();
        }
    }

    public void MenuControls()
    {
        if (Input.GetKey(KeyCode.P))
        {
            SetState(PlayerStates.Idle);
        }
    }

    public void StunnedControls()
    {
        StunnedTimer -= Time.deltaTime;
        if (StunnedTimer <= 0)
        {
            SetState(PlayerStates.Idle);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
       if (other.gameObject.CompareTag("Enemy"))
       {
            SetState(PlayerStates.Stunned);
            StartCoroutine(NeckSnap());
            //Death Sound and Effect
            Footsteps.PlayOneShot(Sounds[3]);
       }
    }

    IEnumerator NeckSnap()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        RB.isKinematic = false;

        // Calculate direction away from the enemy
        Vector3 directionAwayFromEnemy = transform.position - Angel.transform.position;
        directionAwayFromEnemy.y = 0;  // Optional: Keep the movement on the XZ plane only (ignore Y-axis)
        directionAwayFromEnemy.Normalize();  // Normalize the direction to move in a straight line

        // Move the object away from the enemy by a small distance (e.g., 0.5 units)
        transform.position += directionAwayFromEnemy * 0.5f;  // Adjust the distance as needed

        // Add 100 degrees to the Z-axis of the current rotation
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 100f);

        // Apply the new rotation over a short duration (0.3 seconds)
        yield return StartCoroutine(PanToNewPosition(transform.position, newRotation, 0.3f));

        SetState(PlayerStates.Menu);

        // Re-enable the collider after the animation finishes
        collider.enabled = true;
        RB.isKinematic = true;
    }

    void ScaleSprintMeter()
    {
        if(State == PlayerStates.Sprinting)
        {
            float scaleFactor = Mathf.Max(1f, 0.01f - Time.time);
            UIManager.SprintMeter.transform.localScale = new Vector3(scaleFactor, 0.5f, 1);
        }
        else
        {
            float scaleFactor = Mathf.Max(1f, 0.01f + Time.time);
            UIManager.SprintMeter.transform.localScale = new Vector3(scaleFactor, 0.5f, 1);
        }

    }
}
