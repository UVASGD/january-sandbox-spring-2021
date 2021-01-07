using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movementSpeed;
    private Rigidbody rb;
    private Vector3 movment;
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        movment.x = Input.GetAxisRaw("Horizontal");
        movment.z = Input.GetAxisRaw("Vertical");

        if(movment.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movment.x, movment.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movment.normalized * movementSpeed * Time.fixedDeltaTime);
    }
}
