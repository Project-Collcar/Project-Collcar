using System;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private static NetworkManager Instance { get; set; }

    // private NetworkRunner runnerPrefab;
    private NetworkRunner runnerInstance;
    
        
    [Header("Player Prefab")]
    [SerializeField] private NetworkObject playerPrefab; // 유니티 에디터에서 스폰할 자동차 프리팹을 연결
    
    
    void Awake()
    {
        // NetworkManager가 이미 존재하면 새로 생긴 것은 파괴. -> this 조건은 굳이 확인하지 않아도 됨 (Awake 특성 상) - 현석
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        // 첫 번째 NetworkManager라면 파괴되지 않도록 설정. -> SingleTon
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        runnerInstance = GetComponent<NetworkRunner>();
    }

    //  테스트 용으로는 Start가 필요하지만, 이후에는 삭제 (UI 버튼을 통한 수동 호출로 변경)
    void Start()
    {
        // NetworkRunner가 이미 실행 중이라면 StartGame을 다시 호출하지 않음.
        if (runnerInstance.IsRunning)
        {
            return;
        }
        
        #if UNITY_EDITOR
        StartGame(GameMode.Single);
        #else
        StartGame(GameMode.Shared);
        #endif
    }

    public async void StartGame(GameMode mode, string sessionName = "Temp_Room")
    {
        // NetworkRunner에게 이 스크립트가 당신의 비서(콜백)이라고 알려주는 역할.
        // 이 코드가 없으면 OnInput, OnPlayerJoined 등 어떤 메서드도 호출되지 않음.

        if (runnerInstance == null)
            runnerInstance = GetComponent<NetworkRunner>();

        if (runnerInstance.IsRunning) return;
        
        //  RunnerInstance를 RunnerPrefab에서 할당
        runnerInstance.AddCallbacks(this);
    
        //  코드 간소화
        await runnerInstance.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = sessionName,
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 6
        });
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // 1. NetworkInputData 인스터스 생성
        var data = new NetworkInputData();

        // 2. Input 클래스를 사용해 현재 플레이어의 키보드 입력을 저장
        data.throttleInput = Input.GetAxis("Vertical");
        data.steerInput = Input.GetAxis("Horizontal");
        data.handBrake = Input.GetKey(KeyCode.Space);

        NetworkButtons buttons = default;
        buttons.Set(InputButtons.SKILL1, Input.GetKey(KeyCode.LeftShift));
        buttons.Set(InputButtons.SKILL2, Input.GetKey(KeyCode.E));
        data.Buttons = buttons;

        // 3. 입력 데이터를 Fusion에게 전달
        input.Set(data);
    }
    
    // NetworkRunner가 입력을 받기 위해 호출하는 메서드
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            Debug.Log($"I am Player {player.PlayerId}, Spawning my Car.");
            // playerPrefab을 스폰하고, 나 자신에게 입력 권한을 부여합니다.
            runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player {player.PlayerId} Left.");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
     
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }
}