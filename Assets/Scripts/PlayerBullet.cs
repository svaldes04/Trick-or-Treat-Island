using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    private Rigidbody2D thisRigidbody;
    public float speed;
    private bool hitTarget = false;
    public int maxLifetime;
    private int currentLifetime = 0;

    public float thrust;
    public static float knockTime = 0.1f;   // default hardcoded knockTime for playerBullet
    private Vector2 myDirection;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
        myDirection.x = PlayerMovement.lastchange.x;
        myDirection.y = PlayerMovement.lastchange.y;

        // DELTA TIMEE ADD PLEASE TODO
        thisRigidbody.velocity = myDirection * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hitTarget)
        {
            if (currentLifetime < maxLifetime)
            {
                currentLifetime++;
            }
            else if (currentLifetime >= maxLifetime)
            {
                Destroy(gameObject);
            }


        }   // add alt path if object is hit


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            hitTarget = true;
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            if (enemy != null)
            {
                /*
                Vector2 moveBack = myDirection * thrust;
                enemy.AddForce(moveBack, ForceMode2D.Impulse);
                StartCoroutine(KnockCo(enemy));
                */
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator KnockCo(Rigidbody2D enemy)
    {
        if (enemy != null) {
            yield return new WaitForSeconds(knockTime);
            enemy.velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }
}

