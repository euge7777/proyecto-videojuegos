using UnityEngine;
using UnityEngine.UI;

public class StartAnimationButton : MonoBehaviour
{
    public Button startButton;
    public Animator animatorComponent;
    public string animationStateName = "StartAnimation";

    private void Awake()
    {
        if (startButton == null)
        {
            startButton = GetComponent<Button>();
        }

        if (startButton != null)
        {
            startButton.onClick.AddListener(PlayStartAnimation);
        }
        else
        {
            Debug.LogError("No se encontró el componente Button en este objeto.");
        }
    }

    private void Start()
    {
        if (animatorComponent != null)
        {
            animatorComponent.enabled = false;
        }
    }

    public void PlayStartAnimation()
    {
        if (animatorComponent == null)
        {
            Debug.LogWarning("No hay Animator asignado.");
            return;
        }

        Time.timeScale = 1f;

        animatorComponent.enabled = true;
        animatorComponent.Play(animationStateName, 0, 0f);
    }
}