using System.Collections;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    [SerializeField] private GameObject cherryPrefab;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private float moveDuration = 10f;
    private float timer;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnCherry();
            timer = spawnInterval;
        }
    }

    private void SpawnCherry()
    {
        Vector3 spawnPosition = GetRandomEdgePosition();
        GameObject cherry = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(MoveCherry(cherry, spawnPosition));
    }

    private Vector3 GetRandomEdgePosition()
    {
        float padding = 2f;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        int side = Random.Range(0, 4);
        Vector3 cameraPos = mainCamera.transform.position;

        switch (side)
        {
            case 0: // Left side
                return new Vector3(cameraPos.x - cameraWidth / 2 - padding,
                    Random.Range(cameraPos.y - cameraHeight / 2, cameraPos.y + cameraHeight / 2),
                    0);
            case 1: // Right side
                return new Vector3(cameraPos.x + cameraWidth / 2 + padding,
                    Random.Range(cameraPos.y - cameraHeight / 2, cameraPos.y + cameraHeight / 2),
                    0);
            case 2: // Top side
                return new Vector3(Random.Range(cameraPos.x - cameraWidth / 2, cameraPos.x + cameraWidth / 2),
                    cameraPos.y + cameraHeight / 2 + padding,
                    0);
            case 3: // Bottom side
                return new Vector3(Random.Range(cameraPos.x - cameraWidth / 2, cameraPos.x + cameraWidth / 2),
                    cameraPos.y - cameraHeight / 2 - padding,
                    0);
            default:
                return Vector3.zero;
        }
    }

    private IEnumerator MoveCherry(GameObject cherry, Vector3 startPosition)
    {
        Vector3 centerPosition = mainCamera.transform.position;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        float padding = 2f;

        Vector3 endPosition = startPosition;

        if (Mathf.Abs(startPosition.x - centerPosition.x) > Mathf.Abs(startPosition.y - centerPosition.y))
        {
            // If started from left or right side
            float endX = startPosition.x < centerPosition.x ?
                centerPosition.x + cameraWidth / 2 + padding :
                centerPosition.x - cameraWidth / 2 - padding;

            // end Y position using the same slope
            float slope = (centerPosition.y - startPosition.y) / (centerPosition.x - startPosition.x);
            float endY = slope * (endX - startPosition.x) + startPosition.y;
            endPosition = new Vector3(endX, endY, 0);
        }
        else
        {
            // If started from top or bottom side
            float endY = startPosition.y < centerPosition.y ?
                centerPosition.y + cameraHeight / 2 + padding :
                centerPosition.y - cameraHeight / 2 - padding;

            // end X position using the same slope
            float slope = (centerPosition.x - startPosition.x) / (centerPosition.y - startPosition.y);
            float endX = slope * (endY - startPosition.y) + startPosition.x;
            endPosition = new Vector3(endX, endY, 0);
        }

        float elapsed = 0;
        while (elapsed < moveDuration && cherry != null)
        {
            cherry.transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (cherry != null)
        {
            cherry.transform.position = endPosition;
            Destroy(cherry);
        }
    }
}