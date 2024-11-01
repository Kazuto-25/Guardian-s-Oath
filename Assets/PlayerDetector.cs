using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public EnemyBehaviour enemyBehaviour;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyBehaviour.isAttacking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyBehaviour.isAttacking = false;
        }
    }
}
