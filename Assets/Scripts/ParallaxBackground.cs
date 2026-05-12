using UnityEngine;

public sealed class ParallaxBackground : MonoBehaviour
{
    public float Speed { get; private set; }

    private const float initialSpeedValue = 1.5f;
    private const float speedIncreasePerDifficulty = 0.40f;
    private const float overlap = 0.10f;

    private DifficultyScaling difficulty;
    private Camera mainCamera;
    private Renderer tileRenderer;

    public void UpdateSpeed()
    {
        if (difficulty == null)
        {
            return;
        }

        Speed = initialSpeedValue + ((difficulty.DifficultyLevel - 1) * speedIncreasePerDifficulty);
    }

    private void Awake()
    {
        Speed = initialSpeedValue;

        difficulty = FindObjectOfType<DifficultyScaling>();
        mainCamera = Camera.main;
        tileRenderer = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        MoveLeft();
        RepositionIfOutsideCamera();
    }

    private void MoveLeft()
    {
        transform.Translate(Vector3.left * Speed * Time.deltaTime);
    }

    private void RepositionIfOutsideCamera()
    {
        if (mainCamera == null || tileRenderer == null || transform.parent == null)
        {
            return;
        }

        float cameraLeftEdge = mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect;
        float objectRightEdge = tileRenderer.bounds.max.x;

        if (objectRightEdge > cameraLeftEdge)
        {
            return;
        }

        float furthestRightEdge = objectRightEdge;

        foreach (Transform sibling in transform.parent)
        {
            if (sibling == transform)
            {
                continue;
            }

            Renderer siblingRenderer = sibling.GetComponent<Renderer>();

            if (siblingRenderer == null)
            {
                continue;
            }

            if (siblingRenderer.bounds.max.x > furthestRightEdge)
            {
                furthestRightEdge = siblingRenderer.bounds.max.x;
            }
        }

        float tileWidth = tileRenderer.bounds.size.x;
        float newX = furthestRightEdge + tileWidth / 2f - overlap;

        transform.position = new Vector3(
            newX,
            transform.position.y,
            transform.position.z
        );
    }
}