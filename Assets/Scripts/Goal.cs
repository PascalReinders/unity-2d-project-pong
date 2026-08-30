using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private bool isLeftGoal; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.CurrentPhase != GameManager.GamePhase.Playing) return;

        if (collision.gameObject.CompareTag("Ball"))
        {
            collision.gameObject.GetComponent<BallMovement>().LaunchBall(isLeftGoal);

            if (isLeftGoal) GameManager.Instance.Player2Scores();
            else GameManager.Instance.Player1Scores();
        }
    }
}
