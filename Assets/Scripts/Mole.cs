using System;
using UnityEngine;

public class Mole : MonoBehaviour
{
    private Animator animator;
    private bool canHit = false;
    public bool estSortie = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Sortir()
    {
        estSortie = true;
        canHit = true;
        animator.SetTrigger("Sortir");
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
            EstFrappe();
        }
    }
}
