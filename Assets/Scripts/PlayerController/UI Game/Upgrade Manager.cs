using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UpgradeUIController : MonoBehaviour
{
    public GameObject upgradeUIPanel;
    public RaycastWeapon raycastWeapon;
    public PlayerHealth playerHealth;
    public PlayerExperience playerExperience;
    public Button increaseDamageButton;
    public Button increaseHPButton;
    public GameObject[] uiToHide;
    public TextMeshProUGUI errorText;
    public float errorDisplayTime = 2f;

    void Start()
    {
        if (upgradeUIPanel != null)
            upgradeUIPanel.SetActive(false);

        if (increaseDamageButton != null)
            increaseDamageButton.onClick.AddListener(OnIncreaseDamage);
        if (increaseHPButton != null)
            increaseHPButton.onClick.AddListener(OnIncreaseHP);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (errorText != null)
            errorText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradePanel();
        }
    }

    void ToggleUpgradePanel()
    {
        if (upgradeUIPanel != null)
        {
            bool isActive = !upgradeUIPanel.activeSelf;
            upgradeUIPanel.SetActive(isActive);
            Time.timeScale = isActive ? 0f : 1f;

            if (uiToHide != null)
            {
                foreach (GameObject ui in uiToHide)
                {
                    if (ui != null)
                        ui.SetActive(!isActive);
                }
            }

            // Kiểm soát con trỏ
            if (isActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            
            // Điều chỉnh việc bắn của súng
            if (raycastWeapon != null)
            {
                raycastWeapon.canFire = !isActive;
            }
        }
    }

    void OnIncreaseDamage()
    {
        if (playerExperience != null && playerExperience.availableUpgradePoints > 0)
        {
            playerExperience.availableUpgradePoints--;
            if (raycastWeapon != null)
            {
                raycastWeapon.damage += 10f;
                Debug.Log("Damage increased to: " + raycastWeapon.damage);
            }
            playerExperience.UpdateUpgradePointsUI();
        }
        else
        {
            StartCoroutine(ShowErrorMessage("Not enough points to upgrade"));
        }
    }

    void OnIncreaseHP()
    {
        if (playerExperience != null && playerExperience.availableUpgradePoints > 0)
        {
            playerExperience.availableUpgradePoints--;
            if (playerHealth != null)
            {
                playerHealth.UpgradeHealth(50f);
                Debug.Log("Max Health increased to: " + playerHealth.maxHealth);
            }
            playerExperience.UpdateUpgradePointsUI();
        }
        else
        {
            StartCoroutine(ShowErrorMessage("Not enough points to upgrade"));
        }
    }

    IEnumerator ShowErrorMessage(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(errorDisplayTime);
            errorText.gameObject.SetActive(false);
        }
    }
}
