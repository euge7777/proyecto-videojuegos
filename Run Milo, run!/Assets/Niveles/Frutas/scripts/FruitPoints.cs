using UnityEngine;

public class FruitPoints : MonoBehaviour
{
    public int pointsToAdd = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("La fruta tocó a: " + collision.name);

        if (collision.CompareTag("Player") || collision.name == "Milo")
        {
            Debug.Log("El jugador agarró una fruta.");

            DistanceScoreCounter scoreCounter = FindObjectOfType<DistanceScoreCounter>();

            if (scoreCounter != null)
            {
                scoreCounter.AddPoints(pointsToAdd);
                Debug.Log("Se sumaron " + pointsToAdd + " puntos.");
            }
            else
            {
                Debug.LogWarning("No se encontró DistanceScoreCounter en la escena.");
            }

            Destroy(gameObject);
        }
    }
}