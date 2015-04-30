using UnityEngine;
using System.Collections;

public class OnlineGamesBrowser : Menu
{
    public MenuButton GameButtonTemplate;
    public Canvas Canvas;
    public MenuInputModule InputModule;

    private const float DISTANCE_BETWEEN_BUTTONS = 0.12f;

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        RefreshGames();
    }

    private void RefreshGames()
    {
        NetworkManager.Instance.RefreshGames(UpdateGamesList);
    }

    private void UpdateGamesList()
    {
        HostData[] games = NetworkManager.Instance.GetGameInstances();

        Debug.Log("GAME COUNT: " + games.Length);

        InputModule.Buttons = new MenuButton[games.Length]; 

        for (int i = 0; i < games.Length; i++)
        {
            HostData game = games[i];

            MenuButton gameButton = Instantiate(GameButtonTemplate) as MenuButton;
            gameButton.transform.SetParent(Canvas.transform, false);

            gameButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.78f, 0.64f - (DISTANCE_BETWEEN_BUTTONS * i));
            gameButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.22f, 0.54f - (DISTANCE_BETWEEN_BUTTONS * i));

            gameButton.SetText(game.gameName);

            gameButton.onClick.AddListener(() => { JoinSelectedGame(game); });

            InputModule.Buttons[i] = gameButton;
        }

        InputModule.SelectFirstButton();
    }

    private void JoinSelectedGame(HostData game)
    {
        NetworkManager.Instance.JoinGame(game, GameManager.Instance.LoadNextLevel);
    }

    void Update()
    {
        if (InputManager.Instance.GetInputMenuBack())
        {
            MenusManager.Instance.ShowMenu("OnlineOptionsMenu");
        }
    }
}
