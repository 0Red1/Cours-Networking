using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private NetworkManager m_networkManagerPrefab;
    private NetworkManager m_networkManager;
    private UnityTransport m_transport;

    [Header("UIs")]
    [SerializeField] private TMP_InputField m_addressField;
    [SerializeField] private TMP_InputField m_portField;
    [SerializeField] private Button m_hostBtn;
    [SerializeField] private Button m_clientBtn;
    #endregion

    #region Built-in Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!NetworkManager.Singleton)
        {
            m_networkManager = Instantiate(m_networkManagerPrefab);
        }
        else
        {
            m_networkManager = NetworkManager.Singleton;
        }

            m_transport = m_networkManager.GetComponent<UnityTransport>();
        //var transport2 = (m_networkManager.NetworkConfig.NetworkTransport as UnityTransport); la ligne qu'a conseille d'utiliser Guillaume

        m_addressField.text = m_transport.ConnectionData.Address;
        m_portField.text = m_transport.ConnectionData.Port.ToString();

        m_hostBtn.onClick.AddListener(HandleHostButtonClicked);
        m_clientBtn.onClick.AddListener(HandleClientButtonClicked);

        m_networkManager.OnServerStarted += HandleServerStarted;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        m_networkManager.OnServerStarted -= HandleServerStarted;
        m_hostBtn.onClick.RemoveListener(HandleHostButtonClicked);
        m_hostBtn.onClick.RemoveListener(HandleClientButtonClicked);
    }
    #endregion

    #region Custom Methods
    private void HandleHostButtonClicked()
    {
        Debug.Log("Host Button Cliked");

        m_transport.ConnectionData.Address = m_addressField.text;
        m_transport.ConnectionData.ServerListenAddress = m_addressField.text;
        m_transport.ConnectionData.Port = ushort.Parse(m_portField.text);

        m_networkManager.StartHost();
    }

    private void HandleClientButtonClicked()
    {
        Debug.Log("Client Button Cliked");

        m_transport.ConnectionData.Address = m_addressField.text;
        m_transport.ConnectionData.Port = ushort.Parse(m_portField.text);

        m_networkManager.StartClient();
    }

    private void HandleServerStarted() 
    { 
        if (!m_networkManager.IsServer)
        {
            return;
        }

        m_networkManager.SceneManager.LoadScene("GameplayScene", LoadSceneMode.Single);
    }
    #endregion
}
