using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
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

    private void Start()
    {
        mHealth = maxHealth;
        mRigidbody = this.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (hero != null)
        {
            float distToHero = Vector2.Distance(transform.position, heroTransform.position);
            if (distToHero < aggroRange)
            {
                ChaseHero();
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            mHealth -= maxHealth * 0.25f;
            healthbar.value -= 0.25f;

            if (mHealth <= 0)
            {
                Destroy(gameObject);
            }

            hero.MagicBarUpdate(0.50f);
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
    
}
