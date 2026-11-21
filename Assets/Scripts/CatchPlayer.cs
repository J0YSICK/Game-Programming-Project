using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AIController))]
public class CatchPlayer : MonoBehaviour
{
    [Tooltip("Lets the attack animation start before the game over screen appears.")]
    [SerializeField] private float delayBeforeGameOver = 0.6f;
    [SerializeField] private string gameOverMessage = "The nurse caught you";

    private AIController ai;
    private bool caught;

    private void Awake()
    {
        ai = GetComponent<AIController>();
    }

    private void Update()
    {
        if (caught) return;

        if (ai.currentState == StateType.Attack)
        {
            caught = true;
            StartCoroutine(GameOverAfterDelay());
        }
    }

    private IEnumerator GameOverAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeGameOver);

        if (ai.currentState != StateType.Attack)
        {
            caught = false;
            yield break;
        }

        UIManager.Instance?.ShowGameOver(gameOverMessage);
    }
}