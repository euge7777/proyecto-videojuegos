using UnityEngine;

public class FruitMovement : MonoBehaviour
{
    public float velocity = 5f;
    public Camera mainCamera;
    public SpriteRenderer spriteRenderer;

    public float destroyMargin = 0.5f;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    void Update()
    {
        MoveLeft();
        DestroyIfOutsideCamera();
    }

    private void MoveLeft()
    {
        transform.Translate(Vector3.left * velocity * Time.deltaTime, Space.World);
    }

    private void DestroyIfOutsideCamera()
    {
        if (mainCamera == null)
        {
            return;
        }

        float distanceFromCamera = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);

        float leftCameraLimit = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 0, distanceFromCamera)
        ).x;

        float fruitRightSide = transform.position.x;

        if (spriteRenderer != null)
        {
            fruitRightSide = spriteRenderer.bounds.max.x;
        }

        if (fruitRightSide < leftCameraLimit - destroyMargin)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerAction player = collision.gameObject.GetComponent<PlayerAction>();

        if (player != null)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerAction player = collision.gameObject.GetComponent<PlayerAction>();

        if (player != null)
        {
            Destroy(this.gameObject);
        }
    }
}