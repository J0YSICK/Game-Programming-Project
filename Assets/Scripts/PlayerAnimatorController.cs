
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void LateUpdate()
    {
        Vector3 velocity = controller.velocity;

        float speed = new Vector3(
            velocity.x, 0f, velocity.z
        ).magnitude;

        animator.SetFloat("MCSpeed", speed);
    }
}
