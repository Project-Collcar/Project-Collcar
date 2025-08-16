using System;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner networkRunner;
    
    [Header("Player Prefab")]
    [SerializeField] private NetworkObject playerPrefab; // 유니티 에디터에서 스폰할 자동차 프리팹을 연결
    
    void Awake()
    {
        networkRunner = GetComponent<NetworkRunner>();
    }

    void Start()
    {
        #if UNITY_EDITOR
        StartGame(GameMode.Single);
        #else
        StartGame(GameMode.Shared);
        #endif
    }

    public async void StartGame(GameMode mode)
    {
        // NetworkRunner에게 이 스크립트가 당신의 비서(콜백)이라고 알려주는 역할.
        // 이 코드가 없으면 OnInput, OnPlayerJoined 등 어떤 메서드도 호출되지 않음.
        networkRunner.AddCallbacks(this);
        
        var sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = sceneManager,
            PlayerCount = 6
        });
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new InputData();
        
        data.steerInput = Input.GetAxis("Horizontal");
        data.throttleInput = Input.GetAxis("Vertical");
        data.handBrake = Input.GetKey(KeyCode.Space);
        data.skill1 = Input.GetKeyDown(KeyCode.LeftShift);

        input.Set(data);
    }
    
    // NetworkRunner가 입력을 받기 위해 호출하는 메서드
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // 서버/호스트만 스폰 로직을 실행
        if (runner.IsServer)
        {
            Debug.Log($"Player {player.PlayerId} Joined, Spawning Car.");
            // playerPrefab을 스폰하고, 해당 플레이어에게 입력 권한을 부여
            runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player {player.PlayerId} Left.");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
}