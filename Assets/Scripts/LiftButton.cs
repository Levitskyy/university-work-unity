using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LiftButton : NetworkBehaviour
{
    public bool isPressed;

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "Player") return;

        SetIsPressedServerRpc(true);
    }

    public void OnTriggerExit2D(Collider2D other) {
        if (other.tag != "Player") return;

        SetIsPressedServerRpc(false);
    }

    [ServerRpc(RequireOwnership=false)]
    public void SetIsPressedServerRpc(bool newValue) {
        SetIsPressedClientRpc(newValue);
    }

    [ClientRpc]
    public void SetIsPressedClientRpc(bool newValue) {
        isPressed = newValue;
    }
}
