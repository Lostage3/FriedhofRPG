using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] float rotationSmoothFactor = 0.2f;
   

    Camera cam;

    Animator anim;

    Rigidbody rb;

    Vector3 targetDirection;

    float horizontal;
    float vertical;
    float speed;
   

    private void Awake()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
       
    }


    void Update()
    {
        Attack();
        HandleInput();
    }

    void FixedUpdate()
    {
        
        Rotating();
        Moving();
        
    }

    void Attack()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            anim.SetBool("AttackBool", true);
        }
        else
        {
            anim.SetBool("AttackBool", false);
        }
    }

    void HandleInput()
    {
        
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void Rotating()
    {
        
        Vector3 forward = cam.transform.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = new Vector3(forward.z, 0f, -forward.x);
        targetDirection = forward * vertical + right * horizontal;
        if (targetDirection == Vector3.zero)
        {
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion characterRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSmoothFactor);
        rb.MoveRotation(characterRotation);
    }

    void Moving()
    {
        speed = Mathf.Clamp01(targetDirection.magnitude);
        anim.SetFloat("ForwardSpeed", speed, 0.2f, Time.deltaTime);
        
    }

   
}
