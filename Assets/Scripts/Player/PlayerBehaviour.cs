using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool gameWin;
    [SerializeField] private bool isAlive;

    [Header("Knight Attributes")]
    [SerializeField] private GameObject knight;
    [SerializeField] private Animator knight_Animator;
    [SerializeField] private Rigidbody2D knight_rb;
    [SerializeField] private float heavyAttackForce;
    [SerializeField] private bool canMove;
    private bool knightFacingRight;

    [Header("Princess Attributes")]
    [SerializeField] private GameObject princess;
    [SerializeField] private Animator princess_Animator;

    private Vector2 knightMovementInput;
    private Vector2 princessMovementInput;

    private void Start()
    {
        knightFacingRight = true;
        canMove = true;
    }

    private void Update()
    {
        if(canMove)
        {
            KnightMovement();
            PrincessMovement();
        }
        
    }


//Movement Logic
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

            knight.transform.Translate(knightMovementInput * moveSpeed * Time.deltaTime);

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

            princess.transform.Translate(princessMovementInput * moveSpeed * Time.deltaTime);

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


// Attack logic
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

            StartCoroutine(waitforNextAttack());
        }
    }

    private IEnumerator waitforNextAttack()
    {
        canMove = false;

        if(knightFacingRight)
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
