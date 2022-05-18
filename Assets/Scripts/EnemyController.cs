using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float maxHealth;
    public Slider healthbar;
    public Slider magicbar;

    private float mHealth;

    private event EventHandler mHero;

    private void Start()
    {
        mHealth = maxHealth;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            //Hubo una colision
            mHealth -= maxHealth * 0.25f;
            healthbar.value -= 0.25f;
            magicbar.value += 0.50f;

            if (mHealth <= 0)
            {
                Destroy(gameObject);
            }
            if (magicbar.value == 1)
            {
                mHero?.Invoke(this, EventArgs.Empty);
                magicbar.value = 0;
            }
        }
    }

    public void AddWarpDelegate(EventHandler handler)
    {
        mHero += handler;
    }


}
