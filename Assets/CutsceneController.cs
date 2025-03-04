using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [Header("Cutscene Elements")]
    public Image cutsceneImage;       // UI Image chứa hình ảnh cutscene
    public TMP_Text cutsceneText;     // TextMeshPro để hiển thị text
    public Sprite[] images;           // Mảng chứa hình ảnh cutscene
    public string[] texts;            // Mảng chứa văn bản cutscene
    public float displayTime = 3f;    // Thời gian hiển thị mỗi cảnh
    public float fadeDuration = 1f;   // Thời gian fade in/out

    [Header("Credits Elements")]
    public GameObject creditsPanel;   // Panel chứa credits
    public TMP_Text creditsText;      // Text hiển thị credits
    public float creditsScrollSpeed = 50f; // Tốc độ cuộn credits
    public float creditsDuration = 10f;    // Thời gian hiển thị credits

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Ẩn credits panel ban đầu
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }

    void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SkipCutscene();
        }
    }

    void SkipCutscene()
    {
        StopAllCoroutines(); // Dừng toàn bộ coroutine
        SceneManager.LoadScene("MainMenuScene"); // Chuyển sang màn hình chính
    }

    IEnumerator PlayCutscene()
    {
        for (int i = 0; i < images.Length; i++)
        {
            cutsceneImage.sprite = images[i];
            cutsceneText.text = texts[i];

            yield return StartCoroutine(FadeIn());
            yield return new WaitForSeconds(displayTime);
            yield return StartCoroutine(FadeOut());
        }

        // Khi kết thúc cutscene, bắt đầu credits
        StartCoroutine(ShowCredits());
    }

    IEnumerator ShowCredits()
    {
        if (creditsPanel != null && creditsText != null)
        {
            creditsPanel.SetActive(true);
            RectTransform creditsRect = creditsText.GetComponent<RectTransform>();

            float startPos = -creditsRect.rect.height;
            float endPos = 500; // Điểm dừng của credits (có thể tùy chỉnh)
            float elapsedTime = 0f;

            while (elapsedTime < creditsDuration)
            {
                creditsRect.anchoredPosition = new Vector2(creditsRect.anchoredPosition.x, Mathf.Lerp(startPos, endPos, elapsedTime / creditsDuration));
                elapsedTime += Time.deltaTime * creditsScrollSpeed;
                yield return null;
            }
        }

        // Khi credits kết thúc, chuyển về menu
        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator FadeIn()
    {
        float t = 0;
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
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
