using System;
using UnityEngine;

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
    private SpriteRenderer mSpriteRenderer;

    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mFireballPoint = transform.Find("FireballPoint");
    }


    private void Update()
    {
        mMovement = Input.GetAxis("Horizontal");
        mAnimator.SetInteger("Move", mMovement == 0f ? 0 : 1);

        if (mMovement < 0f)
        {
            //mSpriteRenderer.flipX = true;
            transform.rotation = Quaternion.Euler(
                0f,
                180f,
                0f
            );
        } else if (mMovement > 0f)
        {
            //mSpriteRenderer.flipX = false;
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
        /*Color rayColor;
        if (hit)
        {
            rayColor = Color.red;
        }
        else
        {
            rayColor = Color.blue;
        }
        Debug.DrawRay(raycastOrigin.position, Vector2.down * raycastDistance, rayColor);*/

        return !hit;
        // return hit == null ? true : false;
    }

    private void Fire()
    {
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
}
