using UnityEngine;

public class Movment_Velocity : MonoBehaviour
{
    public float movementSpeed;
    private Rigidbody rb;
    private Vector3 movement;
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

        }

        
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * movementSpeed;

    }
}
