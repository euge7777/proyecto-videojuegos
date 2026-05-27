using System.Collections;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public Rigidbody2D rigidbodyComponent;
    public SpriteRenderer spriteRenderer;
    public Animator animatorComponent;
    public BoxCollider2D boxColliderComponent;

    public Vector2 normalColliderSize;
    public Vector2 normalColliderOffset;

    public Vector2 sprintColliderSize = new Vector2(0.6f, 0.7f);
    public Vector2 sprintColliderOffset = new Vector2(0f, -0.2f);

    public RuntimeAnimatorController runController;
    public string runStateName = "Run";

    public Sprite jumpFrame;
    public Sprite fallFrame;
    public Sprite sprintFrame;

    public float jumpForce = 6f;

    public float sprintDistance = 1.5f;
    public float sprintSpeed = 12f;
    public float returnSpeed = 3f;
    public float sprintPause = 0.1f;

    private bool isGrounded = true;
    private bool isSprinting = false;
    private bool isRunAnimationPlaying = false;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (rigidbodyComponent == null)
        {
            rigidbodyComponent = GetComponent<Rigidbody2D>();
        }

        if (animatorComponent == null)
        {
            animatorComponent = GetComponentInChildren<Animator>();
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("No se encontró SpriteRenderer. Asignalo en el Inspector o ponelo en un hijo del personaje.");
            enabled = false;
        }

        if (rigidbodyComponent == null)
        {
            Debug.LogError("No se encontró Rigidbody2D. Asignalo en el Inspector o agregalo al personaje.");
            enabled = false;
        }

        if (animatorComponent == null)
        {
            Debug.LogError("No se encontró Animator. Agregalo al personaje o a un hijo del personaje.");
            enabled = false;
        }

        if (runController != null)
        {
            animatorComponent.runtimeAnimatorController = runController;
        }

        if (boxColliderComponent == null)
        {
            boxColliderComponent = GetComponent<BoxCollider2D>();
        }

        if (boxColliderComponent != null)
        {
            normalColliderSize = boxColliderComponent.size;
            normalColliderOffset = boxColliderComponent.offset;
        }
        else
        {
            Debug.LogError("No se encontró BoxCollider2D. Agregalo al personaje o asignalo en el Inspector.");
            enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isSprinting)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !isSprinting)
        {
            StartCoroutine(SprintAndReturn());
        }

        UpdateJumpAndFallFrame();
    }

    private void Jump()
    {
        isGrounded = false;

        StopRunAnimation();

        spriteRenderer.sprite = jumpFrame;

        rigidbodyComponent.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    private void UpdateJumpAndFallFrame()
    {
        if (isSprinting)
        {
            return;
        }

        if (!isGrounded)
        {
            StopRunAnimation();

            if (rigidbodyComponent.linearVelocity.y > 0.1f)
            {
                spriteRenderer.sprite = jumpFrame;
            }
            else if (rigidbodyComponent.linearVelocity.y < -0.1f)
            {
                spriteRenderer.sprite = fallFrame;
            }
        }
        else
        {
            PlayRunAnimation();
        }
    }

    private IEnumerator SprintAndReturn()
    {
        isSprinting = true;

        StopRunAnimation();

        float originalX = transform.position.x;
        float targetX = originalX + sprintDistance;

        // Ida rápida: frame de sprint + collider pequeño
        spriteRenderer.sprite = sprintFrame;
        SetSprintCollider();

        yield return MoveToX(targetX, sprintSpeed);

        yield return new WaitForSeconds(sprintPause);

        // Vuelta lenta: vuelve el collider normal + animación de correr
        SetNormalCollider();
        PlayRunAnimation();

        yield return MoveToX(originalX, returnSpeed);

        isSprinting = false;

        if (isGrounded)
        {
            PlayRunAnimation();
        }
    }

    private IEnumerator MoveToX(float targetX, float moveSpeed)
    {
        while (Mathf.Abs(transform.position.x - targetX) > 0.01f)
        {
            Vector3 position = transform.position;

            position.x = Mathf.MoveTowards(
                position.x,
                targetX,
                moveSpeed * Time.deltaTime
            );

            transform.position = position;

            yield return null;
        }
    }

    private void PlayRunAnimation()
    {
        if (animatorComponent == null)
        {
            return;
        }

        if (!animatorComponent.enabled)
        {
            animatorComponent.enabled = true;
        }

        if (!isRunAnimationPlaying)
        {
            animatorComponent.Play(runStateName);
            isRunAnimationPlaying = true;
        }
    }

    private void StopRunAnimation()
    {
        if (animatorComponent == null)
        {
            return;
        }

        animatorComponent.enabled = false;
        isRunAnimationPlaying = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
    private void SetSprintCollider()
    {
        if (boxColliderComponent == null)
        {
            return;
        }

        boxColliderComponent.size = sprintColliderSize;
        boxColliderComponent.offset = sprintColliderOffset;
    }

    private void SetNormalCollider()
    {
        if (boxColliderComponent == null)
        {
            return;
        }

        boxColliderComponent.size = normalColliderSize;
        boxColliderComponent.offset = normalColliderOffset;
    }
}


