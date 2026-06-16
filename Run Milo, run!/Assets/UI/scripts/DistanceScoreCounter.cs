using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DistanceScoreCounter : MonoBehaviour
{
    public Image digitHundreds;
    public Image digitTens;
    public Image digitUnits;

    public Sprite[] numberSprites;

    public float pointsDelay = 5f;

    [Header("Partículas por puntos")]
    public ParticleSystem pointsParticles;
    public int particlesEveryPoints = 10;

    private int currentPoints = 0;
    private int nextParticlesPoint = 10;

    private bool isCounting = false;
    private Coroutine countCoroutine;

    private void Start()
    {
        currentPoints = 0;
        nextParticlesPoint = particlesEveryPoints;

        UpdateScoreImages();

        if (pointsParticles != null)
        {
            pointsParticles.Stop();
        }
    }

    public void StartCounter()
    {
        if (isCounting)
        {
            return;
        }

        isCounting = true;
        countCoroutine = StartCoroutine(CountPointsRoutine());
    }

    public void StopCounter()
    {
        isCounting = false;

        if (countCoroutine != null)
        {
            StopCoroutine(countCoroutine);
        }
    }

    public void ResetCounter()
    {
        currentPoints = 0;
        nextParticlesPoint = particlesEveryPoints;

        UpdateScoreImages();

        if (pointsParticles != null)
        {
            pointsParticles.Stop();
        }
    }

    public void AddPoints(int amount)
    {
        currentPoints += amount;

        if (currentPoints > 999)
        {
            currentPoints = 999;
        }

        UpdateScoreImages();
        CheckParticlesByPoints();
    }

    private IEnumerator CountPointsRoutine()
    {
        while (isCounting)
        {
            yield return new WaitForSeconds(pointsDelay);

            currentPoints++;

            if (currentPoints > 999)
            {
                currentPoints = 999;
            }

            UpdateScoreImages();
            CheckParticlesByPoints();
        }
    }

    private void CheckParticlesByPoints()
    {
        if (currentPoints >= nextParticlesPoint)
        {
            PlayPointsParticles();
            nextParticlesPoint += particlesEveryPoints;
        }
    }

    private void PlayPointsParticles()
    {
        if (pointsParticles == null)
        {
            Debug.LogWarning("No están asignadas las partículas de puntos.");
            return;
        }

        pointsParticles.Stop();
        pointsParticles.Play();

        Debug.Log("Partículas activadas en " + currentPoints + " puntos.");
    }

    private void UpdateScoreImages()
    {
        if (numberSprites == null || numberSprites.Length < 10)
        {
            Debug.LogWarning("Faltan asignar los sprites de números del 0 al 9.");
            return;
        }

        int hundreds = currentPoints / 100;
        int tens = (currentPoints / 10) % 10;
        int units = currentPoints % 10;

        digitHundreds.sprite = numberSprites[hundreds];
        digitTens.sprite = numberSprites[tens];
        digitUnits.sprite = numberSprites[units];
    }
}