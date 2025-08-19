using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // Assign the Image component
    [SerializeField] private float fadeDuration = 2f; // Duration for fade in/out
    [SerializeField] private float holdLength = 1f;

    private void Awake()
    {
        // Ensure the screen starts fully transparent
        if (fadeImage != null)
        {
            SetAlpha(1f);
        }
    }

    public void FadeInFromBlack()
    {
        fadeDuration = 5f;
        Debug.Log($"Fade Image Active: {fadeImage.gameObject.activeSelf}");

        Debug.Log("FadeInFromBlack called");
        StartCoroutine(Fade(0f));
        Debug.Log("FadeInFromBlack started");
    }

    public void FadeToBlackAndBack(System.Action onFadeToBlackComplete)
    {
        StartCoroutine(FadeSequence(onFadeToBlackComplete));
    }

    private IEnumerator FadeSequence(System.Action onFadeToBlackComplete)
    {
        // Fade to black
        yield return Fade(1f);

        // Trigger the action after fade-to-black
        onFadeToBlackComplete?.Invoke();
        
        yield return new WaitForSeconds(holdLength);

        // Fade back to transparent
        yield return Fade(0f);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        Debug.Log("Fade coroutine started");
        if (fadeImage == null) yield break;

        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            SetAlpha(newAlpha);
            yield return null;
        }

        SetAlpha(targetAlpha); // Ensure exact target alpha is set

        if (targetAlpha == 0f)
        {
            fadeImage.gameObject.SetActive(false);
            fadeDuration = 2f;
        }
        
    }

    private void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }
}