using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Mole : MonoBehaviour
{
    [Header("Haptique")]
    [SerializeField] private float hapticAmplitude = 0.8f;
    [SerializeField] private float hapticDuration = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioClip sonApparition;

    private AudioSource audioSource;

    private Animator animator;
    private bool canHit = false;
    public bool estSortie = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Configurer l'AudioSource pour du son positionnel
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 100% 3D
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.maxDistance = 5f;
    }

    public void Sortir()
    {
        estSortie = true;
        canHit = true;
        animator.SetTrigger("Sortir");
        audioSource.PlayOneShot(sonApparition);
    }

    void Entrer() 
    {
        animator.SetTrigger("Entrer");
    }


    void EstEntree()
    {
        if (canHit) canHit = false;
        estSortie = false;
    }

    void EstFrappe()
    {
        canHit = false;
        GameManager.Instance.IncrementerPoints(1);
        Entrer();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canHit) return;

        if (other.CompareTag("Marteau"))
        {
            // Récupère le GrabInteractable sur le marteau
            XRGrabInteractable grab = other.GetComponentInParent<XRGrabInteractable>();

            if (grab != null && grab.isSelected)
            {
                // Récupère la main qui tient le marteau
                XRBaseInputInteractor controller = grab.GetOldestInteractorSelecting() as XRBaseInputInteractor;

                // Envoie le retour haptique dans la manette
                if (controller != null)controller.SendHapticImpulse(hapticAmplitude, hapticDuration);
            }

            EstFrappe();
        }
    }
}
