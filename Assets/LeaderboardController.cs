using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using UnityEngine.UIElements;


public class LeaderboardController : MonoBehaviour
{
    public int memberID, score;
    

    private static LeaderboardController instance;
    UIDocument leaderboardDocument;
    [SerializeField] private UIDocument mainMenuDocument;
    private Button closeButton, leaderButton;
    private VisualElement background, mainMenu;
    public Label[] leaderboardLabels = new Label[5];
    public Label[] leaderboardScores = new Label[5];

    [SerializeField] private MainMenuController mainMenuController;
    public string playerName;

    private LeaderboardManager leaderboardManager;

    private SaveManager saveManager;

    // Start is called before the first frame update


    void Start()
    {
        Debug.Log("Leaderboard Controller Started");
        saveManager = FindObjectOfType<SaveManager>();
        leaderboardDocument = GetComponent<UIDocument>();
        var root = leaderboardDocument.rootVisualElement;

        var mainMenuRoot = mainMenuDocument.rootVisualElement;
        mainMenu = mainMenuRoot.Q<VisualElement>("Background");
        // mainMenu.style.display = DisplayStyle.None;

        leaderboardManager = FindObjectOfType<LeaderboardManager>();

        closeButton = root.Q<Button>("CloseButton");
        closeButton.RegisterCallback<ClickEvent>(closeButtonPressed);

        leaderButton = mainMenuRoot.Q<Button>("LeaderButton");
        leaderButton.RegisterCallback<ClickEvent>(leaderButtonPressed);

        background = root.Q<VisualElement>("Background");
        background.style.display = DisplayStyle.None;

        leaderboardLabels[0] = root.Q<Label>("Player1");
        leaderboardLabels[1] = root.Q<Label>("Player2");
        leaderboardLabels[2] = root.Q<Label>("Player3");
        leaderboardLabels[3] = root.Q<Label>("Player4");
        leaderboardLabels[4] = root.Q<Label>("Player5");

        leaderboardScores[0] = root.Q<Label>("Player1Score");
        leaderboardScores[1] = root.Q<Label>("Player2Score");
        leaderboardScores[2] = root.Q<Label>("Player3Score");
        leaderboardScores[3] = root.Q<Label>("Player4Score");
        leaderboardScores[4] = root.Q<Label>("Player5Score");
        
        //StartCoroutine(leaderboardManager.StartSession());
    }

    
    private void closeButtonPressed(ClickEvent evt)
    {
        background.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.Flex;
    }
    private void leaderButtonPressed(ClickEvent click)
    {
        mainMenu.style.display = DisplayStyle.None;
        background.style.display = DisplayStyle.Flex;
        StartCoroutine(leaderboardManager.ShowScores());
    }
}
