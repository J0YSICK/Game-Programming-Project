
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;
    private FpsMovement movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        movement = GetComponent<FpsMovement>();
    }

    void Update()
    {
        Vector3 velocity = controller.velocity;

        float speed = new Vector3(
            velocity.x, 0f, velocity.z
        ).magnitude;

        animator.SetFloat("MCSpeed", speed);
        animator.SetBool("IsCrouching", movement.IsCrouching);
    }
}
