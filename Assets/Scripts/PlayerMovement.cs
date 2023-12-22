using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    walk,
    attack,
    hit
    
}

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D thisRigidBody;  // The player's rigidBody component
    private Vector3 change;             // 3D vector representation of movement (we'll only use x, y axis)
    private Animator animator;          // The player's animator component
    public PlayerState currentState;

    private bool isAttackReady;
    public int attackCooldown;           // time in frames to attack again
    private int attackFrameCounter;      // attack cooldown counter
    public static Vector3 lastchange;    // tracks old non-zero change variable to help define direction of attack
    private float shootPointBias = 0.75f;   // Amount that defines shooting point
    public GameObject playerBulletPrefab;

    public float knockbackStrength;
    private float knockTime = 0.2f;             // time in seconds to move after knockback
    private bool iFrames = false;
    public int maxIFrames;
    private int iFrameCounter;

    public AudioSource audioSource;
    public AudioClip attackSound;
    

    void Start()
    {
        // Assign reference of player's components to the local variables
        thisRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentState = PlayerState.walk;

        // Default value before player moves for the first time
        lastchange = Vector3.zero;
        lastchange.y = -1;
    }


    void FixedUpdate()
    {
        if (change != Vector3.zero)
        {
            lastchange = change;
        }


        change = Vector3.zero;

        // gets input for movement with arrowKeys or WASD (either 0 or 1 for GetAxisRaw)
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Attack") && currentState != PlayerState.attack && isAttackReady)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk)
        {
            UpdateAnimationAndMove();

            // turns isAttackReady to true only after attackCooldown frames
            if (!isAttackReady)
            {
                if (attackFrameCounter < attackCooldown)
                {
                    attackFrameCounter++;
                }
                else if (attackFrameCounter >= attackCooldown)
                {
                    isAttackReady = true;
                }
            }
        }

        if (iFrames)
        {
            if (iFrameCounter >= maxIFrames)
            {
                iFrames = false;
            }
            iFrameCounter++;
        }
    }

    private IEnumerator AttackCo()
    {
        currentState = PlayerState.attack;

        // defines position where bullet will spawn depending on direction of player
        Vector3 shootingPoint = transform.position + lastchange * shootPointBias;
        PlayerShoot(shootingPoint);
        yield return null;
        isAttackReady = false;
        attackFrameCounter = 0;

        currentState = PlayerState.walk;
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();

            // update animator parameters to match movement
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void MoveCharacter()
    {
        thisRigidBody.MovePosition(transform.position + change * speed * Time.deltaTime);
    }

    public void PlayerShoot(Vector3 shootingPoint)
    {
        Instantiate(playerBulletPrefab, shootingPoint, transform.rotation);
        audioSource.PlayOneShot(attackSound, 2);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!iFrames)
            {
                StartCoroutine(knockbackEffect(other));
                iFrames = true;
                iFrameCounter = 0;
            }
        }
    }

    private IEnumerator knockbackEffect(Collider2D other) {
        currentState = PlayerState.hit;
        Vector3 enemyPosition = other.gameObject.transform.position;
        Vector3 knockbackDirection = transform.position - enemyPosition;
        Vector3 knockbackChange = Vector3.zero;

        if (knockbackDirection.x > 0)
        {
            knockbackChange.x = 1;
        }
        else if (knockbackDirection.x <= 0)
        {
            knockbackChange.x = -1;
        }

        if (knockbackDirection.y > 0)
        {
            knockbackChange.y = 1;
        }
        else if (knockbackDirection.y <= 0)
        {
            knockbackChange.y = -1;
        }

        thisRigidBody.MovePosition(transform.position + knockbackChange * knockbackStrength * Time.deltaTime);
        yield return new WaitForSeconds(knockTime);
        currentState = PlayerState.walk;

        
    }
    
}