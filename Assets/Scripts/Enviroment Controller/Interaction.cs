using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Interaction : MonoBehaviour
{
    public GameObject repairShipUI; // UI Repair Ship.
    public GameObject enterShipUI; // UI Enter Ship.
    public Image screenOverlay; // Image để làm hiệu ứng tối màn hình.
    public GameObject[] fireObjects; // Các GameObject chứa lửa (Particle Systems).
    public string endGameSceneName; // Tên scene kết thúc game.

    private bool isPlayerInRange = false;
    private bool hasRepaired = false;

    void Start()
    {
        // Ẩn UI lúc đầu.
        if (repairShipUI != null) repairShipUI.SetActive(false);
        if (enterShipUI != null) enterShipUI.SetActive(false);

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
            ShowUI();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (repairShipUI != null) repairShipUI.SetActive(false);
            if (enterShipUI != null) enterShipUI.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!hasRepaired)
            {
                StartCoroutine(RepairShip());
            }
            else
            {
                StartCoroutine(EnterShip());
            }
        }
    }

    IEnumerator RepairShip()
    {
        hasRepaired = true;
        Debug.Log("Repairing the ship...");

        if (repairShipUI != null) repairShipUI.SetActive(false);

        if (screenOverlay != null)
        {
            yield return StartCoroutine(FadeToBlack());
        }

        // Tắt lửa.
        foreach (GameObject fire in fireObjects)
        {
            if (fire != null)
            {
                var particleSystem = fire.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Stop();
                }
                else
                {
                    fire.SetActive(false);
                }
            }
        }

        yield return new WaitForSeconds(1f);

        if (screenOverlay != null)
        {
            yield return StartCoroutine(FadeToClear());
        }

        if (enterShipUI != null) enterShipUI.SetActive(true);
    }

    IEnumerator EnterShip()
    {
        Debug.Log("Entering the ship...");

        // Tắt UI.
        if (enterShipUI != null) enterShipUI.SetActive(false);

        // Hiệu ứng fade out.
        if (screenOverlay != null)
        {
            yield return StartCoroutine(FadeToBlack());
        }

        // Chuyển đến scene end game.
        LoadEndGameScene();
    }

    void ShowUI()
    {
        if (!hasRepaired)
        {
            if (repairShipUI != null) repairShipUI.SetActive(true);
        }
        else
        {
            if (enterShipUI != null) enterShipUI.SetActive(true);
        }
    }

    IEnumerator FadeToBlack()
    {
        float duration = 0.5f;
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
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / duration));
            screenOverlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    void LoadEndGameScene()
    {
        if (!string.IsNullOrEmpty(endGameSceneName))
        {
            SceneManager.LoadScene(endGameSceneName);
        }
        else
        {
            Debug.LogError("End Game Scene Name is not set!");
        }
    }
}
