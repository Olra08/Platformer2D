using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float accel;
    public float deccel;
    public float speedExp;

    [Header("Jump")]
    public float raycastDistance;
    public float jumpForce;
    public float fallMultiplier;
    private bool secondJump = false;

    [Header("Fire")]
    public GameObject fireball; // prefab
    private Transform mFireballPoint;

    private Rigidbody2D mRigidbody;
    private float mMovement;
    private Animator mAnimator;
    private AudioSource mAudioSource;
    public AudioClip fireballSound;

    private bool warpPower = false;

    public Slider magicbar;
    public Slider healthbar;
    public GameObject dead;
    public GameObject restart;

    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();
        mFireballPoint = transform.Find("FireballPoint");
    }


    private void Update()
    {
        mMovement = Input.GetAxis("Horizontal");
        mAnimator.SetInteger("Move", mMovement == 0f ? 0 : 1);

        if (mMovement < 0f)
        {
            transform.rotation = Quaternion.Euler(
                0f,
                180f,
                0f
            );
        } else if (mMovement > 0f)
        {
            transform.rotation = Quaternion.Euler(
                0f,
                0f,
                0f
            );
        }
        
        bool isOnAir = IsOnAir();

        if (Input.GetButtonDown("Jump") && !isOnAir)
        {
            Jump();
            secondJump = false;
        }

        if (Input.GetButtonDown("Jump") && isOnAir && !secondJump)
        {
            Jump();
            secondJump = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

        if (warpPower)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (transform.rotation.y == 0f)
                {
                    transform.position = new Vector3(
                    transform.position.x + 5,
                    transform.position.y,
                    0f
                    );
                } else
                {
                    transform.position = new Vector3(
                    transform.position.x - 5,
                    transform.position.y,
                    0f
                    );
                }
                warpPower = false;
                magicbar.value = 0f;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                warpPower = false;
                healthbar.value += 0.50f;
                magicbar.value = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        Move();

        if (mRigidbody.velocity.y < 0)
        {
            // Esta cayendo
            mRigidbody.velocity += (fallMultiplier - 1) *
                Time.fixedDeltaTime * Physics2D.gravity;
        }
    }

    private void Move()
    {
        float targetSpeed = mMovement * moveSpeed;
        float speedDif = targetSpeed - mRigidbody.velocity.x;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? accel : deccel;
        float movement = Mathf.Pow(
            accelRate * Mathf.Abs(speedDif),
            speedExp
        ) * Mathf.Sign(speedDif);

        mRigidbody.AddForce(movement * Vector2.right);
    }

    private void Jump()
    {
        mRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public bool IsOnAir()
    {
        Transform raycastOrigin = transform.Find("RaycastPoint");
        RaycastHit2D hit = Physics2D.Raycast(
            raycastOrigin.position,
            Vector2.down,
            raycastDistance
        );
        mAnimator.SetBool("IsJumping", !hit);
        return !hit;
    }

    private void Fire()
    {
        mAudioSource.clip = fireballSound;
        mAudioSource.Play();
        mFireballPoint.GetComponent<ParticleSystem>().Play(); // Ejecutamos Particle System
        GameObject obj = Instantiate(fireball, mFireballPoint);
        obj.transform.parent = null;
    }

    public Vector3 GetDirection()
    {
        return new Vector3(
            transform.rotation.y == 0f ? 1f : -1f,
            0f,
            0f
        );
    }

    public void MagicBarUpdate(float mValue)
    {
        magicbar.value += mValue;
        if (magicbar.value == 1)
        {
            warpPower = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            healthbar.value -= 0.15f;
            if (healthbar.value <= 0)
            {
                Destroy(gameObject);
                dead.SetActive(true);
                restart.SetActive(true);
            }
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            healthbar.value -= 0.40f;
            if (healthbar.value <= 0)
            {
                Destroy(gameObject);
                dead.SetActive(true);
                restart.SetActive(true);
            }
        }
        if (collision.gameObject.CompareTag("FireBallBoss"))
        {
            healthbar.value -= 0.25f;
            if (healthbar.value <= 0)
            {
                Destroy(gameObject);
                dead.SetActive(true);
                restart.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("VoidOut"))
        {
            Destroy(gameObject);
            dead.SetActive(true);
            restart.SetActive(true);
        }
    }
}
