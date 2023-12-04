using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

public class Electromagnet : MonoBehaviour
{
    public float polarityRange = 5;
    public float magnetForce = 5;
    public KeyCode activationKey = KeyCode.F;
    public float activationDuration = 5f;

    private string polarityTag;
    private string colliderTag = "";
    private string playerStateName = "";
    private bool isMagnetismActive = false;

    private string currentPlayerStateString = "";

    private SpriteRenderer spriteRenderer;
    public Sprite activatedSprite;
    public Sprite deactivatedSprite;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on ElectroMagnet Object");
        }
    }
    void Update()
    {
        //NOTE POLARITY IS SET BY TAG OF OBJECT IN UNITY!
        polarityTag = gameObject.tag;

        if (isMagnetismActive)
        {
            //Debug.Log("activated magnet");

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, polarityRange);

            foreach (Collider2D collider in colliders)
            {
                colliderTag = collider.gameObject.tag;

                if(colliderTag == "Player")
                {
                    PlayerMagnetism playerMagnetism = collider.gameObject.GetComponent<PlayerMagnetism>();
                    if (playerMagnetism != null)
                    {
                        PlayerMagnetism.playerState currentState = playerMagnetism.currentState;
                        currentPlayerStateString = playerMagnetism.currentState.ToString();
                    }
                }

                Debug.Log(currentPlayerStateString);

                if ((polarityTag == "NorthPolarity" && colliderTag == "NorthPolarity") || (polarityTag == "SouthPolarity" && colliderTag == "SouthPolarity") || 
                    (polarityTag == "NorthPolarity" && colliderTag == "Player" && currentPlayerStateString == "NorthMode") || (polarityTag == "SouthPolarity" && colliderTag == "Player" && currentPlayerStateString == "SouthMode")) // rejection
                {
                    Vector2 direction = transform.position - collider.transform.position;
                    float distance = direction.magnitude;

                    // Debug.Log(string.Format("direction is {0} and distance is {1}", direction, distance));
                    if (distance < polarityRange)
                    {
                        Rigidbody2D colliderRB = collider.GetComponent<Rigidbody2D>();
                        if (colliderRB != null)
                        {
                            Vector2 force = direction.normalized * magnetForce;
                            colliderRB.AddForce(-force, ForceMode2D.Force);
                        }
                    }
                }
                else if ((polarityTag == "NorthPolarity" && colliderTag == "SouthPolarity") || (polarityTag == "SouthPolarity" && colliderTag == "NorthPolarity") ||
                    (polarityTag == "NorthPolarity" && colliderTag == "Player" && currentPlayerStateString == "SouthMode") || (polarityTag == "SouthPolarity" && colliderTag == "Player" && currentPlayerStateString == "NorthMode"))  //attraction 
                {
                    Vector2 direction = collider.transform.position - transform.position;
                    float distance = direction.magnitude;

                    // Debug.Log(string.Format("direction is {0} and distance is {1}", direction, distance));

                    if (distance < polarityRange)
                    {
                        Rigidbody2D colliderRB = collider.GetComponent<Rigidbody2D>();
                        if (colliderRB != null)
                        {
                            Vector2 force = direction.normalized * magnetForce;
                            colliderRB.AddForce(-force, ForceMode2D.Force);
                        }
                    }
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(activationKey))
        {
            Debug.Log("ActivateMagnetism called");
            ActivateMagnetism();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player") && Input.GetKeyDown(activationKey))
        {
            Debug.Log("ActivateMagnetism called");
            ActivateMagnetism();
        }
    }


    void ActivateMagnetism()
    {
        isMagnetismActive = true;
        spriteRenderer.sprite = activatedSprite;

        Invoke("DeactivateMagnetism", activationDuration);
        Debug.Log("Magnetism activated!");
    }

    void DeactivateMagnetism()
    {
        isMagnetismActive = false;
        spriteRenderer.sprite = deactivatedSprite;

        Debug.Log("Magnetism deactivated!");
    }
}
