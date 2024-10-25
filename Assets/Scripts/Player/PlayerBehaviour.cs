using System.Collections;
using UnityEditor;
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
    [SerializeField] private Animator knight_Animator;
    [SerializeField] private Rigidbody2D knight_rb;
    [SerializeField] private float heavyAttackForce;
    [SerializeField] private bool canMove;
    [SerializeField] private int knight_Health;
    private bool knightFacingRight;

    [Header("Princess Attributes")]
    [SerializeField] private GameObject princess;
    [SerializeField] private Animator princess_Animator;
    [SerializeField] private int princess_Health;

    private Vector2 knightMovementInput;
    private Vector2 princessMovementInput;

    private void Start()
    {
        isAlive = true;
        maxHealth = 100;
        knightFacingRight = true;
        canMove = true;

        knight_Health = maxHealth;
        princess_Health = maxHealth;
    }

    private void Update()
    {
        if(knight_Health <= 0 || princess_Health <= 0)
        {
            isAlive = false;
        }

        if(isAlive)
        {
            knight_Animator.SetBool("isAlive", true);
            princess_Animator.SetBool("isAlive", true);

            if (canMove)
            {
                KnightMovement();
                PrincessMovement();
            }
        }

        else
        {
            knight_Animator.SetBool("isAlive", false);
            princess_Animator.SetBool("isAlive", false);
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
            knight_Animator.SetBool("isMoving", true);
            knight.transform.position = ClampPositionWithinLimits(knight.transform.position + (Vector3)(knightMovementInput * moveSpeed * Time.deltaTime));

            if (knightMovementInput.x < 0)
            {
                knight.transform.localScale = new Vector3(-1, 1, 1);
                knightFacingRight = false;
            }

            else if (knightMovementInput.x > 0)
            {
                knight.transform.localScale = new Vector3(1, 1, 1);
                knightFacingRight = true;
            }
        }

        else
        {
            knight_Animator.SetBool("isMoving", false);
        }
    }

    private void PrincessMovement()
    {
        if (princessMovementInput != Vector2.zero)
        {
            princess_Animator.SetBool("isWalking", true);
            princess.transform.position = ClampPositionWithinLimits(princess.transform.position + (Vector3)(princessMovementInput * moveSpeed * Time.deltaTime));

            if (princessMovementInput.x < 0)
            {
                princess_Animator.SetBool("isFacingRight", false);
            }

            else if (princessMovementInput.x > 0)
            {
                princess_Animator.SetBool("isFacingRight", true);
            }
        }

        else
        {
            princess_Animator.SetBool("isWalking", false);
        }
    }

    private Vector3 ClampPositionWithinLimits(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, -x_Limit, x_Limit);
        position.y = Mathf.Clamp(position.y, -y_Limit, y_Limit);
        return position;
    }

    public void OnLightAttack(InputAction.CallbackContext Lightattack)
    {
        if (Lightattack.performed)
        {
            knight_Animator.SetTrigger("lightAttack");
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext HeavyAttack)
    {
        if (HeavyAttack.performed)
        {
            knight_Animator.SetTrigger("heavyAttack");
            StartCoroutine(waitforNextHeavyAttack());
        }
    }

    private IEnumerator waitforNextLightAttack()
    {
        canMove = false;

        yield return new WaitForSeconds(1f);

        canMove = true;
    }

    private IEnumerator waitforNextHeavyAttack()
    {
        canMove = false;

        if (knightFacingRight)
        {
            knight_rb.AddForce(Vector2.right * heavyAttackForce);
        }
        else
        {
            knight_rb.AddForce(Vector2.left * heavyAttackForce);
        }

        yield return new WaitForSeconds(.2f);

        knight_rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(1f);

        canMove = true;
    }
}
