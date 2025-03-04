using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalCutsceneController : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public Image cutsceneImage;       // Image component for cutscene visuals
    public TMP_Text cutsceneText;     // Text component for cutscene narration
    public Sprite[] images;           // Array of cutscene images
    public string[] texts;            // Array of corresponding texts
    public float displayTime = 3f;    // Time to display each cutscene image
    public float fadeDuration = 1f;   // Duration for fade in/out effects

    [Header("Credits Settings")]
    public GameObject creditsPanel;   // Panel that contains the credits UI
    public TMP_Text creditsText;      // Text component for the credits content
    public float creditsScrollSpeed = 50f; // Speed of the credits scrolling
    public float creditsDuration = 10f;    // Duration for the credits scroll
    public string creditContent = "Game Developed by Your Name\nMusic by Composer\nSpecial Thanks to Everyone!\n\nThank you for playing!";

    private CanvasGroup canvasGroup;

    void Awake()
    {
        // Ensure a CanvasGroup is attached to control fade effects
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Hide the credits panel initially
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    void Update()
    {
        // Allow skipping the entire sequence by pressing E
        if (Input.GetKeyDown(KeyCode.E))
            SkipCutscene();
    }

    void SkipCutscene()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator PlayCutscene()
    {
        // Loop through each cutscene image and text
        for (int i = 0; i < images.Length; i++)
        {
            cutsceneImage.sprite = images[i];
            cutsceneText.text = texts[i];

            yield return StartCoroutine(FadeIn());
            yield return new WaitForSeconds(displayTime);
            yield return StartCoroutine(FadeOut());
        }
        // After the cutscene, start the credits sequence
        StartCoroutine(ShowCredits());
    }

    IEnumerator ShowCredits()
    {
        if (creditsPanel != null && creditsText != null)
        {
            // Assign the credit text content
            creditsText.text = creditContent;

            creditsPanel.SetActive(true);
            RectTransform creditsRect = creditsText.GetComponent<RectTransform>();

            // Set starting and ending positions for the credits scroll
            float startPos = -creditsRect.rect.height;
            float endPos = 500f; // Adjust this value as needed for the final position
            float elapsedTime = 0f;

            // Reset the credits text starting position
            creditsRect.anchoredPosition = new Vector2(creditsRect.anchoredPosition.x, startPos);

            // Smoothly scroll the credits upward over the creditsDuration
            while (elapsedTime < creditsDuration)
            {
                creditsRect.anchoredPosition = new Vector2(creditsRect.anchoredPosition.x, 
                    Mathf.Lerp(startPos, endPos, elapsedTime / creditsDuration));
                elapsedTime += Time.deltaTime * creditsScrollSpeed;
                yield return null;
            }
        }
        // When credits finish, return to the main menu
        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
