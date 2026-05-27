using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;

    public Image[] heartImages;

    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;
    public GameOverMenu gameOverMenu;
    private int currentLives;

    private void Start()
    {
        currentLives = maxLives;

        Time.timeScale = 1f;

        UpdateHearts();

        Debug.Log("Vidas iniciales: " + currentLives);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ObstacleMovement obstacle = collision.gameObject.GetComponent<ObstacleMovement>();

        if (obstacle != null)
        {
            LoseLife();

            Destroy(collision.gameObject);
        }
    }

    private void LoseLife()
    {
        currentLives--;

        if (currentLives < 0)
        {
            currentLives = 0;
        }

        UpdateHearts();

        Debug.Log("Perdiste una vida. Vidas restantes: " + currentLives);

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentLives)
            {
                heartImages[i].sprite = fullHeartSprite;
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite;
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");

        if (gameOverMenu != null)
        {
            gameOverMenu.ShowGameOver();
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}
