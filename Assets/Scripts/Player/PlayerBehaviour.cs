using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Common Attributes")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool gameWin;
    [SerializeField] private bool isAlive;
    [SerializeField] private float x_Limit = 20f;
    [SerializeField] private float y_Limit = 14f;
    [SerializeField] private int maxHealth;

    [Header("Knight Attributes")]
    [SerializeField] private GameObject knight;
    [SerializeField] private Animator knightAnimator;
    [SerializeField] private Rigidbody2D knightRb;
    [SerializeField] private float heavyAttackForce;
    [SerializeField] private bool canMove;
    [SerializeField] private int knightHealth;
    private bool knightFacingRight;
    private Transform knightTransform;

    [Header("Princess Attributes")]
    [SerializeField] private GameObject princess;
    [SerializeField] private Animator princessAnimator;
    [SerializeField] private int princessHealth;
    private Transform princessTransform;

    private Vector2 knightMovementInput;
    private Vector2 princessMovementInput;

    private void Start()
    {
        isAlive = true;
        maxHealth = 100;
        knightFacingRight = true;
        canMove = true;
        knightHealth = maxHealth;
        princessHealth = maxHealth;
        knightTransform = knight.transform;
        princessTransform = princess.transform;
    }

    private void Update()
    {
        if (knightHealth <= 0 || princessHealth <= 0)
        {
            isAlive = false;
            knightAnimator.SetBool("isAlive", false);
            princessAnimator.SetBool("isAlive", false);
        }
        else if (isAlive && canMove)
        {
            KnightMovement();
            PrincessMovement();
        }
    }

    public void OnKnightMove(InputAction.CallbackContext context)
    {
        knightMovementInput = context.ReadValue<Vector2>();
    }

    public void OnPrincessMove(InputAction.CallbackContext context)
    {
        princessMovementInput = context.ReadValue<Vector2>();
    }

    private void KnightMovement()
    {
        if (knightMovementInput != Vector2.zero)
        {
            knightAnimator.SetBool("isMoving", true);
            knightTransform.position = ClampPosition(knightTransform.position + (Vector3)(knightMovementInput * moveSpeed * Time.deltaTime));
            knightFacingRight = knightMovementInput.x > 0;
            knightTransform.localScale = new Vector3(knightFacingRight ? 1 : -1, 1, 1);
        }
        else
        {
            knightAnimator.SetBool("isMoving", false);
        }
    }

    private void PrincessMovement()
    {
        if (princessMovementInput != Vector2.zero)
        {
            princessAnimator.SetBool("isWalking", true);
            princessTransform.position = ClampPosition(princessTransform.position + (Vector3)(princessMovementInput * moveSpeed * Time.deltaTime));
            princessAnimator.SetBool("isFacingRight", princessMovementInput.x > 0);
        }
        else
        {
            princessAnimator.SetBool("isWalking", false);
        }
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, -x_Limit, x_Limit);
        position.y = Mathf.Clamp(position.y, -y_Limit, y_Limit);
        return position;
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            knightAnimator.SetTrigger("lightAttack");
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            knightAnimator.SetTrigger("heavyAttack");
            StartCoroutine(PerformAttack(heavyAttackForce, 0.2f));
        }
    }

    private IEnumerator PerformAttack(float attackForce, float attackDuration)
    {
        canMove = false;
        knightRb.AddForce((knightFacingRight ? Vector2.right : Vector2.left) * attackForce);
        yield return new WaitForSeconds(attackDuration);
        knightRb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        canMove = true;
    }
}
