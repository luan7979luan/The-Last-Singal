using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutEffect : MonoBehaviour
{
    public Image fadeImage; // Image sẽ fade
    public ParticleSystem[] fireEffects; // Mảng các hệ thống hạt lửa
    public float fadeDuration = 1f; // Thời gian fade out và fade in
    private bool isFading = false;

    void Update()
    {
        // Kiểm tra nếu người dùng nhấn phím E
        if (Input.GetKeyDown(KeyCode.F) && !isFading)
        {
            StartCoroutine(FadeOutIn());
        }
    }

    public IEnumerator FadeOutIn()
    {
        isFading = true;

        // Tắt tất cả các hiệu ứng lửa khi fade out bắt đầu
        foreach (var fireEffect in fireEffects)
        {
            if (fireEffect != null)
            {
                fireEffect.Stop(); // Dừng từng hệ thống hạt lửa
            }
        }

        // Fade out (Màn hình tối lại)
        yield return StartCoroutine(FadeToAlpha(1f)); // Fade to black (alpha = 1)

        // Sau khi màn hình tối, có thể thêm một thời gian delay nếu muốn
        yield return new WaitForSeconds(1f); // Độ trễ trước khi fade in

        // Fade in (Màn hình sáng lại)
        yield return StartCoroutine(FadeToAlpha(0f)); // Fade to transparent (alpha = 0)

        isFading = false;
    }

    IEnumerator FadeToAlpha(float targetAlpha)
    {
        Color startColor = fadeImage.color;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, targetAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo alpha cuối cùng là giá trị chính xác
        fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
    }

    // Hàm này để bật lại lửa khi cần
    public void ResetFireEffects()
    {
        foreach (var fireEffect in fireEffects)
        {
            if (fireEffect != null)
            {
                fireEffect.Play(); // Bật lại từng hệ thống hạt lửa
            }
        }
    }
}
