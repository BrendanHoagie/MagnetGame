using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class DoorExit : MonoBehaviour
{
    public Animator doorAnimator, playerAnimator;
    public ParticleSystem northParticles, southParticles;
    public Rigidbody2D playerRB;
    public GameObject playerDisplay;

    private enum playerState
    {
        NorthMode,
        SouthMode,
        Neutral
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("found the exit");
            StartCoroutine(EndLevel());
        }
    }

    public IEnumerator EndLevel()
    {
        // wanted this to work to switch but it just doesn't unfortunately. If I had more time - Brendan
//        UpdateStateAnimation();
//        yield return new WaitForSeconds(1);
        playerRB.isKinematic = true;
        yield return new WaitForSeconds(1);
        playerDisplay.SetActive(false);
        doorAnimator.SetBool("exiting", true);
        yield return new WaitForSeconds(1);
        Debug.Log("Going to next level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void UpdateStateAnimation()
    {
        if (playerAnimator.GetBool("isNorth"))
        {
            playerAnimator.SetBool("isNorth", false);
            playerAnimator.SetBool("isNeutral", true);
            playerAnimator.SetBool("isSouth", false);
            //northParticles.Play();
            //southParticles.Play();
        }
        else if (playerAnimator.GetBool("isSouth"))
        {
            playerAnimator.SetBool("isNorth", false);
            playerAnimator.SetBool("isNeutral", true);
            playerAnimator.SetBool("isSouth", false);
            northParticles.Play();
            southParticles.Play();
        }
    }
}
