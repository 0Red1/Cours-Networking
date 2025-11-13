using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Variables
    public GameObject waitingPan;
    public GameObject timerPan;
    public GameObject scorePan;
    [SerializeField] private TMP_Text scoreJ1InGameTxt;
    [SerializeField] private TMP_Text scoreJ2InGameTxt;
    [SerializeField] private TMP_Text timerTxt;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Custom Methods
    public void ShowWaitingScreen()
    {
        if (waitingPan != null) 
        { 
            waitingPan.SetActive(true);
        }
        Debug.Log("[UIManager] Affichage de l'écran d'attente.");
    }

    public void HideWaitingScreen()
    {
        if (waitingPan != null)
        {
            waitingPan.SetActive(false);
        }
        Debug.Log("[UIManager] Masquage de l'écran d'attente.");
    }

    public void UpdateUITimer(float timer)
    {
        timerTxt.text = timer.ToString("0");
    }

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
