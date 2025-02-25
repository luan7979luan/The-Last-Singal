using System.Collections;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public float jumHeight;
    public float gravity;
    public float stepDown;
    public float airControl;
    public float jumpDamp;
    public float groundSpeed;
    public float dodgeDistance = 5f;  // Khoảng cách dodge
    public string dodgeStateName = "Dodge"; // Tên state dodge trong Animator (ngoài blend tree)
    public float dodgeDuration = 0.5f; // Thời gian dodge, có thể điều chỉnh theo clip dodge

    Animator animator;
    CharacterController cc;
    Vector2 input;

    Vector3 rootMotion;
    Vector3 velocity;
    bool isJumping;
    bool isDodging = false;
    int isSprintingParam = Animator.StringToHash("isSprinting");

    public AudioSource footStepsSound, jumpSound, sprintsound, fallLandSound;
    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Nhận giá trị input
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                footStepsSound.enabled = false;
                sprintsound.enabled = true;
            }
            else
            {
                footStepsSound.enabled = true;
                sprintsound.enabled= false;
            }
        }

        else
        {
            footStepsSound.enabled = false;
            sprintsound.enabled = false;
        }

        UpdateIsSprinting();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Nếu nhấn phím Z và nhân vật không đang nhảy, không đang dodge
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isJumping && !isDodging)
            {
                StartCoroutine(PerformDodge());
            }
        }
    }

    private void UpdateIsSprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool(isSprintingParam, isSprinting);
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }
    
    private void FixedUpdate()
    {
        if (isJumping) // Trạng thái không trung
        {
            UpdateInAir();
        }
        else // Trạng thái trên mặt đất
        {
            UpdateOnGround();
        }
    }

    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        displacement += CalculateAirControl();
        cc.Move(displacement);
        isJumping = !cc.isGrounded;
        rootMotion = Vector3.zero;
        animator.SetBool("isJumping", isJumping);
    }

    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;
        
        cc.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (!cc.isGrounded)
        {
            SetInAir(0);
        }
    }

    Vector3 CalculateAirControl()
    {
        return ((transform.forward * input.y) + (transform.right * input.x)) * (airControl / 100);
    }

    void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity * jumpDamp * groundSpeed;
        velocity.y = jumpVelocity;
        animator.SetBool("isJumping", true);
    }

    // Coroutine thực hiện dodge
    IEnumerator PerformDodge()
{
    isDodging = true;
    animator.Play(dodgeStateName, 0, 0f);

    // Tính toán hướng dodge dựa vào input
    Vector3 dodgeDir = transform.forward; // Mặc định là dodge về phía trước
    if (Mathf.Abs(input.x) > 0.1f || Mathf.Abs(input.y) > 0.1f)
    {
        dodgeDir = ((transform.forward * input.y) + (transform.right * input.x)).normalized;
    }

    // Di chuyển nhân vật theo hướng dodge
    Vector3 dodgeMovement = dodgeDir * dodgeDistance;
    cc.Move(dodgeMovement);

    // Chờ hết thời gian dodge (dodgeDuration)
    yield return new WaitForSeconds(dodgeDuration);

    isDodging = false;
}

}
