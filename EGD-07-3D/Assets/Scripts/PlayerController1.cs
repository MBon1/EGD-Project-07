using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController1 : MonoBehaviour
{
    CharacterController characterController;    // Character Controller

    Vector3 hitNormal;

    public bool canMove = true;

    Vector3 movementDirection = Vector3.zero;   // Direction the player is moving
    [SerializeField] float speed = 5.5f;        // Player movement speed
    float maxSlope;
    bool onStableGround;

    [SerializeField] GameObject[] whisps;
    int score = 0;

    /*[SerializeField] GameObject bullet;
    [SerializeField] GameObject[] bullets = new GameObject[3];
    [SerializeField] int bulletCap = 3;
    int bulletStock = 0;*/

    /*Light light;
    float lightIntensity = 0;*/

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        /*light = GetComponentInChildren<Light>();
        lightIntensity = light.intensity;
        light.intensity = 0;*/
    }

    // Start is called before the first frame update
    void Start()
    {
        maxSlope = characterController.slopeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded and allowed to move.
        // If so, allow for the player to input movement controls.
        if (characterController.isGrounded)
        {
            if (canMove)
            {
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                float verticalInput = Input.GetAxisRaw("Vertical");

                movementDirection = new Vector3(horizontalInput, 0, verticalInput);
                movementDirection = transform.TransformDirection(movementDirection);
                movementDirection *= speed;
            }
            else
            {
                movementDirection = Vector3.zero;
            }
            // If the player is on a surface where the slope exceeds the maximum slope, 
            // push the player back to cause them to slide off
            if (!onStableGround)
            {
                float slopeAngle = 1f - hitNormal.y;
                float pushBackSpeed = -(Physics.gravity.y / 3 * 2 * slopeAngle);
                movementDirection.x += slopeAngle * hitNormal.x * pushBackSpeed;
                movementDirection.z += slopeAngle * hitNormal.z * pushBackSpeed;
            }
        }

        // Check if the player's colliding with a surface that causes the player to be on a slope
        // greater than the player's maximum slope
        if (Vector3.Angle(Vector3.up, hitNormal) > maxSlope)
        {
            onStableGround = false;
        }
        else
        {
            onStableGround = characterController.isGrounded;
        }

        // Apply Gravity
        movementDirection.y += Physics.gravity.y * Time.deltaTime;
        // Move the player
        characterController.Move(movementDirection * speed * Time.deltaTime);
    }

    /* Player Collision */
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Record the normal of what the player hit
        hitNormal = hit.normal;

        // Check if the player hit something
        if (hit != null)
        {
            string hitTag = hit.gameObject.tag;

            if (hitTag == "Whisp")
            {
                score++;
                Destroy(hit.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster"))
        {
            SceneLoader.LoadScene("GameOver");
        }
    }
}
