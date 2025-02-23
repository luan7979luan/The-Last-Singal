using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage = 10f; // Số dame mà viên đạn gây ra

    // Phương thức này được gọi khi viên đạn va chạm với một collider khác
    private void OnCollisionEnter(Collision collision)
    {
        // Nếu va chạm với đối tượng có tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Giả sử đối tượng người chơi có script PlayerHealth với phương thức TakeDamage
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        // Hủy viên đạn sau khi va chạm (có thể thay đổi tùy nhu cầu)
        Destroy(gameObject);
    }
}
