using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float maxHealth;
    public Slider healthbar;

    private float mHealth;

    private bool isRunning = false;
    public float speed;

    private Rigidbody2D mRigidbody;
    public HeroController hero;
    public Transform heroTransform;
    private Vector2 movement;

    private void Start()
    {
        mHealth = maxHealth;
        mRigidbody = this.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (isRunning && hero != null)
        {
            Vector3 heroPosition = new Vector3(heroTransform.position.x, transform.position.y, 0f);
            Vector3 position = new Vector3(transform.position.x, 0f, 0f);
            Vector3 direction = heroPosition - position;
            direction.Normalize();
            movement = direction;
        }
        if (isRunning && hero == null)
        {
            speed = 0f;
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

    private void FixedUpdate()
    {
        MoveEnemy(movement);
    }

    private void MoveEnemy(Vector2 direction)
    {
        mRigidbody.MovePosition((Vector2)transform.position + (speed * Time.deltaTime * direction));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Paso checkpoint");
        isRunning = true;
    }
}
