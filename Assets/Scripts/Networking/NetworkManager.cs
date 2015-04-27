using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class NetworkManager : MonoBehaviour
{
    private float btnX;
    private float btnY;
    private float btnW;
    private float btnH;

    private bool _isRefreshingHostList = false;
    private HostData[] _hostDataList;

    private const string _gameName = "ETS_ABoutDeSouffle";

    public bool IsInAGame
    {
        get { return Network.isClient || Network.isServer; }
    }

    private static NetworkManager _instance;

    public static NetworkManager Instance
    {
        get { return _instance; }
    }

    public Player PlayerTest;

    void Awake()
    {
        _instance = this;

        _hostDataList = new HostData[0];
    }

    void Start()
    {
        btnX = Screen.width * 0.05f;
        btnY = Screen.width * 0.05f;
        btnW = Screen.width * 0.1f;
        btnH = Screen.width * 0.1f;
    }

    void Update()
    {
        if (_isRefreshingHostList)
        {
            _hostDataList = MasterServer.PollHostList();

            if (_hostDataList.Length > 0)
            {
                _isRefreshingHostList = false;
                Debug.Log(MasterServer.PollHostList().Length);
            }
            
        }
    }

    void StartServer()
    {
        Network.InitializeServer(4, 25001, !Network.HavePublicAddress());
        MasterServer.RegisterHost(_gameName, "Game Test 1", "This is a test lobby for \"À bout de souffle \"");
    }

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(_gameName);
        _isRefreshingHostList = true;
    }

    void OnServerInitialized()
    {
        Debug.Log("Server Initialized");
    }

    void OnMasterServerEvent(MasterServerEvent mse)
    {
        if (mse == MasterServerEvent.RegistrationSucceeded)
        {
//            GameManager.Instance.GetPlayerAt(0).SpawnCharacter(GameManager.MultiplayerModes.Online);
        }
    }

    void OnConnectedToServer()
    {
        //PlayerTest.SelectedCharacter.InstantiateInput();
    }

    void OnGUI()
    {
        return;
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server"))
            {
                Debug.Log("Starting Server");
                StartServer();
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
        }
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
