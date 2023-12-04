using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    Transform cam;
    Vector3 startCamPos;
    float distance;

    GameObject[] backgrounds;
    Material[] mats;
    float[] backSpeed;

    float furthestBack;

    [Range(0f, 0.5f)]
    public float parallaxSpeed;

    private void Start()
    {
        cam = Camera.main.transform;
        startCamPos = cam.position;

        int backCount = transform.childCount;
        mats = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for(int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mats[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        // gets furthest background
        for(int i = 0; i < backCount; i++)
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > furthestBack)
            {
                furthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }
        }

        // sets speed for backgrounds
        for (int i = 0; i < backCount; i++)
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / furthestBack;
        }
    }

    private void LateUpdate()
    {
        distance = cam.position.x - startCamPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y, 0);
        for(int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            mats[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }

}
