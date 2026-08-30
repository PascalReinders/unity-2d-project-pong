using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GamePhase { Explanation, NameEntry, Playing, GameOver }
    public GamePhase CurrentPhase;

    [SerializeField] private GameObject ballPrefab;

    public string player1Name;
    public string player2Name;
    public int player1Score = 0;
    public int player2Score = 0;

    void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

    public void ChangePhase(GamePhase newPhase)
    {
        CurrentPhase = newPhase;

        switch (newPhase)
        {
            case GamePhase.Explanation:
                Time.timeScale = 0f;
                break;

            
            case GamePhase.Playing:
                Time.timeScale = 1f;
                break;

            case GamePhase.GameOver:
                Time.timeScale = 0f;
                break;
        }
    }

    public void Startmatch(string p1, string p2)
    {
        player1Name = p1;
        player2Name = p2;

        ChangePhase(GamePhase.Playing);
        GameObject.Instantiate(ballPrefab, Vector2.zero, Quaternion.identity);
    }

    public void Player1Scores()
    {
        player1Score++;
        UIManager.Instance.Player1ScoreUpdate(player1Score);
        
        if (player1Score == 11)
        {
            ChangePhase(GamePhase.GameOver);
            UIManager.Instance.ShowGameOver(player1Name, player1Score, player2Score);
        }
    }

    public void Player2Scores()
    {
        player2Score++;
        UIManager.Instance.Player2ScoreUpdate(player2Score);

        if (player2Score == 11)
        {
            ChangePhase(GamePhase.GameOver);
            UIManager.Instance.ShowGameOver(player2Name, player1Score, player2Score);
        }
    }
}
