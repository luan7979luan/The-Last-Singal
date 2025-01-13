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

    private bool isPlayerInRange = false;
    private bool hasSummonedBoss = false;

    void Start()
    {
        // Đảm bảo màn hình overlay ban đầu trong suốt.
        if (screenOverlay != null)
        {
            screenOverlay.color = new Color(0, 0, 0, 0);
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

        // Chờ 1 giây trước khi sáng màn hình trở lại.
        yield return new WaitForSeconds(1f);

        if (screenOverlay != null)
        {
            yield return StartCoroutine(FadeToClear());
        }

        Debug.Log("Boss summoned and sky changed to red!");
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
