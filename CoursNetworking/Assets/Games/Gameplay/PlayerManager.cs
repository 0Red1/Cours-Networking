using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Variables
    private List<Character> activePlayers = new List<Character>();

    private static PlayerManager _instance;
    #endregion

    #region Properties
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
    public void AddPlayer(Character newPlayer)
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
            player.enabled = isActive;
        }
        Debug.Log($"[PlayerManager] Contrôles des joueurs changés à : {isActive}");
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
}
