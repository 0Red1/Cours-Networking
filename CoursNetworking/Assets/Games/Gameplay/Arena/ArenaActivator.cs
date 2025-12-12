using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class ArenaActivator : MonoBehaviour
{
    [SerializeField] private ArenaController arenaController;
    
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject side;
    [SerializeField] private GameObject arenaCamera;
    [SerializeField] private int totalPlayer = 2;
    
    private int _currentPlayer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        back.SetActive(false);
        side.SetActive(false);
        arenaCamera.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered the arena");
        _currentPlayer++;
        if (_currentPlayer == totalPlayer)
        {
            ActiveArena();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited the arena");
        _currentPlayer--;
    }

    private void ActiveArena()
    {
        back.SetActive(true);
        side.SetActive(true);
        arenaCamera.SetActive(true);
        arenaController.ActivateArena();
    }

    public void DisabledArena()
    {
        back.SetActive(false);
        front.SetActive(false);
        side.SetActive(false);
        arenaCamera.SetActive(false);
    }
}
