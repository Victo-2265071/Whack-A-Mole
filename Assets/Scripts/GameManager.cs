using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum EtatJeu { Menu, EnJeu, GameOver }

    [Header("Canvas")]
    [SerializeField] private GameObject canvasMenu;
    [SerializeField] private GameObject canvasHUD;
    [SerializeField] private GameObject canvasGameOver;

    [Header("Textes")]
    [SerializeField] private TextMeshProUGUI texteTimer;
    [SerializeField] private TextMeshProUGUI texteScoreFinal;
    [SerializeField] private TextMeshProUGUI texteScore;

    [Header("Timer")]
    [SerializeField] private int minutesDepart = 2;
    [SerializeField] private int secondesDepart = 30;

    private EtatJeu etatActuel;
    private float tempsRestant;
    private bool timerActif;
    private int score;

    void Start()
    {
        ChangerEtat(EtatJeu.Menu);
    }

    void Update()
    {
        if (timerActif)
        {
            tempsRestant -= Time.deltaTime;

            if (tempsRestant <= 0f)
            {
                tempsRestant = 0f;
                AfficherTimer();
                TerminerJeu();
                return;
            }

            AfficherTimer();
        }
    }

    public void ChangerEtat(EtatJeu nouvelEtat)
    {
        etatActuel = nouvelEtat;
        canvasMenu.SetActive(etatActuel == EtatJeu.Menu);
        canvasHUD.SetActive(etatActuel == EtatJeu.EnJeu);
        canvasGameOver.SetActive(etatActuel == EtatJeu.GameOver);
    }

    public void CommencerJeu()
    {
        tempsRestant = minutesDepart * 60f + secondesDepart;
        score = 0;
        timerActif = true;
        AfficherTimer();
        ChangerEtat(EtatJeu.EnJeu);
    }

    public void TerminerJeu()
    {
        timerActif = false;
        texteScoreFinal.text = $"Score : {score}";
        ChangerEtat(EtatJeu.GameOver);
    }

    public void IncrementerPoints(int nbPoints)
    {
        score += nbPoints;
        texteScore.text = $"Score : {score}";
    }

    public void Rejouer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    private void AfficherTimer()
    {
        int minutes = Mathf.FloorToInt(tempsRestant / 60f);
        int secondes = Mathf.FloorToInt(tempsRestant % 60f);
        texteTimer.text = $"{minutes:00}:{secondes:00}";
    }
}