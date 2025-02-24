using UnityEngine;

public class NPC_MeleeDamageZone : MonoBehaviour
{
    public int damage = 10; // Số damage gây ra
    private bool hasDealtDamage = false; // Để đảm bảo chỉ gây damage 1 lần mỗi đợt tấn công
    private Collider damageCollider;

    private void Awake()
    {
        // Lấy component Collider gắn trên cùng GameObject
        damageCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        // Đảm bảo collider bị tắt khi game bắt đầu
        damageCollider.enabled = false;
    }

    // Phương thức này sẽ được gọi khi đòn tấn công bắt đầu (Animation Event)
    public void EnableDamageZone()
    {
        hasDealtDamage = false;
        damageCollider.enabled = true;  // Bật collider để nhận va chạm
    }

    // Phương thức này sẽ được gọi khi đòn tấn công kết thúc (Animation Event)
    public void DisableDamageZone()
    {
        damageCollider.enabled = false; // Tắt collider sau khi tấn công xong
    }

    // Khi có đối tượng đi vào vùng collider (với Is Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (!hasDealtDamage && other.CompareTag("Player"))
        {
            // Lấy component PlayerHealth từ đối tượng Player hoặc đối tượng cha của nó
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                hasDealtDamage = true;
                Debug.Log("Melee damage " + damage + " applied to player.");
            }
        }
    }
}
