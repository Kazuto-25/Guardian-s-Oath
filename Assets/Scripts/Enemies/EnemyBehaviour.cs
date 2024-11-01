using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Current States")]
    [SerializeField] private bool isAlive;
    [SerializeField] private bool canChase;
    [SerializeField] private float currentHealth;
    private float maxHealth;

    [Header("References")]
    [SerializeField] private Transform queen;
    private Collider2D enemyCollider;
    private Animator animator;

    [Header("Movement Attribute")]
    [SerializeField] private bool canMove;
    [SerializeField] private float movementSpeed;
    private float usualSpeed;
    public bool isAttacking;  // Controlled by PlayerDetector

    [Header("Attack Range")]
    [SerializeField] private Vector2 stoppingDistance = new Vector2(1, 1);

    private void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        usualSpeed = 2;
        isAlive = true;
        canMove = true;
        canChase = true;
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            HandleDeath();
            return;
        }

        if (isAlive)
        {
            Attack();

            if(canMove)
            {
                Chase();
                PlayerDirection();
            }
        }
    }

    private void HandleDeath()
    {
        animator.SetBool("isAlive", false);
        canChase = false;
        enemyCollider.enabled = false;
        isAlive = false;
        Destroy(this.gameObject, 3f);
    }

    private void Chase()
    {
        if (canChase && !isAttacking)
        {
            movementSpeed = usualSpeed;
            Vector3 directionToPlayer = queen.position - transform.position;

            if (Mathf.Abs(directionToPlayer.x) > stoppingDistance.x || Mathf.Abs(directionToPlayer.y) > stoppingDistance.y)
            {
                MoveTowardsPlayer();
            }
            else
            {
                canMove = false;
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, queen.position, movementSpeed * Time.deltaTime);
        animator.SetBool("isChasing", true);
    }

    private void Attack()
    {
        if (isAttacking)
        {
            StartAttack();
        }
        else
        {
            StopAttack();
        }
    }

    private void StartAttack()
    {
        canChase = false;
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", true);
    }

    private void StopAttack()
    {
        animator.SetBool("isAttacking", false);
        StartCoroutine(WaitToChaseAgain(2f));
    }

    private IEnumerator WaitToChaseAgain(float timer)
    {
        yield return new WaitForSeconds(timer);
        canMove = true;
        canChase = true;
        animator.SetBool("isChasing", true);
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
            StartCoroutine(GotHit());
        }
        else if (collision.gameObject.CompareTag("PlayerHeavyAttack"))
        {
            currentHealth -= 40;
            StartCoroutine(GotHit());
        }
    }

    private IEnumerator GotHit()
    {
        canMove = false;
        animator.SetTrigger("getHit");
        yield return new WaitForSeconds(0.25f);
        animator.ResetTrigger("getHit");
        canMove = true;
    }
}
