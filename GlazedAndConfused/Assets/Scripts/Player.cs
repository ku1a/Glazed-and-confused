using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Movement related - horizontal and vertical
    float movement, speed;

    public bool godMode;
    // Adjustable values
    [Header("Movement")]
    public float jumpForce = 4f;
    public float moveSpeed = 2f;
    // Donut spinning animation
    float rotSpeed = 240f;
    float rotMultiplier = 2f;

    // Karma rules
    [Header("Karma")]
    public int maxKarma = 5;
    public int karmaScore = 10;
    int karma = 0;
    public int Karma { get { return karma; } }
    bool hasFreeHit;
    //Invincibility rules
    bool invulnerable;
    public float invincibleTime;
    float elapsed;

    // Horizontal boundary and grounding rules
    [Header("Ground Rules")]
    public float boundary_max;
    public float boundary_min;
    [SerializeField] LayerMask whatIsGround;
    public float groundCheckerRadius = 0.05f;
    public Transform grounder;

    bool isGrounded;
    bool isRotated;

    //References
    Rigidbody rb;
    GameManager gameManager;
    Animator animationlayer;
    Animator shieldAnim;

    [Header("References")]
    public Transform playerModel;
    public ParticleSystem charged, depleted, collection;
    public GameObject shield;

    //sound stuff
    AudioManager audioManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        animationlayer = gameObject.GetComponent<Animator>();
        shieldAnim = shield.GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        hasFreeHit = false;
        isRotated = false;
    }
    
    void Update()
    {
        // Check for which axis pressed then flip to it
        if (Input.GetButton("Horizontal"))
        {
            isRotated = true;
            gameManager.ChangeSpeed(0);
        }
        else if (Input.GetButton("Vertical"))
        {
            isRotated = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isRotated)
                isRotated = true;
            else isRotated = false;
        }


        // Invulnerablility
        if (elapsed < invincibleTime)
        {
            invulnerable = true;
        }
        else
        {
            invulnerable = false;
        }
        elapsed += Time.deltaTime;

        // Check grounding
        isGrounded = Physics.CheckSphere(grounder.position, groundCheckerRadius, whatIsGround);
        if (karma == maxKarma) hasFreeHit = true;

        if (!isRotated) playerModel.Rotate(0, 0, rotSpeed * Time.deltaTime);

        // Movement input
        InputSystem();

        // Animate turns
        Animate();

        // Shield adjustments
        shield.transform.rotation = Quaternion.Euler(0, -90f, 0);

        // Lying down - tba  
    }

    private void OnTriggerEnter(Collider other)
    {
        // Obstacles
        if (other.tag == "Obstacle" && !godMode)
        {
            // If karma meter full
            if (hasFreeHit)
            {
                karma = 0;
                elapsed = 0;
                hasFreeHit = false;
                shieldAnim.SetBool("maxKarma", false);
            }

            // Otherwise, Omae wa mou Shindeiru
            else if (!hasFreeHit && !invulnerable)
            {
                gameManager.Lose();
                audioManager.Play("death");
            }
        }

        // Karma collectible
        if (other.tag == "karma")
        {
            // Increment karma bar if not full
            if (karma < maxKarma)
            {
                karma++;
                audioManager.Play("collect");
            }
            // Otherwise, add to score instead
            else gameManager.ChangeScore(karmaScore);

            // Play particle effect for max karma reached
            if (karma == maxKarma)
            {
                shieldAnim.SetBool("maxKarma", true);
                audioManager.Play("maximumKarma");
            }

            particleeffect particle = other.GetComponent<particleeffect>();
            particle.playParticle();

            Destroy(other.gameObject);
        }

        if(other.tag == "kill")
        {
            gameManager.Lose();
        }

    }

    private void FixedUpdate()
    {
        Move(movement);
        Speed(speed);

        if (Input.GetButton("Jump") && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce), ForceMode.Impulse);
            audioManager.Play("jump");
        }
    }

    void InputSystem()
    {
        // Switching active keys based on mode
        if (isRotated) movement = Input.GetAxis("Horizontal");
        else if (!isRotated) speed = Input.GetAxisRaw("Vertical");
    }

    void Move(float movement)
    {
        if (isRotated)
        {
            // Apply movement
            transform.Translate(transform.forward * movement * moveSpeed * Time.fixedDeltaTime);
            // Apply spinning (Coded Animation)
            playerModel.Rotate(0, 0, movement * rotSpeed * rotMultiplier * Time.fixedDeltaTime);

            Vector3 clampedPos = transform.position;
            clampedPos.z = Mathf.Clamp(clampedPos.z, boundary_min, boundary_max);
            transform.position = clampedPos;
        }
    }

    void Speed(float movement)
    {
        if (!isRotated)
        {
            gameManager.ChangeSpeed(movement);
            // Apply spinning (Coded Animation)
            playerModel.Rotate(0, 0, movement * rotSpeed * rotMultiplier * Time.fixedDeltaTime);
        }
    }

    void Animate()
    {
        if (isRotated) animationlayer.SetBool("isFlipped", true);
        else animationlayer.SetBool("isFlipped", false);
    }
}
