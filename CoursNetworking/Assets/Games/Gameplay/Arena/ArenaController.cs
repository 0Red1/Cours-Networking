using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    
    [SerializeField] private ArenaActivator activator;
    [SerializeField] private int nbCoin = 10;
    [SerializeField] private int startCoin = 3;
    [SerializeField] private int maxCoin = 4;
    [SerializeField] private float timeToSpawn = 3f;
    
    private int _currentCoin;
    private int _currentVisibleCoin; 
    private float _currentTimeToSpawn;
    
    private void Start()
    {
        _currentCoin = nbCoin;
        _currentTimeToSpawn = 0;
    }

    private void Update()
    {
        if (_currentTimeToSpawn > 0)
        {
            _currentTimeToSpawn -= Time.deltaTime;
            if ((int)_currentTimeToSpawn % (int)timeToSpawn <= 1)
            {
                InstantiateCoin();
            }
        }
    }
    
    public void ActivateArena()
    {
        for (int i = 0; i < startCoin; i++)
        {
           InstantiateCoin();
        }
    }

    private void InstantiateCoin()
    {
        if (!Unity.Netcode.NetworkManager.Singleton.IsServer)
        {
            return;
        }
            
        int x = UnityEngine.Random.Range((int)transform.position.x-5,(int)transform.position.x+5);
        int z = UnityEngine.Random.Range((int)transform.position.z-5, (int)transform.position.z+5);

        Vector3 spawnLocation = new Vector3(x, 1, z);

        GameObject newCoin = Instantiate(coinPrefab, spawnLocation, Quaternion.identity);
        NetworkObject newCoinNetworkObject = newCoin.GetComponent<NetworkObject>();

        if (newCoinNetworkObject != null) 
        {
            newCoinNetworkObject.Spawn();
            _currentVisibleCoin++;
        }
    }
    
    private void UnregisteryCoins()
    {
        _currentCoin -= 1;
        if (_currentCoin <= 0)
        {
            EndArena();
        }
        else
        {
            _currentVisibleCoin--;
            _currentTimeToSpawn += timeToSpawn;
        }
    }

    void OnEnable()
    {
        Coin.OnCoinRecover += UnregisteryCoins;
    }

    void OnDisable()
    {
        Coin.OnCoinRecover -= UnregisteryCoins;
    }

    private void EndArena()
    {
        activator.DisabledArena();
    }

   
}
