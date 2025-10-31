using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnDisconnectionMainMenuLoader : MonoBehaviour
{
    [SerializeField] private NetworkManager m_networkManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_networkManager.OnClientStopped += HandleClientStopped;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleClientStopped(bool a_obj)
    {
        Debug.Log($"[DISCONNECTION] Reason: { m_networkManager.DisconnectReason}" );

        SceneManager.LoadSceneAsync("MainMenuScene");
    }
}
