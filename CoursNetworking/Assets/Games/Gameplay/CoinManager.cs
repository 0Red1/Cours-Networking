using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject coinPrefab;

    private List<GameObject> coins = new List<GameObject>();

    private static CoinManager _instance;
    #endregion

    public static CoinManager Instance => _instance;

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

    public void InitializeCoin(int count)
    {
        while (coins.Count < count)
        {
            SpawnCoin();
        }
        Debug.Log($"[CoinManager] Création de {coins.Count} pièces.");
    }

    public void SpawnCoin()
    {
        if (!Unity.Netcode.NetworkManager.Singleton.IsServer)
        {
            return;
        }

        int x = UnityEngine.Random.Range(-3, 3);
        int z = UnityEngine.Random.Range(-3, 3);

        Vector3 spawnLocation = new Vector3(x, 1, z);

        GameObject newCoin = Instantiate(coinPrefab, spawnLocation, Quaternion.identity);
        NetworkObject newCoinNetworkObject = newCoin.GetComponent<NetworkObject>();

        if (newCoinNetworkObject != null) 
        {
            newCoinNetworkObject.Spawn();
        }
        coins.Add(newCoin);
    }

    public void RemoveCoin(GameObject coinToRemove)
    {
        if (coins.Contains(coinToRemove))
        {
            coins.Remove(coinToRemove);
            Debug.Log($"[CoinManager] Pièce retirée de la liste. Pièces restantes : {coins.Count}");

            if (NetworkManager.Singleton.IsServer && coins.Count < 5)
            {
                SpawnCoin();
            }
        }
    }
}
