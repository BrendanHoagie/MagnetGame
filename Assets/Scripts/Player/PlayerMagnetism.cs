using UnityEngine;
using UnityEngine.Playables;

public class PlayerMagnetism : MonoBehaviour
{
    public float magnetRadius = 5f;
    public float magnetForce = 2f;
    //public float maxAttractionDistance = 1f; // Maximum distance for attraction.
    public float breakingForce = .1f; // Force applied to break attraction.
    public SpriteRenderer spriteRenderer; //sprite rendered for player

    public Sprite neutral, north, south;
    public Animator animator;
    public ParticleSystem northParticles;
    public ParticleSystem southParticles;

    public LineRenderer lineRendererPrefab;
    private LineRenderer currentLineRenderer;
    public Material lineMaterial;
    public float lineLifeTime = 0.5f;

    private Rigidbody2D playerRigidbody;  // Rigidbody for the player.
    
    public enum playerState
    {
        NorthMode,
        SouthMode,
        Neutral
    }

    public playerState currentState = playerState.Neutral;
    private playerState previousState = playerState.Neutral;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>(); // Assuming the player has the Rigidbody2D component on the same GameObject as this script.
        animator.SetBool("isNeutral", true);
    }

    void Update()// do not used fixed update no matter what here
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentState = playerState.NorthMode;
            if(previousState != currentState)
            {
                northParticles.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentState = playerState.SouthMode;
            if (previousState != currentState)
            {
                southParticles.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            currentState = playerState.Neutral;
            if (previousState != currentState)
            {
                northParticles.Play();
                southParticles.Play();
            }
        }
        UpdateStateAnimation(currentState);
        previousState = currentState;

        //TODO change player sprite for Polarity here 
        switch (currentState)
        {
            case playerState.NorthMode:
                //spriteRenderer.sprite = Resources.Load<Sprite>("NorthPlayer"); // Change to "NorthPlayer" sprite.
                break;
            case playerState.SouthMode:
                //spriteRenderer.sprite = Resources.Load<Sprite>("SouthPlayer"); // Change to "SouthPlayer" sprite.
                break;
            case playerState.Neutral:
                // You can set a default sprite or leave it as is.
                break;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //checking pull or push objects
        if (currentState == playerState.NorthMode || currentState == playerState.SouthMode)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, magnetRadius);
            foreach (Collider2D collider in colliders)
            {
                //TODO wall walking, hmm
                if ((collider.name == "ElectroMagnet"))
                {
                    continue;
                }
                //regular polarity
                else if ((currentState == playerState.NorthMode && collider.CompareTag("NorthPolarity")) || (currentState == playerState.SouthMode && collider.CompareTag("SouthPolarity"))) // handles push 
                {
                    //Debug.Log("Push mode");
                    Vector2 direction = transform.position - collider.transform.position;
                    float distance = direction.magnitude;

                    Vector2 force = direction.normalized * (magnetForce * 0.5f); // Half the force to player 
                    playerRigidbody.AddForce(force, ForceMode2D.Impulse);

                    Rigidbody2D objectRigidbody = collider.GetComponent<Rigidbody2D>();
                    if (objectRigidbody != null)
                    {
                        objectRigidbody.AddForce(-force, ForceMode2D.Impulse); // half hte force on object
                    }
                }
                else if ((currentState == playerState.SouthMode && collider.CompareTag("NorthPolarity")) || (currentState == playerState.NorthMode && collider.CompareTag("SouthPolarity"))) // handles pull
                {
                    //TODO CHECK IF NOT ELECTRO MAGNET

                    //Debug.Log("in pull mode");
                    Vector2 direction = collider.transform.position - transform.position; //from collider to 
                    float distance = direction.magnitude;
                    Rigidbody2D objectRigidbody = collider.GetComponent<Rigidbody2D>();

                    //begin break force code when player on top
                    Vector2 breakDirectionVector = Vector2.zero;
                    if (Input.GetKeyUp("space")) 
                    {
                        //TODO breaking force might need work
                        
                        breakDirectionVector = new Vector2(0.0f, 1.0f); // Upwards direction
                        if (objectRigidbody != null)
                        {
                            Debug.Log("collider is" + collider.name + " using breaking force");
                            Vector2 breakForce = breakDirectionVector.normalized * breakingForce;
                            playerRigidbody.AddForce(breakForce, ForceMode2D.Impulse);

                            // Apply breaking force to the collider in the opposite direction
                            Vector2 breakForceCollider = -breakDirectionVector.normalized * breakingForce;
                            objectRigidbody.AddForce(breakForceCollider, ForceMode2D.Impulse);
                            continue;
                        }
                        //continue;
                    }


                    Vector2 force = direction.normalized * (magnetForce * 0.5f); // Half the force.
                    playerRigidbody.AddForce( force, ForceMode2D.Impulse);
                    if (objectRigidbody != null)
                    {
                        objectRigidbody.AddForce( -force, ForceMode2D.Impulse);
                        //playerRigidbody.AddForce(force, ForceMode2D.Impulse);
                    }
                    //end of attraction code

                    if (collider.CompareTag("NorthPolarity") || collider.CompareTag("SouthPolarity"))
                    {
                        UpdateLine(collider.transform);
                    }
                }

            }
        }
        
    }

    // Instantiate or update the LineRenderer.
    private void UpdateLine(Transform target)
    {
        LineManager lineManager = target.GetComponent<LineManager>();

        if (lineManager == null)
        {
            GameObject lineObject = new GameObject("Line");
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineManager = lineObject.AddComponent<LineManager>();
            lineManager.SetUpLine(new Transform[] { transform, target });

            // Set LineRenderer properties
            SetLineRendererProperties(lineRenderer, target.tag);
            Destroy(lineManager.gameObject, lineLifeTime);
        }
        else
        {
            lineManager.SetUpLine(new Transform[] { transform, target });
            Destroy(lineManager.gameObject, lineLifeTime);
        }
    }

    private void SetLineRendererProperties(LineRenderer lineRenderer, string polarityTag)
    {
        // Set width to 1/3rd of its normal size
        lineRenderer.startWidth = 0.20f;
        lineRenderer.endWidth = 0.20f;

        lineRenderer.material = lineMaterial;

        // Set color based on polarity
        Color startColor = polarityTag == "NorthPolarity" ? new Color(1, 0, 0, 0.5f) : new Color(0, 0, 1, 0.5f);
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Gradient to 0 alpha

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Exit")
        {
            currentState = playerState.Neutral;
        }
    }

    private void UpdateStateAnimation(playerState mode)
    {
        switch (mode)
        {
            case playerState.NorthMode:
                animator.SetBool("isNorth", true);
                animator.SetBool("isNeutral", false);
                animator.SetBool("isSouth", false);
                break;
            case playerState.Neutral:
                animator.SetBool("isNorth", false);
                animator.SetBool("isNeutral", true);
                animator.SetBool("isSouth", false);
                break;
            case playerState.SouthMode:
                animator.SetBool("isNorth", false);
                animator.SetBool("isNeutral", false);
                animator.SetBool("isSouth", true);
                break;
        }
    }
}
