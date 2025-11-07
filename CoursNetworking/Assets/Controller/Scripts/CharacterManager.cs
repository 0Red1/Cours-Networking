using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterManager : NetworkBehaviour
{
    [SerializeField]
    private Character m_characterPrefab;
    
    private Dictionary<ulong, Character> m_character = new Dictionary<ulong, Character>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnDestroy()
    {
        NetworkManager.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.OnClientDisconnectCallback -= HandleClientStopped;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        

        NetworkManager.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.OnClientDisconnectCallback += HandleClientStopped;

        var clientsEnumerator = NetworkManager.ConnectedClients.GetEnumerator();

        while (clientsEnumerator.MoveNext())
        {
            var clientPair = clientsEnumerator.Current;
            HandleClientConnected(clientPair.Key);
        }
    }

    private void HandleClientConnected(ulong a_clientId)
    {
        if (!NetworkManager.IsServer)
            return;
        
        Character newCharacter;

        if (m_character.ContainsKey(a_clientId))
        {
            newCharacter = m_character[a_clientId];
        }
        else
        {
            newCharacter = Instantiate(m_characterPrefab,  new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f)), Quaternion.identity);
            m_character.Add(a_clientId, newCharacter);
        }

        if (!newCharacter.IsSpawned)
        {
            newCharacter.NetworkObject.SpawnWithOwnership(a_clientId);
        }
        else
        {
            newCharacter.NetworkObject.ChangeOwnership(a_clientId);
        }
    }

    private void HandleClientStopped(ulong a_clientId)
    {
        if (!NetworkManager.IsServer)
            return;
        

        if (!m_character.ContainsKey(a_clientId))
            return;
        
        
        var character = m_character[a_clientId];
        m_character.Remove(a_clientId);
        Destroy(character.gameObject);
    }
}
