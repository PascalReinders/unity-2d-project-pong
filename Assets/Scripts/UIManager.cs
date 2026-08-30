using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PanelRenderer))]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}

    private VisualElement explanationContainer;
    private VisualElement nameContainer;
    private VisualElement scoreContainer;
    private VisualElement gameoverContainer;

    private Button understoodButton;
    private Button startButton;
    private Button playAgainButton;
    private Button exitButton;

    private TextField inputPlayer1;
    private TextField inputPlayer2;


    private Label errorMessage;
    private Label player1NameLabel;
    private Label player1ScoreLabel;
    private Label player2NameLabel;
    private Label player2ScoreLabel;
    private Label winnerLabel;
    private Label scoreCompareLabel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void OnEnable()
    {
        PanelRenderer renderer = GetComponent<PanelRenderer>();
        renderer.RegisterUIReloadCallback(ActivateMenu);
    }

    private void ActivateMenu(PanelRenderer renderer, VisualElement uiRoot)
    {
        explanationContainer = uiRoot.Q<VisualElement>("explanation-container");
        nameContainer = uiRoot.Q<VisualElement>("name-container");
        scoreContainer = uiRoot.Q<VisualElement>("score-container");
        gameoverContainer = uiRoot.Q<VisualElement>("gameover-container");

        understoodButton = uiRoot.Q<Button>("understood-button");
        startButton = uiRoot.Q<Button>("start-button");
        playAgainButton = uiRoot.Q<Button>("play-again-button");
        exitButton = uiRoot.Q<Button>("exit-button");

        inputPlayer1 = uiRoot.Q<TextField>("input-p1");
        inputPlayer2 = uiRoot.Q<TextField>("input-p2");

        errorMessage = uiRoot.Q<Label>("error-message");
        player1NameLabel = uiRoot.Q<Label>("player1-score-name");
        player1ScoreLabel = uiRoot.Q<Label>("player1-score");
        player2NameLabel = uiRoot.Q<Label>("player2-score-name");
        player2ScoreLabel = uiRoot.Q<Label>("player2-score");
        winnerLabel = uiRoot.Q<Label>("winner-label");
        scoreCompareLabel = uiRoot.Q<Label>("score-compare-label");

        explanationContainer.style.display = DisplayStyle.Flex;
        nameContainer.style.display = DisplayStyle.None;
        scoreContainer.style.display = DisplayStyle.None;
        gameoverContainer.style.display = DisplayStyle.None;

        understoodButton.clicked += OnUnderstoodClicked;
        startButton.clicked += OnStartClicked;
        playAgainButton.clicked += OnPlayAgainClicked;
        exitButton.clicked += OnExitClicked;

        errorMessage.style.display = DisplayStyle.None;

        inputPlayer1.RegisterValueChangedCallback(evt => OnPlayer1NameChanged(evt));
        inputPlayer2.RegisterValueChangedCallback(evt => OnPlayer2NameChanged(evt));
    }

    private void OnUnderstoodClicked()
    {
        explanationContainer.style.display = DisplayStyle.None;
        nameContainer.style.display = DisplayStyle.Flex;
    }

    private void OnStartClicked()
    {
        bool inputCheck = string.IsNullOrWhiteSpace(inputPlayer1.value)
                            || string.IsNullOrWhiteSpace(inputPlayer2.value);

        if (inputCheck)
        {
            errorMessage.style.display = DisplayStyle.Flex;
            errorMessage.text = "ERROR. FIELD IS EMPTY OR CONTAINS SPACING. PLEASE ENTER YOUR NAME!";
            return;
        }
        else if (inputPlayer1.value.Length > 11 || inputPlayer2.value.Length > 11)
        {
            errorMessage.style.display = DisplayStyle.Flex;
            errorMessage.text = "ERROR. YOUR NAME CANNOT BE LONGER THAN 11 CHARACTERS. PLEASE CHANGE AND TRY AGAIN.";
            return;
        }
        else
        {
            nameContainer.style.display = DisplayStyle.None;
            scoreContainer.style.display = DisplayStyle.Flex;

            GameManager.Instance.Startmatch(inputPlayer1.value, inputPlayer2.value);

            player1NameLabel.text = inputPlayer1.value;
            player2NameLabel.text = inputPlayer2.value;
        }
    }

    private void OnPlayer1NameChanged(ChangeEvent<string> evt)
    {
        string upperCaseName = evt.newValue.ToUpper();
        if (inputPlayer1.value != upperCaseName) inputPlayer1.value = upperCaseName;
    }

    private void OnPlayer2NameChanged(ChangeEvent<string> evt)
    {
        string upperCaseName = evt.newValue.ToUpper();
        if (inputPlayer2.value != upperCaseName) inputPlayer2.value = upperCaseName;
    }

    private void OnPlayAgainClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnExitClicked()
    {
        Application.Quit();
        Debug.Log("Exited the game.");
    }

    public void Player1ScoreUpdate(int updatedScore)
    {
        player1ScoreLabel.text = $"{updatedScore}";
    }

    public void Player2ScoreUpdate(int updatedScore)
    {
        player2ScoreLabel.text = $"{updatedScore}";
    }

    public void ShowGameOver(string winnerName, int score1, int score2)
    {
        scoreContainer.style.display = DisplayStyle.None;
        gameoverContainer.style.display = DisplayStyle.Flex;

        winnerLabel.text = $"{winnerName} IS THE WINNER!";
        scoreCompareLabel.text = $"THE FINAL SCORE IS {score1} / {score2}";
    }

    void OnDisable()
    {
        PanelRenderer renderer = GetComponent<PanelRenderer>();
        renderer.UnregisterUIReloadCallback(ActivateMenu);
    }
}
