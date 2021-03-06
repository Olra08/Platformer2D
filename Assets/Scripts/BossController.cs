using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public float maxHealth;
    public Slider healthbar;

    private float mHealth;

    public float aggroRange;
    public float speed;
    public float gravity;

    private Rigidbody2D mRigidbody;
    public HeroController hero;
    public Transform heroTransform;
    private float mMovement;
    private SpriteRenderer mSpriteRenderer;

    public GameObject fireballBoss; // prefab
    private Transform mFireballPoint1;
    private Transform mFireballPoint2;

    public GameObject door;
    public GameObject checkLeft;
    public GameObject checkRight;
    public GameObject bossImage;
    public GameObject bossHealthBar;
    public GameObject image;
    public GameObject heroHealthbar;
    public GameObject magicbar;
    public GameObject win;
    public GameObject restart;
    public GameObject doorVoidOut;

    private float timer = 0f;

    private void Start()
    {
        mHealth = maxHealth;
        mRigidbody = GetComponent<Rigidbody2D>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mFireballPoint1 = transform.Find("FireballPoint1");
        mFireballPoint2 = transform.Find("FireballPoint2");
    }
    private void Update()
    {
        mMovement = Input.GetAxis("Horizontal");
        if (hero != null)
        {
            float distToHero = Vector2.Distance(transform.position, heroTransform.position);
            if (distToHero < aggroRange)
            {
                ChaseHero();
                timer += Time.deltaTime;
                if (timer > 1.1f)
                {
                    Fire();
                    timer = 0f;
                }
            }
            else
            {
                StopChasingHero();
            }
        }
        if (hero == null)
        {
            StopChasingHero();
        }

        if (mMovement < 0f)
        {
            mSpriteRenderer.flipX = true;
            transform.rotation = Quaternion.Euler(
                0f,
                180f,
                0f
            );
        }
        else if (mMovement > 0f)
        {
            mSpriteRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(
                0f,
                0f,
                0f
            );
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            mHealth -= maxHealth * 0.02f;
            healthbar.value -= 0.02f;

            if (mHealth <= 0)
            {
                Destroy(gameObject);
                bossImage.SetActive(false);
                bossHealthBar.SetActive(false);
                image.SetActive(false);
                heroHealthbar.SetActive(false);
                magicbar.SetActive(false);
                win.SetActive(true);
                Destroy(hero);
                restart.SetActive(true);
            }

            hero.MagicBarUpdate(0.10f);
        }
    }
    private void ChaseHero()
    {
        if (transform.position.x < heroTransform.position.x)
        {
            mRigidbody.velocity = new Vector2(speed, gravity);
        }
        else
        {
            mRigidbody.velocity = new Vector2(-speed, gravity);
        }
    }

    private void StopChasingHero()
    {
        mRigidbody.velocity = new Vector2(0, gravity - 5);
    }

    private void Fire()
    {
        GameObject obj1 = Instantiate(fireballBoss, mFireballPoint1);
        GameObject obj2 = Instantiate(fireballBoss, mFireballPoint2);
        obj1.transform.parent = null;
        obj2.transform.parent = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
        {
            door.SetActive(true);
            doorVoidOut.SetActive(true);
            bossImage.SetActive(true);
            bossHealthBar.SetActive(true);
            checkLeft.SetActive(false);
            checkRight.SetActive(false);
        } 
    }

}
