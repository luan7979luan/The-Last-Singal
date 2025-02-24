using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.EventSystems;  // Để kiểm tra UI

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }
    
    public bool isFiring = false;
    public bool canFire = true; // Biến cờ điều khiển việc bắn đạn
    public int fireRate = 10;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public ParticleSystem[] muzzleFlash;
    AudioSource m_shootingSound;
    public ParticleSystem hitEffect;
    public TrailRenderer bulletTracer;

    public Transform raycastOrigin;
    public Transform raycastDestination;
    public float damage = 10;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();
    float maxlifetime = 3.0f;

    // Tính toán vị trí của đạn theo công thức chuyển động có gia tốc trọng trường
    Vector3 GetPosition(Bullet bullet)
    {
        // p + v*t + 0.5*g*t^2
        Vector3 gravity = Vector3.down * bulletDrop;
        return bullet.initialPosition + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }
    
    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(bulletTracer, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }
    
    void Start()
    {
        m_shootingSound = GetComponent<AudioSource>();
    }
    
    // Gọi hàm này khi muốn bắt đầu bắn
    public void StartFiring()
    {
        // Nếu game đang bị pause (Time.timeScale == 0), không cho bắn
        if (Time.timeScale == 0f)
            return;
            
        isFiring = true;
        accumulatedTime = 0.0f;
        FireBullet();
    }
    
    // Hàm update bắn đạn, có kiểm tra nếu con trỏ đang ở trên UI hoặc game bị pause thì không bắn
    public void UpdateFiring(float deltaTime)
    {
        // Nếu game đang bị pause thì không cho bắn
        if (Time.timeScale == 0f)
            return;

        // Nếu con trỏ đang trên UI, không cho bắn
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (!canFire)
            return;  // Nếu không được phép bắn, thoát luôn

        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while (accumulatedTime >= fireInterval)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    // Cập nhật vị trí và trạng thái của các viên đạn
    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }
    
    // Mô phỏng chuyển động của đạn
    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }
    
    // Hủy các viên đạn sau khi hết thời gian tồn tại
    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxlifetime);
    }
    
    // Kiểm tra va chạm của viên đạn theo đoạn thẳng từ vị trí cũ đến mới
    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxlifetime;
            end = hitInfo.point;

            var rb2d = hitInfo.collider.GetComponent<Rigidbody>();
            if (rb2d)
            {
                rb2d.AddForceAtPosition(ray.direction * 20, hitInfo.point, ForceMode.Impulse);
            }

            var hitBox = hitInfo.collider.GetComponent<HitBox>();
            if (hitBox)
            {
                hitBox.OnRayCastHit(this, ray.direction);
            }
            var hitBoxMelee = hitInfo.collider.GetComponent<hitboxMelee>();
            if (hitBoxMelee)
            {
                hitBoxMelee.OnRayCastHit(this, ray.direction);
            }
            var hitBoxNPC2 = hitInfo.collider.GetComponent<HitBoxNPC2>();
            if (hitBoxNPC2)
            {
                hitBoxNPC2.OnRayCastHit(this, ray.direction);
            }
        }
        bullet.tracer.transform.position = end;
    }

    // Phương thức bắn đạn: phát hiệu ứng, âm thanh và tạo ra viên đạn
    private void FireBullet()
    {
        // Nếu game đang bị pause hoặc không được phép bắn, không thực hiện FireBullet
        if (Time.timeScale == 0f || !canFire)
            return;
            
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
            m_shootingSound.Play();
        }

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
    }

    // Dừng bắn đạn
    public void StopFiring()
    {
        isFiring = false;
    }
}
