using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public int maxHealth = 5;
    public bool isInvulnerable;
    public bool hit;
    public float knockback = 0.3f;
    [SerializeField] private float iFramesDuration = 3.0f;
    [SerializeField] private int numberOfFlashes = 5;
    private SpriteRenderer spriteRend;
    public TryAgin tryAgin;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        spriteRend = GetComponent<SpriteRenderer>();
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int damage)
    {
        //audioManager.PlaySFX(audioManager.hurt);
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            tryAgin.gameOver();
        }
        else
        {
            StartCoroutine(Invulnerability());
        }
    }
    public IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        Physics2D.IgnoreLayerCollision(8, 9, true);
        StartCoroutine(Knockback());
        //yield return new WaitForSeconds(iFramesDuration);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
        isInvulnerable = false;
    }
    public IEnumerator Knockback()
    {
        hit = true;
        yield return new WaitForSeconds(knockback);
        hit = false;
    }
}
