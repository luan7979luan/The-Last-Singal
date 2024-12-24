using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public int upgradePoints = 5; // Số điểm nâng cấp có sẵn
    public float playerSpeed = 5f; // Tốc độ mặc định
    public int playerDamage = 10; // Damage mặc định
    public int playerMaxHealth = 100; // Máu tối đa mặc định

    public GameObject upgradePanel; // UI nâng cấp
    public Button speedButton; // Nút tăng tốc độ
    public Button damageButton; // Nút tăng damage
    public Button healthButton; // Nút tăng máu

    private bool isUpgradeMenuActive = false; // Trạng thái menu nâng cấp
     public GameObject playerHUD;

    void Start()
    {
        // Gán sự kiện cho các nút
        speedButton.onClick.AddListener(IncreaseSpeed);
        damageButton.onClick.AddListener(IncreaseDamage);
        healthButton.onClick.AddListener(IncreaseHealth);

        // Ẩn panel nâng cấp lúc đầu
        upgradePanel.SetActive(false);
    }

    void Update()
    {
        // Nhấn phím U để mở menu nâng cấp
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
    }

    void ToggleUpgradeMenu()
    {
        isUpgradeMenuActive = !isUpgradeMenuActive;

        if (isUpgradeMenuActive)
        {
            // Hiện panel và dừng game
            playerHUD.SetActive(false);
            upgradePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None; // Hiện chuột
            Cursor.visible = true;
            Time.timeScale = 0f; // Tạm dừng game
        }
        else
        {
            // Ẩn panel và tiếp tục game
            playerHUD.SetActive(true);
            upgradePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked; // Ẩn chuột
            Cursor.visible = false;
            Time.timeScale = 1f; // Tiếp tục game
        }
    }

    // Hàm tăng tốc độ chạy
    public void IncreaseSpeed()
    {
        if (upgradePoints > 0)
        {
            playerSpeed += 1f; // Tăng tốc độ chạy lên 1 đơn vị
            upgradePoints--;
            Debug.Log("Tốc độ hiện tại: " + playerSpeed);
        }
        else
        {
            Debug.Log("Không đủ điểm nâng cấp!");
        }
    }

    // Hàm tăng damage súng
    public void IncreaseDamage()
    {
        if (upgradePoints > 0)
        {
            playerDamage += 5; // Tăng thêm 5 damage
            upgradePoints--;
            Debug.Log("Damage hiện tại: " + playerDamage);
        }
        else
        {
            Debug.Log("Không đủ điểm nâng cấp!");
        }
    }

    // Hàm tăng máu
    public void IncreaseHealth()
    {
        if (upgradePoints > 0)
        {
            playerMaxHealth += 10; // Tăng thêm 10 máu
            upgradePoints--;
            Debug.Log("Máu tối đa hiện tại: " + playerMaxHealth);
        }
        else
        {
            Debug.Log("Không đủ điểm nâng cấp!");
        }
    }
}
