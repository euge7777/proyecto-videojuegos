using UnityEngine;

public class ObstacleMovement : MonoBehaviour
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

        float obstacleRightSide = transform.position.x;

        if (spriteRenderer != null)
        {
            obstacleRightSide = spriteRenderer.bounds.max.x;
        }

        if (obstacleRightSide < leftCameraLimit - destroyMargin)
        {
            Destroy(this.gameObject);
        }
    }
}