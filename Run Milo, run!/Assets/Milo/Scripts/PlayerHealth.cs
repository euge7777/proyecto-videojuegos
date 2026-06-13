using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;

    public Image[] heartImages;

    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    public GameObject deadObject;
    public GameOverMenu gameOverMenu;

    public AudioSource musicSource;
    public AudioClip backgroundMusic;

    [Range(0f, 1f)]
    public float backgroundMusicVolume = 1f;

    public GameObject deathAnimationObject;
    public Animator deathAnimator;
    public string deathAnimationStateName = "Death";

    public AudioSource deathAudioSource;
    public AudioClip deathSound;

    public AudioSource damageAudioSource;
    public AudioClip damageSound;

    private int currentLives;
    private bool isDead = false;

    private void Start()
    {
        currentLives = maxLives;

        Time.timeScale = 1f;

        if (deathAnimationObject != null)
        {
            deathAnimationObject.SetActive(false);
        }

        if (deathAnimator == null)
        {
            deathAnimator = GetComponentInChildren<Animator>(true);
        }

        if (deathAnimator != null)
        {
            deathAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        UpdateHearts();

        PlayBackgroundMusic();

        Debug.Log("Vidas iniciales: " + currentLives);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
        {
            return;
        }

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
        else
        {
            PlayDamageSound();
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

    private void PlayBackgroundMusic()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        if (backgroundMusic == null)
        {
            Debug.LogWarning("No hay música asignada en Background Music.");
            return;
        }

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = backgroundMusicVolume;
        musicSource.Play();
    }

    private void StopBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    private void PlayDamageSound()
    {
        if (damageSound == null)
        {
            Debug.LogWarning("No hay audio de daño asignado.");
            return;
        }

        if (damageAudioSource == null)
        {
            damageAudioSource = GetComponent<AudioSource>();
        }

        if (damageAudioSource == null)
        {
            damageAudioSource = gameObject.AddComponent<AudioSource>();
        }

        damageAudioSource.PlayOneShot(damageSound);
    }

    private void PlayDeathAnimation()
    {
        if (deathAnimationObject != null)
        {
            deathAnimationObject.SetActive(true);
        }

        if (deadObject != null)
        {
            deadObject.SetActive(true);
        }

        if (deathAnimator == null)
        {
            deathAnimator = GetComponentInChildren<Animator>(true);
        }

        if (deathAnimator == null)
        {
            Debug.LogWarning("No se encontró Animator de muerte.");
            return;
        }

        deathAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        deathAnimator.enabled = true;
        deathAnimator.Play(deathAnimationStateName, 0, 0f);
    }

    private void PlayDeathSound()
    {
        if (deathSound == null)
        {
            Debug.LogWarning("No hay audio de muerte asignado.");
            return;
        }

        if (deathAudioSource == null)
        {
            deathAudioSource = GetComponent<AudioSource>();
        }

        if (deathAudioSource == null)
        {
            deathAudioSource = gameObject.AddComponent<AudioSource>();
        }

        deathAudioSource.PlayOneShot(deathSound);
    }

    private void GameOver()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log("Game Over");

        StopBackgroundMusic();

        PlayDeathAnimation();

        PlayDeathSound();

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