using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    
    [SerializeField] private ArenaActivator activator;
    [SerializeField] private int nbEnemy = 10;
    [SerializeField] private int startEnemy = 3;
    [SerializeField] private int maxEnemy = 4;
    [SerializeField] private float timeToSpawn = 4f;
    
    private int _currentEnemy;
    private int _currentVisibleEnemy; 
    private float _currentTimeToSpawn;
    [SerializeField] private float loseTimeToEnemy; 
    
    private void Start()
    {
        _currentEnemy = nbEnemy;
        _currentTimeToSpawn = 0;
    }

    private void Update()
    {
        if (_currentTimeToSpawn > 0)
        {
            _currentTimeToSpawn -= Time.deltaTime;
        }
        else if (_currentVisibleEnemy < maxEnemy)
        {
            InstantiateEnemy();
            _currentTimeToSpawn = timeToSpawn;
        }
    }
    
    public void ActivateArena()
    {
        for (int i = 0; i < startEnemy; i++)
        {
           InstantiateEnemy();
        }
    }

    private void InstantiateEnemy()
    {
        if (!Unity.Netcode.NetworkManager.Singleton.IsServer)
        {
            return;
        }
            
        int x = UnityEngine.Random.Range((int)transform.position.x-5,(int)transform.position.x+5);
        int z = UnityEngine.Random.Range((int)transform.position.z-5, (int)transform.position.z+5);

        Vector3 spawnLocation = new Vector3(x, 1, z);

        GameObject newCoin = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
        NetworkObject newCoinNetworkObject = newCoin.GetComponent<NetworkObject>();

        if (newCoinNetworkObject != null) 
        {
            newCoinNetworkObject.Spawn();
            _currentVisibleEnemy++;
        }
    }
    
    private void UnregisteryCoins()
    {
        _currentEnemy -= 1;
        _currentVisibleEnemy--;
        _currentTimeToSpawn = loseTimeToEnemy;
        if (_currentEnemy <= 0)
        {
            EndArena();
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
