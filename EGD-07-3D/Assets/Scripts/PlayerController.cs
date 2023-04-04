using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";

    float horizontalInput;
    float verticalInput;
    float currentMotorForce;
    float currentSteeringAngle;
    float currentBreakForce;

    [Header("Movement Variables")]
    [SerializeField] float motorForce = 5;
    [SerializeField] float breakForce = 5;
    [SerializeField] float maxSteeringAngle;
    [Space(10)]
    [SerializeField] AudioLoudnessDetection detection;
    [Space(10)]
    [SerializeField] bool useBalanceBoardControls = true;

    [Header("Wheels")]
    [SerializeField] WheelCollider frontLeftWheelCollider;
    [SerializeField] WheelCollider frontRightWheelCollider;
    [SerializeField] WheelCollider rearLeftWheelCollider;
    [SerializeField] WheelCollider rearRightWheelCollider;

    [Header("Player Race Data")]
    public int lapNumber = 1;
    public int checkpointIndex = 0;

    [Header("Collision and Health Data")]
    Rigidbody rb;
    [SerializeField] float minimumDamageThreshold = 1;
    [SerializeField] float collisionDamageScale = 1;
    [SerializeField] public float health = 5;
    float totalHealth = 0;
    [SerializeField] ScaleHP healthBar;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sandHitSFX;
    [SerializeField] string gameOverScene = "TitleScene";



    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();

        totalHealth = health;
        ScaleHealthBar();
    }

    void FixedUpdate()
    {
        /*Vector3 move = new Vector3(Input.GetAxisRaw(HORIZONTAL), 0, Input.GetAxisRaw(VERTICAL)).normalized;
        Vector3 velocity = move * motorForce * Time.deltaTime;*/
        GetInput();
        HandleMotor();
        HandleSteering();
    }

    void GetInput()
    {
        Vector2 input = Vector2.zero;
        if (useBalanceBoardControls)
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                input = new Vector2(Input.GetAxisRaw(VERTICAL) * -1, Input.GetAxisRaw(HORIZONTAL));
                /*horizontalInput = Input.GetAxisRaw(VERTICAL) * -1;
                verticalInput = Input.GetAxisRaw(HORIZONTAL);*/
                input.Normalize();
            }
        }
        else
        {
            input = new Vector2(Input.GetAxisRaw(HORIZONTAL), Input.GetAxisRaw(VERTICAL));
            /*horizontalInput = Input.GetAxisRaw(HORIZONTAL);
            verticalInput = Input.GetAxisRaw(VERTICAL);*/
        }

        horizontalInput = input.x;
        verticalInput = input.y;
    }

    void HandleMotor()
    {
        if (useBalanceBoardControls)
        {
            currentMotorForce = motorForce * Mathf.Lerp(0, verticalInput, detection.GetLoudnessFromMic());
        }
        else
        {
            currentMotorForce = motorForce * verticalInput;
        }
        frontLeftWheelCollider.motorTorque = currentMotorForce;
        frontRightWheelCollider.motorTorque = currentMotorForce;

        /*rearLeftWheelCollider.motorTorque = currentMotorForce;
        rearRightWheelCollider.motorTorque = currentMotorForce;*/

        // If force is in opposite direction than vertical input, breaking
        //currentBreakForce = 0;

    }

    void ApplyBreaking()
    {
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    void HandleSteering()
    {
        currentSteeringAngle = maxSteeringAngle * horizontalInput;

        frontLeftWheelCollider.steerAngle = currentSteeringAngle;
        frontRightWheelCollider.steerAngle = currentSteeringAngle;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Impact code from https://gamedev.stackexchange.com/questions/174710/measure-force-of-an-impact-to-deal-damage
        string tag = collision.gameObject.tag;
        if (tag == "Track" || tag == "Player" || tag == "CopyPlayer")
        {
            Vector3 impactVelocity = collision.relativeVelocity;

            // Subtracting a minimum threshold can avoid tiny scratches at negligible speeds.
            float magnitude = Mathf.Max(0f, impactVelocity.magnitude - minimumDamageThreshold);
            //Debug.Log("IMPACT MAG : " + impactVelocity.magnitude);

            // Using sqrMagnitude can feel good here,
            // making light taps less damaging and high-speed strikes devastating.
            float damage = magnitude * collisionDamageScale;
            health -= damage;
            ScaleHealthBar();

            if (health <= 0)
            {
                //Debug.Log("Game Over");
                SceneLoader.LoadSceneAndDestroyAudioInstance(gameOverScene);
            }
            else if (tag == "Track")
            {
                audioSource.clip = sandHitSFX;
                audioSource.Play();
            }
        }
    }

    void ScaleHealthBar()
    {
        healthBar.ScaleHPBar(health, totalHealth);
    }

    /// References: 
    ///     https://www.youtube.com/watch?v=F1JRy8nFTb4
    ///     https://www.youtube.com/watch?v=ehDRTdRGd1w
    ///     https://www.google.com/search?q=car+controls+unity&oq=car+controls+unity+&aqs=edge..69i57.2478j0j1&sourceid=chrome&ie=UTF-8#kpvalbx=_-Q8jZIqiH5uqptQP6sOcsAk_30
    ///     https://www.youtube.com/watch?v=IlqcaNkjMRY
    ///     https://www.youtube.com/watch?v=CBgtU9FCEh8
}
