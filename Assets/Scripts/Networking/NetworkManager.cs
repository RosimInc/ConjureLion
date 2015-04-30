using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(NetworkView))]
public class NetworkManager : MonoBehaviour
{
    private bool _isRefreshingHostList = false;
    private HostData[] _hostDataList;

    private const string _gameName = "ETS_ABoutDeSouffle";
    private const int _playerCount = 2;
    private const int _port = 25001;
    private const float _refreshTimeout = 2f;

    private Action _onConnectedToGameCallback;
    private Action _onGameListRefreshedCallback;
    private Action _onGameCreatedCallback;

    private float _refreshElapsedTime = 0f;


    public bool IsInAGame
    {
        get { return Network.isClient || Network.isServer; }
    }

    private static NetworkManager _instance;

    public static NetworkManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;

        _hostDataList = new HostData[0];
    }

    void Update()
    {
        // TODO: Transfer that code into a coroutine

        if (_isRefreshingHostList)
        {
            _refreshElapsedTime += Time.deltaTime;

            _hostDataList = MasterServer.PollHostList();

            if (_hostDataList.Length > 0)
            {
                _isRefreshingHostList = false;

                Debug.Log("Found available games");

                if (_onGameListRefreshedCallback != null)
                {
                    _onGameListRefreshedCallback();
                    _onGameListRefreshedCallback = null;
                }
            }
            else if (_refreshElapsedTime >= _refreshTimeout)
	        {
		        // TODO: Tell the player that no games have been found.

                Debug.Log("No games have been found");

                _isRefreshingHostList = false;

                _onGameListRefreshedCallback = null;
	        }
        }
    }

    public void CreateGame(Action callback)
    {
        Network.InitializeServer(_playerCount, _port, !Network.HavePublicAddress());
        MasterServer.RegisterHost(_gameName, "Game Test 1", "This is a test lobby");

        _onGameCreatedCallback += callback;
    }

    public void JoinGame(HostData game, Action callback)
    {
        // TODO: Verify if the game is still available and put a "Waiting..." popup meanwhile
        // TODO: Verify that the game is not full
        Network.Connect(_hostDataList[0]);

        Debug.Log("Joining game...");

        _onConnectedToGameCallback += callback;
    }

    public void RefreshGames(Action callback)
    {
        _onGameListRefreshedCallback += callback;

        MasterServer.RequestHostList(_gameName);
        _isRefreshingHostList = true;
    }

    void OnMasterServerEvent(MasterServerEvent mse)
    {
        if (mse == MasterServerEvent.RegistrationSucceeded)
        {
//            GameManager.Instance.GetPlayerAt(0).SpawnCharacter(GameManager.MultiplayerModes.Online);
            Debug.Log("The game has been created. Waiting for players to connect.");

            if (_onGameCreatedCallback != null)
            {
                _onGameCreatedCallback();
            }
        }

        if (mse != MasterServerEvent.HostListReceived)
	    {
            _onGameCreatedCallback = null;
	    }
    }

    void OnConnectedToServer()
    {
        //PlayerTest.SelectedCharacter.InstantiateInput();

        if (_onConnectedToGameCallback != null)
        {
            _onConnectedToGameCallback();
        }

        Debug.Log("Connected to game");

        _onConnectedToGameCallback = null;
    }

    void OnGUI()
    {
        /*
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server"))
            {
                Debug.Log("Starting Server");
                //StartServer();
            }

            if (GUI.Button(new Rect(btnX, btnY * 1.2f + btnH, btnW, btnH), "Refresh Hosts"))
            {
                Debug.Log("Refreshing Hosts");
                RefreshHostList();
            }

            for (int i = 0; i < _hostDataList.Length; i++)
            {
                HostData hostData = _hostDataList[i];

                if (GUI.Button(new Rect(btnX * 1.5f + btnW, btnY * 1.2f + (btnH * i), btnW * 3f, btnH * 0.5f), hostData.gameName))
                {
                    Network.Connect(hostData);
                }
            }
        }*/
    }

    public HostData[] GetGameInstances()
    {
        return _hostDataList;
    }

    // Wrapper so that only NetworkManager interacts directly with the RPC interface
    public void SendJumpMessage(int playerID)
    {
        networkView.RPC("OnJumpInput", RPCMode.AllBuffered, playerID);
    }

    public void SendHorizontalMoveMessage(int playerID, float direction)
    {
        networkView.RPC("OnHorizontalMoveInput", RPCMode.AllBuffered, playerID, direction);
    }

    public void SendAttackMessage(int playerID)
    {
        networkView.RPC("OnAttackInput", RPCMode.AllBuffered, playerID);
    }

    [RPC]
    void OnJumpInput(int playerID)
    {
//        GameManager.Instance.GetPlayerAt(playerID).SelectedCharacter.Jump();
        Debug.Log("Receiving jump message");
    }

    [RPC]
    void OnHorizontalMoveInput(int playerID, float direction)
    {
//        GameManager.Instance.GetPlayerAt(playerID).SelectedCharacter.HorizontalMove(direction);
        Debug.Log("Receiving move message");
    }

    [RPC]
    void OnAttackInput(int playerID)
    {
//        GameManager.Instance.GetPlayerAt(playerID).SelectedCharacter.Attack();
    }
}
