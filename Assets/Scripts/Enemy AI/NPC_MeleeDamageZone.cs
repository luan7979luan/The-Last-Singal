using UnityEngine;

public class NPC_MeleeDamageZone : MonoBehaviour
{
    public int damage = 10;
    // Giả sử chỉ có thể gây damage 1 lần mỗi lần tấn công, bạn có thể dùng một biến để tránh gây damage liên tục

    private bool hasDealtDamage = false;

    // Phương thức này sẽ được gọi khi kích hoạt từ animation (bằng cách gọi ResetDamageZone)
    public void ResetDamageZone()
    {
        hasDealtDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasDealtDamage && other.CompareTag("Player"))
        {
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
