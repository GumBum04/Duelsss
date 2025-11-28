using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform optionsPanel;
    public CanvasGroup panelGroup;
    public Image bgFade;

    [Header("Positions")]
    public Vector2 hiddenPos = new Vector2(0, -800);
    public Vector2 shownPos = new Vector2(0, 0);

    [Header("Durations")]
    public float panelFadeDuration = 1.0f;
    public float panelSlideDuration = 1.0f;
    public float bgFadeDuration = 1.2f;

    [Header("Background Fade")]
    [Range(0f, 1f)]
    public float bgFadeAlpha = 0.6f;  // <-- new variable

    // Keep track of coroutines individually
    private Coroutine panelFadeCoroutine;
    private Coroutine panelSlideCoroutine;
    private Coroutine bgFadeCoroutine;

    void Start()
    {
        // Initial state
        optionsPanel.anchoredPosition = hiddenPos;
        panelGroup.alpha = 0f;
        panelGroup.interactable = false;
        panelGroup.blocksRaycasts = false;

        if (bgFade != null)
            bgFade.gameObject.SetActive(false);
    }

    public void OpenOptions()
    {
        // Stop only the relevant coroutines
        if (panelFadeCoroutine != null) StopCoroutine(panelFadeCoroutine);
        if (panelSlideCoroutine != null) StopCoroutine(panelSlideCoroutine);
        if (bgFadeCoroutine != null) StopCoroutine(bgFadeCoroutine);

        panelFadeCoroutine = StartCoroutine(FadePanel(0f, 1f, panelFadeDuration));
        panelSlideCoroutine = StartCoroutine(SlidePanel(hiddenPos, shownPos, panelSlideDuration));
        if (bgFade != null)
            bgFadeCoroutine = StartCoroutine(FadeBackground(0f, bgFadeAlpha, bgFadeDuration)); // <-- uses bgFadeAlpha
    }

    public void CloseOptions()
    {
        if (panelFadeCoroutine != null) StopCoroutine(panelFadeCoroutine);
        if (panelSlideCoroutine != null) StopCoroutine(panelSlideCoroutine);
        if (bgFadeCoroutine != null) StopCoroutine(bgFadeCoroutine);

        panelFadeCoroutine = StartCoroutine(FadePanel(1f, 0f, panelFadeDuration));
        panelSlideCoroutine = StartCoroutine(SlidePanel(shownPos, hiddenPos, panelSlideDuration));
        if (bgFade != null)
            bgFadeCoroutine = StartCoroutine(FadeBackground(bgFadeAlpha, 0f, bgFadeDuration)); // <-- uses bgFadeAlpha
    }

    IEnumerator FadePanel(float start, float end, float time)
    {
        float t = 0f;
        panelGroup.interactable = end > 0.5f;
        panelGroup.blocksRaycasts = end > 0.5f;

        while (t < time)
        {
            t += Time.deltaTime;
            panelGroup.alpha = Mathf.Lerp(start, end, t / time);
            yield return null;
        }
        panelGroup.alpha = end;
    }

    IEnumerator SlidePanel(Vector2 start, Vector2 end, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            optionsPanel.anchoredPosition = Vector2.Lerp(start, end, t / time);
            yield return null;
        }
        optionsPanel.anchoredPosition = end;
    }

    IEnumerator FadeBackground(float start, float end, float time)
    {
        if (bgFade == null)
            yield break;

        bgFade.gameObject.SetActive(true);

        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start, end, t / time);
            bgFade.color = new Color(0, 0, 0, a);
            yield return null;
        }

        bgFade.color = new Color(0, 0, 0, end);

        if (end == 0f)
            bgFade.gameObject.SetActive(false);
    }
}