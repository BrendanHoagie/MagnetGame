using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControllerHorzontal : MonoBehaviour
{
    public GameObject Door;
    public LayerMask interactableObjectLayer;
    private bool doorIsOpening = false;
    public float stopAt;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.layer == interactableObjectLayer)
        {
            doorIsOpening = true;
        }
        if(other.gameObject.tag == "NorthPolarity" || other.gameObject.tag =="SouthPolarity")
        {
            doorIsOpening = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == interactableObjectLayer)
        {
            doorIsOpening = true;
        }
        if (other.gameObject.tag == "NorthPolarity" || other.gameObject.tag == "SouthPolarity")
        {
            doorIsOpening = true;
        }
    }

    void Update()
    {
        //Debug.Log("Door state is " + doorIsOpening);
        if (doorIsOpening)
        {
            Door.transform.Translate(Vector2.right * Time.deltaTime);

            if (Door.transform.position.x >= stopAt)
            {
                doorIsOpening = false;
            }
        }
    }
}
