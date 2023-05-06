using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Relay : MonoBehaviour
{
    public static Relay Singleton;

    private string joinCode; // Код для присоединения к серверу
    // Start is called before the first frame update
    private async void Start()
    {
        if (Singleton == null) {
            Singleton = this;
        }
        else {
            Destroy(gameObject);
        }

        await UnityServices.InitializeAsync();

        // Добавляем функцию, которая будет выполнена при авторизации
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

       await AuthenticationService.Instance.SignInAnonymouslyAsync(); // Авторизуемся анонимно
    }

    public async void CreateRelay() {
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            Debug.Log("join code: " + joinCode);
            PlayerSpawner.Singleton.SpawnPlayerServerRpc(0);
        } catch (RelayServiceException e){
            Debug.Log(e);
        }
    }

    public async void JoinRelay(string joinCode) {
        try {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.OnClientConnectedCallback += (ulong u) => {
                PlayerSpawner.Singleton.SpawnPlayerServerRpc(0);
            };
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);      
            NetworkManager.Singleton.StartClient();
            
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }
    public static void ShowConnectedIds() {
        foreach(var item in NetworkManager.Singleton.ConnectedClientsIds) {
            Debug.Log(item);
        }
    }
}
