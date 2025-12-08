using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables
    public GameObject waitingPan;

    [Header("TimeBeforeStartGame")]
    public GameObject waitingStartGamePan;
    [SerializeField] private TMP_Text timerBeforeStartGameTxt;

    public GameObject timerPan;
    [SerializeField] private TMP_Text timerTxt;

    [Header("Health")]
    [SerializeField] private Slider p1healthBar;
    [SerializeField] private Slider p2healthBar;

    [Header("Coin")]
    public GameObject scorePan;
    [SerializeField] private TMP_Text scoreJ1InGameTxt;
    [SerializeField] private TMP_Text scoreJ2InGameTxt;
    [SerializeField] private TMP_Text scoreJ1Txt;
    [SerializeField] private TMP_Text scoreJ2Txt;
    [SerializeField] private TMP_Text winnerTxt;
    #endregion

    #region Properties
    public TMP_Text TimerTxt
    {
        get { return timerTxt; }
        set { timerTxt = value; }
    }
    #endregion

    #region Built-in Methods
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
    /// <summary>
    /// Methods pour afficher l ecran d attente du deuxieme joueur
    /// </summary>
    public void ShowWaitingScreen()
    {
        if (waitingPan != null) 
        { 
            waitingPan.SetActive(true);
        }
        Debug.Log("[UIManager] Affichage de l'écran d'attente.");
    }

    /// <summary>
    /// Methods pour cacher l ecran d attente du deuxieme joueur
    /// </summary>
    public void HideWaitingScreen()
    {
        if (waitingPan != null)
        {
            waitingPan.SetActive(false);
        }
        Debug.Log("[UIManager] Masquage de l'écran d'attente.");
    }

    public void ShowWaitingStartGameScreen()
    {
        if (waitingStartGamePan != null)
        {
            waitingStartGamePan.SetActive(true);
        }
        Debug.Log("[UIManager] Affichage de l'écran d'attente du début de la partie.");
    }

    public void HideWaitingStartGameScreen()
    {
        if (waitingStartGamePan != null)
        {
            waitingStartGamePan.SetActive(false);
        }
        Debug.Log("[UIManager] Masquage de l'écran d'attente du début de la partie.");
    }

    public void UpdateTimerBeforeStartGame(float timer)
    {
        timerBeforeStartGameTxt.text = timer.ToString("0");
    }

    public void UpdateUITimer(float timer)
    {
        timerTxt.text = timer.ToString("0");
    }

    public void RegisterHealthSystem(ulong clientID, HealthSystem hs)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            InitializedHealthBar(p1healthBar, hs.MaxHealth);
        }
        else
        {
            InitializedHealthBar(p2healthBar, hs.MaxHealth);
        }

        hs.OnHealthChanged += UpdateUIHealth;

        UpdateUIHealth(clientID, hs.CurrentHealth.Value);
    }

    void InitializedHealthBar(Slider healthBar, float maxValue)
    {
        healthBar.maxValue = maxValue;
    }

    public void UpdateUIHealth(ulong clientId, float newHealth)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            p1healthBar.value = newHealth;
        }
        else
        {
            p2healthBar.value = newHealth;
        }
    }
    #endregion

    #region Coin
    public void ShowScoreScreen() 
    {
        if (scorePan != null)
        {
            scorePan.SetActive(true);
            timerPan.SetActive(false);
        }
    }

    public void UpdateScoreInGameScreen(int scoreJ1, int scoreJ2)
    {
        scoreJ1InGameTxt.text = "J1 : " + scoreJ1.ToString("0");
        scoreJ2InGameTxt.text = "J2 : " + scoreJ2.ToString("0");
    }

    public void UpdateScoreScreen(int scoreJ1, int scoreJ2)
    {
        scoreJ1Txt.text = "J1 : " + scoreJ1.ToString("0");
        scoreJ2Txt.text = "J2 : " + scoreJ2.ToString("0");

        string winner = "";

        if (scoreJ1 > scoreJ2) 
        {
            winner = "LE JOUEUR 1 A GAGNÉ !";
        }

        if (scoreJ2 > scoreJ1)
        {
            winner = "LE JOUEUR 2 A GAGNÉ !";
        }

        if (scoreJ1 == scoreJ2)
        {
            winner = "ÉGALITÉ !";
        }
        winnerTxt.text = winner;
    }
    #endregion
}
