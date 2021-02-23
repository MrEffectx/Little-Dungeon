using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float invincibilityTimeAfterHit = 3f;
    public float healingTime = 0.25f;
    public float invincibilityFlashDelay = 0.2f;
    public float hasLowHeathValue = 1;

    public bool isLowHealthRedRunning = false;
    public bool isInvinsible = false;
    public bool hasLowHealth = false;
    public bool isHealing = false;

    public SpriteRenderer graphics;
    public HealthBar healthBar;

    public AudioClip hitSound;

    public static PlayerHealth instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerHealth dans la scène");
            return;
        }

        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(35);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            HealPlayer(20);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvinsible)
        {
            AudioManager.instance.PlayClipAt(hitSound, transform.position);
            currentHealth -= damage;

            if ((currentHealth < maxHealth / 5) && currentHealth > 0)
            {
                hasLowHealth = true;
                hasLowHeathValue = 1;
            }

            else if (currentHealth <= 0)
            {
                Die();
                return;
            } 

            else
            {
                hasLowHealth = false;
                hasLowHeathValue = 0;
                isLowHealthRedRunning = false;
            }

            healthBar.SetHealth(currentHealth);
            isInvinsible = true;
            StartCoroutine(InvicibilityFlash());
            StartCoroutine(HandleInvincibilityDelay());

          
        }

        if (hasLowHealth && (!isLowHealthRedRunning))
        {
            StartCoroutine(LowHealthRed());
        }

    }

    public void Die()
    {
        Debug.Log("Le joueur est éliminé)");
        PlayerMovement.instance.enabled = false; //desactive le sprict player movement
        PlayerMovement.instance.animator.SetTrigger("Die");
        PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Kinematic;
        PlayerMovement.instance.rb.velocity = Vector3.zero;
        PlayerMovement.instance.playerCollider.enabled = false;
        GameOverManager.instance.OnPlayerDeath();
    }

    public void Respawn()
    {
        Debug.Log("Le joueur est spawn)");
        PlayerMovement.instance.enabled = true; //desactive le sprict player movement
        PlayerMovement.instance.animator.SetTrigger("Respawn");
        PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Dynamic;
        PlayerMovement.instance.playerCollider.enabled = true;
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }

    public void HealPlayer(int amount)
    {
        if ((currentHealth + amount) > maxHealth)
        {
            currentHealth = maxHealth;
        } 
        else
        {
            currentHealth += amount; 
        }
            healthBar.SetHealth(currentHealth);
            isHealing = true;
            StartCoroutine(Healing());
            StartCoroutine(HandleHealingDelay());

    }

    public IEnumerator InvicibilityFlash()
    {
        if (!hasLowHealth)
        {
            while (isInvinsible)
            {
                graphics.color = new Color(1f, 1f, 1f, 0f); //rendre transparent
                yield return new WaitForSeconds(invincibilityFlashDelay);
                graphics.color = new Color(1f, 1f, 1f, 1f); //rendre opaque
                yield return new WaitForSeconds(invincibilityFlashDelay);
            }

        }
        else
        {
            while (isInvinsible)
            {
                graphics.color = new Color(1f, 1f, 1f, 0f); //rendre transparent
                yield return new WaitForSeconds(invincibilityFlashDelay);
                graphics.color = new Color(1f, 0.5f, 0.5f, 1f); //rendre opaque
                yield return new WaitForSeconds(invincibilityFlashDelay);
            }
        }
    }

    public IEnumerator Healing()
    {
            while (isHealing)
            {
                graphics.color = new Color(0.5f, 1f, 1f, 1f); //rendre transparent
                yield return new WaitForSeconds(healingTime);
                graphics.color = new Color(0.75f, 1f, 1f, 1f); //rendre transparent
                yield return new WaitForSeconds(healingTime);
                graphics.color = new Color(1f, 1f, 1f, 1f); //rendre opaque
                yield return new WaitForSeconds(healingTime);
                graphics.color = new Color(0.75f, 1f, 1f, 1f); //rendre transparent
                yield return new WaitForSeconds(healingTime);
                
        }

    }

    public IEnumerator LowHealthRed()
    {
        while (hasLowHealth)
        {
            isLowHealthRedRunning = true;
            yield return new WaitForSeconds(0.2f);
            graphics.color = new Color(1f, 0.5f, 0.5f, 1f); //rendre rouge
            yield return new WaitForSeconds(0.2f);
            graphics.color = new Color(1f, 0.75f, 0.75f, 1f); //rendre rouge
            yield return new WaitForSeconds(0.2f);
            graphics.color = new Color(1f, 1f, 1f, 1f); //rendre opaque
            yield return new WaitForSeconds(0.2f);
            graphics.color = new Color(1f, 0.75f, 0.75f, 1f); //rendre opaque
            yield return new WaitForSeconds(0.2f);
            yield return new WaitWhile(()=> hasLowHeathValue < 1);
        }
    }

    public IEnumerator HandleInvincibilityDelay()
    {
        yield return new WaitForSeconds(invincibilityTimeAfterHit);
        isInvinsible = false;

    }

    public IEnumerator HandleHealingDelay()
    {
        yield return new WaitForSeconds(invincibilityTimeAfterHit);
        graphics.color = new Color(1f, 1f, 1f, 1f); //rendre opaque
        isHealing = false;

    }
}
