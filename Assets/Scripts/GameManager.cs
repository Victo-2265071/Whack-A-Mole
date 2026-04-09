using UnityEngine;
using TMPro;
using static Unity.VisualScripting.Metadata;

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

    [Header("Moles")]
    [SerializeField] private GameObject listeMoles;
    [SerializeField] private float intervaleDebut = 1.5f;
    [SerializeField] private float intervaleFin = 0.3f;

    [Header("Marteau")]
    [SerializeField] private GameObject marteau;

    private Mole[] moles;

    private EtatJeu etatActuel;
    private float tempsRestant;
    private bool timerActif;
    private int score;
    private float intervaleTimer = 0f;
    private float tempsTotal;

    // Singleton
    public static GameManager Instance { get; private set; }

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ChangerEtat(EtatJeu.Menu);
        moles = listeMoles.GetComponentsInChildren<Mole>();
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

            intervaleTimer += Time.deltaTime;

            float progression = 1f - (tempsRestant / tempsTotal); // 0 au début, 1 à la fin.
            float intervaleActuel = Mathf.Lerp(intervaleDebut, intervaleFin, progression); // Math.Lerp() proposé par Claude pour extrapoler de facon linéaire le temps entre les apparition de moles.

            if (intervaleTimer >= intervaleActuel)
            {
                intervaleTimer = 0f;
                SelectRandomMole();
            }
        }

    }

    private void ChangerEtat(EtatJeu nouvelEtat)
    {
        etatActuel = nouvelEtat;
        canvasMenu.SetActive(etatActuel == EtatJeu.Menu);
        canvasHUD.SetActive(etatActuel == EtatJeu.EnJeu);
        canvasGameOver.SetActive(etatActuel == EtatJeu.GameOver);
    }

    private void CommencerJeu()
    {
        tempsTotal = minutesDepart * 60f + secondesDepart;
        tempsRestant = tempsTotal;
        score = 0;
        texteScore.text = $"Score : {score}";
        timerActif = true;
        AfficherTimer();
        ChangerEtat(EtatJeu.EnJeu);

        // Remettre le marteau sur le socle.
        marteau.gameObject.transform.position = new Vector3(0.75f, 1.15f, 1.3f);
        marteau.gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        Rigidbody rb = marteau.gameObject.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void TerminerJeu()
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

    private void AfficherTimer()
    {
        int minutes = Mathf.FloorToInt(tempsRestant / 60f);
        int secondes = Mathf.FloorToInt(tempsRestant % 60f);
        texteTimer.text = $"{minutes:00}:{secondes:00}";
    }

    private void SelectRandomMole()
    {
        Mole[] molesDisponibles = System.Array.FindAll(moles, mole => !mole.estSortie);

        if (molesDisponibles.Length == 0) return;

        int index = Random.Range(0, molesDisponibles.Length);
        molesDisponibles[index].Sortir();
    }
}