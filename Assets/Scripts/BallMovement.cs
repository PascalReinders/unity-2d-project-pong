using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float speed = 5f;
    private int paddleHitCount = 0;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        bool firstTurnLeft = Random.Range(0, 2) == 0;

        LaunchBall(firstTurnLeft);
    }

    public void LaunchBall(bool isLeftPlayerTurn)
    {
        float xDirection;
        float yDirection;

        bool yRandom = Random.Range(0, 2) == 0;

        paddleHitCount = 0;

        transform.position = Vector2.zero;

        if (isLeftPlayerTurn) xDirection = -1f;
        else xDirection = 1f;

        if (yRandom) yDirection = -1f;
        else yDirection = 1f;

        Vector2 direction = new Vector2(xDirection, yDirection * 1.4f).normalized;

        rb.linearVelocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            paddleHitCount++;

            float yImpact = transform.position.y - collision.transform.position.y;
            yImpact /= 0.4f;

            float xDirection = (transform.position.x > collision.transform.position.x) ? 1f : -1f;
            float rallySpeed = speed;

            if (paddleHitCount >= 12) rallySpeed *= 1.44f;
            else if (paddleHitCount >= 4) rallySpeed *= 1.2f;

            float cornerSpeedBonus = 1.4f;
            float finalSpeed = rallySpeed + (Mathf.Abs(yImpact) * cornerSpeedBonus);

            Vector2 newDirection = new Vector2(xDirection, yImpact).normalized;
            rb.linearVelocity = newDirection * finalSpeed;
        }
    }
}
