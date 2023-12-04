using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChange : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Physics2D.gravity = new Vector2(0, 9.8f);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Physics2D.gravity = new Vector2(0, -9.8f);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Physics2D.gravity = new Vector2(-9.8f, 0);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Physics2D.gravity = new Vector2(9.8f, 0);
        }
    }
}
