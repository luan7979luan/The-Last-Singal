using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public GameObject Targetplayer;
    private NavMeshAgent agent;
    private Animator animator;

    public GameObject bulletPrefab;   // Thêm prefab viên đạn
    public Transform firePoint;       // Điểm bắn đạn ra

    public float timer = 1;
    private float bullettime;

    public float shootingRange = 20f; // Tầm bắn của quái vật
    public float attackDelay = 0.5f;    // Khoảng thời gian giữa các lần bắn
    private float attackTimer;
    
   

   void Start()
{
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponent<Animator>();
    agent.speed = Random.Range(3f, 6f);
    attackTimer = attackDelay;

    // Nếu chưa gán Targetplayer từ Inspector, tìm đối tượng trong scene có tag "Player"
    if (Targetplayer == null)
    {
        Targetplayer = GameObject.FindGameObjectWithTag("Player");
    }
}
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
{
    if (Targetplayer != null)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Targetplayer.transform.position);

        // Nếu người chơi ngoài tầm dừng và trong tầm bắn
        if (distanceToPlayer > agent.stoppingDistance)
        {
            // Di chuyển về phía người chơi
            agent.SetDestination(Targetplayer.transform.position);
            animator.SetFloat("Velocity", 0.5f);  // Kích hoạt animation di chuyển
        }
        else
        {
            // Dừng lại và bắn khi người chơi ở gần
            agent.SetDestination(transform.position);
            animator.SetFloat("Velocity", 0f);  // Ngừng animation di chuyển

            // Quay về phía người chơi và bắn
            RotateTowardsPlayer();
            shootatplayer();
        }
    }
}

   void shootatplayer()
{
    bullettime += Time.deltaTime;

    if (bullettime >= timer)
    {
        bullettime = 0;

        // Kích hoạt animation tấn công
        animator.SetTrigger("Attack");

        // Xác định hướng bắn
        Vector3 directionToPlayer = (Targetplayer.transform.position - firePoint.position).normalized;

        // Đẩy viên đạn ra xa vị trí xuất phát
        Vector3 spawnPosition = firePoint.position + firePoint.forward * 1f;
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(directionToPlayer));

        // Thêm vận tốc cho viên đạn
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.isKinematic = false;
            bulletRb.useGravity = false;
            bulletRb.velocity = directionToPlayer * 30f; // Tăng tốc độ viên đạn
        }

        Debug.Log("Bullet fired!");
    }
}


IEnumerator FireBulletAfterAnimation()
{
    // Giả định thời gian animation tấn công là 0.5 giây
    yield return new WaitForSeconds(0.5f);

    // Xác định hướng bắn
    Vector3 directionToPlayer = (Targetplayer.transform.position - firePoint.position).normalized;

    // Đẩy viên đạn ra xa hơn
    Vector3 spawnPosition = firePoint.position + firePoint.forward * 0.5f;
    GameObject bullet = Instantiate(bulletPrefab, spawnPosition, firePoint.rotation);

    // Thêm vận tốc cho viên đạn
    Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
    if (bulletRb != null)
    {
        bulletRb.velocity = directionToPlayer * 30f; // Tăng tốc độ viên đạn
    }

}



    // Quay mặt quái vật về phía người chơi
    void RotateTowardsPlayer()
    {
        Vector3 direction = (Targetplayer.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
