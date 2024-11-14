using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCode : MonoBehaviour
{
    public Rigidbody RB; // Rigidbody for the camera
    public GameObject TargetPlayer;

    public float mouseSensitivity = 2f;
    float cameraVerticalRotation = 0f;
    float cameraHorizontalRotation = 0f;
    public float speed = 5f;

    public string LookingAt;

    public enum PlayerStates {Menu, Idle, Walking, Crouching, Sprinting, Stunned};

    public float StunnedTimer = 0;

    // Declare State as a class-level variable
    private PlayerStates State;

    void Start()
    {
        mouseSensitivity = 2f;
        speed = 5f;
        RB.isKinematic = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        LookingAt = "Nothing";
        State = PlayerStates.Menu;  // Set State to Menu initially

        transform.position = new Vector3(8, 2.5f, 26);
        TargetPlayer.transform.position = transform.position + new Vector3(0, -3, 0);
        transform.localEulerAngles = new Vector3(0, -120, 0);
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

        // Check how fast the players are moving
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
            SetState(PlayerStates.Crouching);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
            SetState(PlayerStates.Sprinting);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
            SetState(PlayerStates.Walking);
        }

        if (direction.magnitude >= 0.1f)
        {
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);

            // Create the final movement vector
            Vector3 moveDirection = (forward * direction.z + right * direction.x).normalized;

            // Move the player using Rigidbody
            RB.MovePosition(RB.position + moveDirection * speed * Time.deltaTime);
        }
        else
        {
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

        if (Physics.Raycast(ray, out RaycastHit hit, 7))
        {
            GameObject HitObject = hit.collider.gameObject;
            string HitTag = HitObject.tag;
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
        }
        if (State == PlayerStates.Walking)
        {
            //Debug.Log(st);
            speed = 5f;
        }
        if (State == PlayerStates.Crouching)
        {
            //Debug.Log(st);
            speed = 1.5f;
        }
        if (State == PlayerStates.Sprinting)
        {
            //Debug.Log(st);
            speed = 10f;
        }
        if (State == PlayerStates.Stunned)
        {
            //Debug.Log(st);
            StunnedTimer = 1;
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
            //Death Sound and Effect
            //GameMasterCode.Singleton.PlaySoundFXClip(DeathSoundClip, transform, 1f);
            //Instantiate(PlayerDeathEffect, transform.position, transform.rotation);
            //Destroy(gameObject);
       }
    }
}
