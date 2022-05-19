using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public float maxHealth;
    public Slider healthbar;

    private float mHealth;

    public HeroController hero;

    private Animator mAnimator;

    private bool isRunning = false;

    public float speed;

    private void Start()
    {
        mHealth = maxHealth;
        mAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isRunning)
        {
            transform.position += speed * Time.deltaTime * Vector3.left;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            //Hubo una colision
            mHealth -= maxHealth * 0.05f;
            healthbar.value -= 0.05f;

            if (mHealth <= 0)
            {
                Destroy(gameObject);
            }

            hero.MagicBarUpdate(0.10f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Paso checkpoint");
        isRunning = true;
    }

}
