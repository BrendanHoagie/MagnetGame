using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private GameObject player;
    public ParticleSystem BlueParticles;
    public ParticleSystem RedParticles;

    public GameObject DeathUI;

    private void Start()
    {
        player = this.gameObject;
    }
    private void OnParticleCollision(GameObject other)
    {
        StartCoroutine(doParticles());
        player.SetActive(false);
        DeathUI.SetActive(true);
    }

    private IEnumerator doParticles()
    {
        BlueParticles.Play();
        RedParticles.Play();
        yield return new WaitForSeconds(0.5f);
    }
}
