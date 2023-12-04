using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectPolarityHandler : MonoBehaviour
{
    public float polarityRange = 2;
    public float magnetForce = 2;

    private string polarityTag; // The tag for this object, set in the Unity inspector.
    private string colliderTag = "";
    private Rigidbody2D myRigidbody;
    private Vector2 renderSize;
    private float size;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        renderSize = GetComponent<Collider2D>().bounds.size;
        size = renderSize.x + renderSize.y;
        polarityRange *= size;
        magnetForce *= size;
    }

    void Update()
    {
        polarityTag = gameObject.tag;

        //Boolean isInRange = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircleRadius, groundLayer);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, polarityRange);


        foreach (Collider2D collider in colliders)
        {
            colliderTag = collider.gameObject.tag;

            if ((polarityTag == "NorthPolarity" && colliderTag == "NorthPolarity") || (polarityTag == "SouthPolarity" && colliderTag == "SouthPolarity")) // rejection
            {
                Vector2 direction = transform.position - collider.transform.position;
                float distance = direction.magnitude;

                // Debug.Log(string.Format("direction is {0} and distance is {1}", direction, distance));

                if (distance < polarityRange)
                {
                    Rigidbody2D colliderRB = collider.GetComponent<Rigidbody2D>();

                    if (colliderRB != null && myRigidbody != null)
                    {
                        Vector2 force = direction.normalized * magnetForce;
                        myRigidbody.AddForce(force, ForceMode2D.Force);
                        colliderRB.AddForce(-force, ForceMode2D.Force);
                    }
                }


            }
            else if ((polarityTag == "NorthPolarity" && colliderTag == "SouthPolarity") || (polarityTag == "SouthPolarity" && colliderTag == "NorthPolarity"))  //attraction 
            {
                Vector2 direction = collider.transform.position - transform.position;
                float distance = direction.magnitude;

                // Debug.Log(string.Format("direction is {0} and distance is {1}", direction, distance));

                if (distance < polarityRange)
                {
                    Rigidbody2D colliderRB = collider.GetComponent<Rigidbody2D>();

                    if (colliderRB != null && myRigidbody != null)
                    {
                        Vector2 force = direction.normalized * magnetForce;
                        myRigidbody.AddForce(force, ForceMode2D.Force);
                        colliderRB.AddForce(-force, ForceMode2D.Force);
                    }
                }

            }
        }
    }
}