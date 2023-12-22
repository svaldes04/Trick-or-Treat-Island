using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : Enemy
{
    public Transform target;
    public float attackRadius;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        Vector3 direction = target.position - transform.position;
        Vector3 animatorMove = Vector3.zero;

        if (direction.x > 0)
        {
            animatorMove.x = 1;
        }
        else if (direction.x <= 0)
        {
            animatorMove.x = -1;
        }

        if (direction.y > 0)
        {
            animatorMove.y = 1;
        }
        else if (direction.y <= 0)
        {
            animatorMove.y = -1;
        }

        animator.SetFloat("MoveX", animatorMove.x);
        animator.SetFloat("MoveY", animatorMove.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ScoreManager.instance.addPoint();
            Destroy(gameObject);
        }
        /*
        if (collision.gameObject.CompareTag("Enemy"))
        {

        }
        */
    }


}
