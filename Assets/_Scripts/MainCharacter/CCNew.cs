using UnityEngine;

public class CCNew : MonoBehaviour
{
    [Range(0f, 1f), SerializeField] float rotationSmoothing = 0.8f;
    [SerializeField] float groundCheckDistance = 0.15f;
    [SerializeField] float jumpHeight = 6f;
    [SerializeField] float minHeight = 0.5f;
    [SerializeField] float speedFactorForJump = 0.5f;
    [SerializeField] float jumpForwardStrength = 200;
    [SerializeField] LayerMask groundLayer;
    CCState state;
    Animator anim;
    Rigidbody rb;
    CapsuleCollider capsule;
    Transform camTransform;
    Vector3 targetDir;
    float speed;
    float vertical;
    float horizontal;
    int id_float_speed;
    int id_trigger_jump;
    int id_bool_grounded;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        camTransform = Camera.main.transform;
        id_float_speed = Animator.StringToHash("ForwardSpeed");
        id_trigger_jump = Animator.StringToHash("JumpTrigger");
        id_bool_grounded = Animator.StringToHash("IsGrounded");
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        switch (state)
        {
            case CCState.Grounded:
                if (Input.GetButtonDown("Jump"))
                {
                    state = CCState.StartJump;
                }
                break;
            default:
                break;
        }
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case CCState.Grounded:
                speed = Mathf.Clamp01(targetDir.magnitude);
                Rotate();
                anim.SetFloat(id_float_speed, speed, 0.2f, Time.deltaTime);
                break;
            case CCState.StartJump:
                anim.SetTrigger(id_trigger_jump);
                anim.SetBool(id_bool_grounded, false);
                capsule.material.dynamicFriction = 0f;
                capsule.material.staticFriction = 0f;
                rb.AddForce(Vector3.up * jumpHeight * (minHeight + speed * speedFactorForJump), ForceMode.VelocityChange);
                state = CCState.Jumping;
                break;
            case CCState.Jumping:
                rb.AddForce(transform.forward * jumpForwardStrength * speed, ForceMode.Acceleration);
                if (CheckIsGrounded())
                {
                    capsule.material.dynamicFriction = 6f;
                    capsule.material.staticFriction = 6f;
                    anim.SetBool(id_bool_grounded, true);
                    state = CCState.Grounded;
                }
                break;
            default:
                break;
        }
    }

    void Rotate()
    {
        Vector3 forward = camTransform.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        targetDir = forward * vertical + right * horizontal;
        if (targetDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            Quaternion characterRotation = Quaternion.Slerp(rb.rotation, targetRotation, 1 - rotationSmoothing);
            rb.MoveRotation(characterRotation);
        }
    }

    bool CheckIsGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * (capsule.radius + groundCheckDistance * 0.5f), Vector3.down);
        //Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (Physics.SphereCast(ray, capsule.radius, groundCheckDistance, groundLayer))
        {
            return true;
        }
        return false;
    }

    public Vector3 GetCenter()
    {
        return capsule.center;
    }

    public float GetHeight()
    {
        return capsule.height;
    }

    public void SetCenter(Vector3 center)
    {
        capsule.center = center;
    }

    public void SetHeight(float height)
    {
        capsule.height = height;
    }
}

public enum CCState
{
    Grounded, StartJump, Jumping
}
