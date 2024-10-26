using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSoundControl : MonoBehaviour
{
   public Transform player; // Kéo Transform của người chơi vào đây
    private AudioSource fireSound;

    void Start()
    {
        fireSound = GetComponent<AudioSource>();
        fireSound.spatialBlend = 1.0f; // Chuyển âm thanh thành 3D
        fireSound.minDistance = 5f; // Khoảng cách gần nhất nghe âm thanh đầy đủ
        fireSound.maxDistance = 20f; // Khoảng cách xa nhất nghe được âm thanh
    }

    void Update()
    {
        if (fireSound != null && player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance > fireSound.maxDistance)
            {
                fireSound.volume = 0;
            }
            else
            {
                fireSound.volume = 1 - (distance - fireSound.minDistance) / (fireSound.maxDistance - fireSound.minDistance);
            }
        }
    }
}
