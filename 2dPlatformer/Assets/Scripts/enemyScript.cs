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

    [SerializeField] public GameObject bloodParticle;

    private GameObject mainCamera;
    private ScreenShake screenShakeScript;

    [SerializeField] public GameObject damageText;
    [SerializeField] public GameObject audioDeath;
    [SerializeField] public GameObject audioHurt;

    void Start()
    {

        mainCamera = GameObject.Find("Main Camera");
        screenShakeScript = mainCamera.GetComponent<ScreenShake>();


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
            updateHealth();
            Vector2 collisionPoint = collision.transform.position;
            GameObject blood = Instantiate(bloodParticle, collisionPoint-new Vector2(-1,0), Quaternion.identity);
            Destroy(blood, 1f);
        }
    }
   


    private void updateHealth( )
    {
        damage = swordmanScript.Damage;
        currentHealth -= damage;
        healthBar.value = currentHealth / maxHealth;
        GameObject damageTextPop = Instantiate(damageText,transform.position + new Vector3(0,1.3f,0),Quaternion.identity);
        Destroy(damageTextPop,0.22f);
        if (currentHealth<=0)
        {
           Instantiate(audioDeath, transform.position , Quaternion.identity);
            screenShakeScript.isShaking = true;
            Destroy(this.gameObject);
        }
        else
        {
            Instantiate(audioHurt, transform.position, Quaternion.identity);
        }
    }
}
