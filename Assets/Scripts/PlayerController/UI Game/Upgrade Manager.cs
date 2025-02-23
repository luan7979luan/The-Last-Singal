using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UpgradeUIController : MonoBehaviour
{
    // Panel UI chứa các tùy chọn nâng cấp
    public GameObject upgradeUIPanel;
    
    // Tham chiếu đến script RaycastWeapon để tăng damage
    public RaycastWeapon raycastWeapon;
    // Tham chiếu đến script PlayerHealth để tăng max HP
    public PlayerHealth playerHealth;
    // Tham chiếu đến script PlayerExperience để lấy điểm nâng cấp
    public PlayerExperience playerExperience;
    
    // Nút UI dùng để tăng damage
    public Button increaseDamageButton;
    // Nút UI dùng để tăng HP
    public Button increaseHPButton;

    // Các UI khác cần ẩn khi bật panel nâng cấp (ví dụ: HUD, mini-map, v.v.)
    public GameObject[] uiToHide;
    
    // Text hiển thị thông báo lỗi khi không đủ điểm nâng cấp (sử dụng TextMeshProUGUI)
    public TextMeshProUGUI errorText;
    // Thời gian hiển thị thông báo lỗi (sử dụng WaitForSecondsRealtime vì Time.timeScale = 0)
    public float errorDisplayTime = 2f;

    void Start()
    {
        // Ẩn panel nâng cấp khi khởi tạo
        if (upgradeUIPanel != null)
            upgradeUIPanel.SetActive(false);

        // Gán sự kiện cho các nút
        if (increaseDamageButton != null)
            increaseDamageButton.onClick.AddListener(OnIncreaseDamage);
        if (increaseHPButton != null)
            increaseHPButton.onClick.AddListener(OnIncreaseHP);

        // Đảm bảo ban đầu con trỏ bị ẩn và được khóa lại
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ẩn thông báo lỗi khi khởi tạo
        if (errorText != null)
            errorText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Khi nhấn phím U, bật/toggle panel nâng cấp và ẩn/hiện các UI khác
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

            // Nếu bật panel, dừng trò chơi; nếu tắt, tiếp tục trò chơi
            Time.timeScale = isActive ? 0f : 1f;

            // Ẩn/hiện các UI khác khi panel nâng cấp bật/tắt
            if (uiToHide != null)
            {
                foreach (GameObject ui in uiToHide)
                {
                    if (ui != null)
                        ui.SetActive(!isActive);
                }
            }

            // Kiểm soát con trỏ: bật khi panel mở, ẩn khi panel tắt
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
        }
    }

    // Hàm được gọi khi nhấn nút tăng damage
    void OnIncreaseDamage()
    {
        if (playerExperience != null && playerExperience.availableUpgradePoints > 0)
        {
            // Trừ 1 điểm nâng cấp
            playerExperience.availableUpgradePoints--;
            // Tăng damage
            if (raycastWeapon != null)
            {
                raycastWeapon.damage += 10f;
                Debug.Log("Damage increased to: " + raycastWeapon.damage);
            }
            // Cập nhật lại giao diện điểm nâng cấp
            playerExperience.UpdateUpgradePointsUI();
        }
        else
        {
            // Hiển thị thông báo lỗi nếu không đủ điểm nâng cấp
            StartCoroutine(ShowErrorMessage("Not enough points to upgrade"));
        }
    }

    // Hàm được gọi khi nhấn nút tăng HP
    void OnIncreaseHP()
    {
        if (playerExperience != null && playerExperience.availableUpgradePoints > 0)
        {
            // Trừ 1 điểm nâng cấp
            playerExperience.availableUpgradePoints--;
            // Gọi phương thức UpgradeHealth của PlayerHealth để tăng health và cập nhật slider
            if (playerHealth != null)
            {
                playerHealth.UpgradeHealth(50f);
                Debug.Log("Max Health increased to: " + playerHealth.maxHealth);
            }
            // Cập nhật lại giao diện điểm nâng cấp
            playerExperience.UpdateUpgradePointsUI();
        }
        else
        {
            // Hiển thị thông báo lỗi nếu không đủ điểm nâng cấp
            StartCoroutine(ShowErrorMessage("Not enough points to upgrade"));
        }
    }

    IEnumerator ShowErrorMessage(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
            // Sử dụng WaitForSecondsRealtime vì Time.timeScale có thể = 0
            yield return new WaitForSecondsRealtime(errorDisplayTime);
            errorText.gameObject.SetActive(false);
        }
    }
}
