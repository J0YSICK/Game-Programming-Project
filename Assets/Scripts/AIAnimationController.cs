
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class AIAnimationController : MonoBehaviour
{
    public Animator animator { get; private set; }

    private AIController aiController;
    private NavMeshAgent agent;

    void Awake()
    {
        animator = GetComponent<Animator>();
        aiController = GetComponent<AIController>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Update animation parameters
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        float speed = agent != null
            ? agent.velocity.magnitude
            : 0f;

        animator.SetFloat("CharacterSpeed", speed);
    }

    void HitPlayer()
    {
        GameObject objectHit;

        if (aiController.CheckHandsCollision(
            out objectHit, "Player"))
        {
            //SceneManager.LoadScene(0);
        }
    }
}
