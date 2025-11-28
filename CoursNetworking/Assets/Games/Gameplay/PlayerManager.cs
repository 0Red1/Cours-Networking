using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Variables
    private List<Character> activePlayers = new List<Character>();

    private static PlayerManager _instance;
    #endregion

    #region Properties
    public List<Character> ActivePlayers => activePlayers;
    public static PlayerManager Instance => _instance;
    #endregion

    #region Built-in Methods
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Custom Methods
    public void AddPlayer(ulong clientId, Character newPlayer)
    {
        if (!activePlayers.Contains(newPlayer))
        {
            activePlayers.Add(newPlayer);
        }
    }

    public int GetPlayerCount()
    {
        return activePlayers.Count;
    }

    public void SetPlayerActive(bool isActive)
    {
        foreach (Character player in activePlayers) 
        { 
            player.movementController.Stop(!isActive);
        }
        Debug.Log($"[PlayerManager] Contrï¿½les des joueurs changï¿½s ï¿½ : {isActive}");
    }

    public int GetPlayerScore(int playerIndex)
    {
        int listIndex = playerIndex - 1;

        if (listIndex >= 0 && listIndex < activePlayers.Count)
        {
            return activePlayers[listIndex].score.Value;
        }
        return 0;
    }
    #endregion

    // fonction de test a suppr
    public Character GetCharacterByClientId(ulong clientId)
    {
        foreach (Character player in activePlayers)
        {
            // On cherche le joueur dont l'OwnerClientId correspond à l'ID recherché
            if (player.OwnerClientId == clientId)
            {
                return player;
            }
        }
        return null; // Non trouvé
    }
}
