using Unity.Netcode;
using UnityEngine;

public class Coin : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();

        if (character != null)
        {
            if (character.IsOwner)
            {
                RequestDestroyAndScoreUpdateServerRpc(character.NetworkObject);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestDestroyAndScoreUpdateServerRpc(NetworkObjectReference characterRef)
    {
        if (characterRef.TryGet(out NetworkObject characterNetworkObject))
        {
            Character character = characterNetworkObject.GetComponent<Character>();

            if (character != null)
            {
                character.score.Value++;
            }
        }

        if (IsSpawned)
        {
            if (CoinManager.Instance != null)
            {
                CoinManager.Instance.RemoveCoin(gameObject);
            }

            NetworkObject.Despawn(true);
        }
    }
}