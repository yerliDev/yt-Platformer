using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyScript : MonoBehaviour
{
    GameObject Player;
    Swordman swordmanScript;
    [SerializeField] public float maxHealth=100f;
    [SerializeField] public float currentHealth;
    private float damage;
    [SerializeField] Slider healthBar;

    [SerializeField] public float moveSpeed;

    void Start()
    {
        currentHealth = maxHealth;
        Player = GameObject.Find("Player_SwordMan");
        swordmanScript = Player.GetComponent<Swordman>();

        moveSpeed = 0.01f;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Player.transform.position, moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            updateHealth(damage);
        }
    }
   


    private void updateHealth(float damageTaken)
    {
        damage = swordmanScript.Damage;
        currentHealth -= damage;
        healthBar.value = currentHealth / maxHealth;
        if(currentHealth<=0)
        {
            Destroy(this.gameObject);
        }
    }
}
