using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyNumber;
    
    private List<GameObject> _enemyList = new List<GameObject>();
    private int _countEnemy;
    
    private static EnemyManager _instance;
    public static EnemyManager Instance => _instance;
    #endregion
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _countEnemy = enemyNumber;

        HealthSystem.OnCharacterDeath += RemoveCharacter;
    }

    private void RemoveCharacter(GameObject cGameObject)
    {
        if (_enemyList.Contains(cGameObject))
        {
            _enemyList.Remove(cGameObject);
            _countEnemy -= 1;
            Debug.Log(_countEnemy);
            if (_countEnemy - _enemyList.Count == 0)
            {
                Debug.Log("GAGNE");
            }
            else if (NetworkManager.Singleton.IsServer && _enemyList.Count < 5)
            {
                SpawnEnemy();
            }
        }
    }
    
    public void InitializeEnemy(int count)
    {
        while (_enemyList.Count < count)
        {
            SpawnEnemy();
        }
        Debug.Log($"[CoinManager] Cr�ation de {_enemyList.Count} pi�ces.");
    }

    private void SpawnEnemy()
    {
        if (!Unity.Netcode.NetworkManager.Singleton.IsServer)
        {
            return;
        }

        int x = UnityEngine.Random.Range(-3, 3);
        
        int z = UnityEngine.Random.Range(-3, 3);

        Vector3 spawnLocation = new Vector3(x, 1, z);

        GameObject newCoin = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
        NetworkObject newCoinNetworkObject = newCoin.GetComponent<NetworkObject>();

        if (newCoinNetworkObject != null) 
        {
            newCoinNetworkObject.Spawn();
        }
        _enemyList.Add(newCoin);
    }

    public bool allEnemiesDead()
    {
        return _countEnemy == 0;
    }
}
