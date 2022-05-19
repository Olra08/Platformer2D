using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float maxHealth;
    public Slider healthbar;

    private float mHealth;

    private bool isRunning = false;
    public float speed;

    public HeroController hero;

    private void Start()
    {
        mHealth = maxHealth;
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
            mHealth -= maxHealth * 0.25f;
            healthbar.value -= 0.25f;

            if (mHealth <= 0)
            {
                Destroy(gameObject);
            }

            hero.MagicBarUpdate(0.50f);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Paso checkpoint");
        isRunning = true;
        /*
        if (collision.gameObject.CompareTag("TriggerLeft"))
        {
            Debug.Log("Paso por izq");
            isRunning = true;
        }

        if (collision.gameObject.CompareTag("TriggerRight"))
        {
            Debug.Log("Paso por der");
            speed = speed * -1f;
            isRunning = true;
        }
        */
    }

}
