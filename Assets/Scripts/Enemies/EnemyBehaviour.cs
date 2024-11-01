using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Current States")]
    [SerializeField] private bool isAlive;
    [SerializeField] private float currentHealth;
    private float maxHealth;

    [Header("References")]
    [SerializeField] private Vector3 distanceFromQueen;
    [SerializeField] private Transform queen;
    private Collider2D enemyCollider;
    private Animator animator;
    private Vector3 targetPos;

    [Header("Movement Attribute")]
    [SerializeField] private bool canMove;
    [SerializeField] private float speed;


    private void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        isAlive = true;
        canMove = true;
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            enemyCollider.enabled = false;
            isAlive = false;
            Destroy(this.gameObject, 3f);
        }

        else
        {
            isAlive = true;
        }

        if (isAlive)
        {
            if (canMove)
            {
                Chase();
                PlayerDirection();
            }
        }

        else
        {
            canMove = false;
            animator.SetBool("isAlive", false);
        } 
    }

    private void Chase()
    {
        targetPos = queen.position - distanceFromQueen;

        float currentDistance = Vector3.Distance(transform.position, targetPos);

        if (currentDistance <= 0.1f)
        {
            animator.SetBool("isChasing", false);
            Attack();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            animator.SetBool("isChasing", true);
        }
    }

    private void Attack()
    {
        animator.SetTrigger("attack");
    }

    private void PlayerDirection()
    {
        if (queen.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLightAttack"))
        {
            currentHealth -= 20;
            StartCoroutine(gotHit());
        }

        else if(collision.gameObject.CompareTag("PlayerHeavyAttack"))
        {
             currentHealth -= 40;
            StartCoroutine(gotHit());
        }
    }

    private IEnumerator gotHit()
    {
        canMove = false;
        animator.SetTrigger("getHit");
        yield return new WaitForSeconds(0.25f);
        animator.ResetTrigger("getHit");
        canMove = true;
    }
}
