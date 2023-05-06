using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour {
    [SerializeField] private GameObject playerPrefabA; //add prefab in inspector
    [SerializeField] private GameObject playerPrefabB; //add prefab in inspector
    [SerializeField] private Transform playerSpawnPointA;
    [SerializeField] private Transform playerSpawnPointB;
    private NetworkObject netObj;
    
    public static PlayerSpawner Singleton;

    void Start() {
        if (Singleton == null) {
            Singleton = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    [ServerRpc(RequireOwnership=false)] //server owns this object but client can request a spawn
    public void SpawnPlayerServerRpc(int prefabId, ServerRpcParams serverRpcParams = default) {
        GameObject newPlayer;
        if (prefabId==0){
             newPlayer=(GameObject)Instantiate(playerPrefabA);
             newPlayer.transform.position = playerSpawnPointA.position;
        }
        else {
            newPlayer=(GameObject)Instantiate(playerPrefabB);
            newPlayer.transform.position = playerSpawnPointB.position;
        }
        netObj=newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        var clientId = serverRpcParams.Receive.SenderClientId;
        netObj.SpawnAsPlayerObject(clientId,true);
    }
}