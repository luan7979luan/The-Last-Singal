using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;  // Import để sử dụng SceneManager

public class CutsceneController : MonoBehaviour
{
    public Image cutsceneImage;         // UI Image chứa hình ảnh cutscene
    public TMP_Text cutsceneText;       // TextMeshPro để hiển thị text
    public Sprite[] images;             // Mảng chứa các hình ảnh (Sprite)
    public string[] texts;              // Mảng chứa các đoạn văn bản tương ứng
    public float displayTime = 3f;      // Thời gian hiển thị mỗi cảnh
    public float fadeDuration = 1f;     // Thời gian thực hiện hiệu ứng fade

    private CanvasGroup canvasGroup;    // CanvasGroup để điều khiển alpha

    void Awake()
    {
        // Lấy hoặc thêm CanvasGroup cho GameObject chứa script
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    void Update()
    {
        // Nếu người chơi nhấn phím E, thực hiện skip cutscene và chuyển sang menu scene
        if (Input.GetKeyDown(KeyCode.E))
        {
            SkipCutscene();
        }
    }

    void SkipCutscene()
    {
        // Dừng toàn bộ coroutine hiện tại
        StopAllCoroutines();
        // Chuyển sang scene menu, thay "MenuScene" bằng tên scene menu của bạn
        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator PlayCutscene()
    {
        for (int i = 0; i < images.Length; i++)
        {
            // Cập nhật hình ảnh và văn bản cho cảnh hiện tại
            cutsceneImage.sprite = images[i];
            cutsceneText.text = texts[i];

            // Thực hiện fade in
            yield return StartCoroutine(FadeIn());

            // Hiển thị cảnh trong khoảng thời gian displayTime
            yield return new WaitForSeconds(displayTime);

            // Thực hiện fade out
            yield return StartCoroutine(FadeOut());
        }

        // Sau khi hoàn thành cutscene, chuyển sang menu scene
        SceneManager.LoadScene("MainMenuScene");
    }

    // Coroutine fade in: tăng alpha từ 0 lên 1
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

    // Coroutine fade out: giảm alpha từ 1 xuống 0
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
