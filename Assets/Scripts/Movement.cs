using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement;
    private string currentAnimation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontal, vertical);

        // Prevent diagonal movement from being faster
        movement = movement.normalized;

        UpdateAnimation(horizontal, vertical);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(
            rb.position +
            movement * moveSpeed * Time.fixedDeltaTime
        );
    }

    private void UpdateAnimation(float horizontal, float vertical)
    {
        // ==========================================
        // PLAYER STOPPED
        // ==========================================
        if (horizontal == 0 && vertical == 0)
        {
            string idleAnimation;

            if (currentAnimation == "rightWalk")
            {
                idleAnimation = "rightIdle";
            }
            else if (currentAnimation == "leftWalk")
            {
                idleAnimation = "leftIdle";
            }
            else if (currentAnimation == "backWalk")
            {
                idleAnimation = "backIdle";
            }
            else if (currentAnimation == "frontWalk")
            {
                idleAnimation = "frontIdle";
            }
            else
            {
                return;
            }

            if (currentAnimation != idleAnimation)
            {
                animator.Play(idleAnimation);
                currentAnimation = idleAnimation;
            }

            return;
        }

        // ==========================================
        // PLAYER MOVING
        // ==========================================

        string animationName;

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            if (horizontal > 0)
                animationName = "rightWalk";
            else
                animationName = "leftWalk";
        }
        else
        {
            if (vertical > 0)
                animationName = "backWalk";
            else
                animationName = "frontWalk";
        }

        if (currentAnimation != animationName)
        {
            animator.Play(animationName);
            currentAnimation = animationName;
        }
    }
}