using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSummon : MonoBehaviour
{
    public Image screenOverlay; // Image để làm hiệu ứng tối màn hình.
    public GameObject bossPrefab; // Prefab của con boss.
    public Transform bossSpawnPoint; // Vị trí để triệu hồi boss.
    public Material redSkybox; // Skybox màu đỏ.

    public AudioSource gameMusic; // Nhạc nền của game.
    public AudioSource bossMusicSource; // AudioSource cho nhạc nền boss.
    public AudioClip bossMusic; // Nhạc nền của boss.

    private bool isPlayerInRange = false;
    private bool hasSummonedBoss = false;

    void Start()
    {
        // Đảm bảo màn hình overlay ban đầu trong suốt.
        if (screenOverlay != null)
        {
            screenOverlay.color = new Color(0, 0, 0, 0);
        }

        // Thiết lập ban đầu cho nhạc boss (chưa phát).
        if (bossMusicSource != null)
        {
            bossMusicSource.clip = bossMusic;
            bossMusicSource.volume = 0f;
            bossMusicSource.loop = true; // Lặp lại nhạc boss nếu cần.
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && !hasSummonedBoss)
        {
            StartCoroutine(SummonBossSequence());
        }
    }

    IEnumerator SummonBossSequence()
    {
        hasSummonedBoss = true;
        Debug.Log("Summoning the boss...");

        // Hiệu ứng tối màn hình.
        if (screenOverlay != null)
        {
            yield return StartCoroutine(FadeToBlack());
        }

        // Triệu hồi boss.
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        }

        // Đổi Skybox thành màu đỏ.
        if (redSkybox != null)
        {
            RenderSettings.skybox = redSkybox;
            DynamicGI.UpdateEnvironment(); // Cập nhật môi trường để thay đổi có hiệu lực ngay lập tức.
        }

        // Thay đổi nhạc nền.
        if (gameMusic != null && bossMusicSource != null)
        {
            StartCoroutine(SwitchToBossMusic());
        }

        // Chờ 1 giây trước khi sáng màn hình trở lại.
        yield return new WaitForSeconds(1f);

        if (screenOverlay != null)
        {
            yield return StartCoroutine(FadeToClear());
        }

        Debug.Log("Boss summoned, sky changed to red, and music switched!");
    }

    IEnumerator SwitchToBossMusic()
    {
        float fadeDuration = 1f; // Thời gian fade nhạc.
        
        // Fade out nhạc nền game.
        if (gameMusic != null)
        {
            float startVolume = gameMusic.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                gameMusic.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }

            gameMusic.Stop();
        }

        // Fade in nhạc boss.
        if (bossMusicSource != null)
        {
            bossMusicSource.Play();
            float startVolume = bossMusicSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                bossMusicSource.volume = Mathf.Lerp(0, 1f, t / fadeDuration); // Điều chỉnh âm lượng tối đa của boss nhạc.
                yield return null;
            }
        }
    }

    IEnumerator FadeToBlack()
    {
        float duration = 0.5f; // Thời gian fade.
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            screenOverlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    IEnumerator FadeToClear()
    {
        float duration = 0.5f; // Thời gian fade.
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / duration));
            screenOverlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
