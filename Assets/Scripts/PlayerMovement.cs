using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 7f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

    }
    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 targetVelocity = moveDirection * moveSpeed;

        rb.linearVelocity = new Vector3(
            targetVelocity.x,
            rb.linearVelocity.y,
            targetVelocity.z
        );
    }
}
