using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMovement : MonoBehaviour
{
    [SerializeField] float BackgroundSpeed = 0.03f;
    Material myMaterial;
    Vector2 offSet;

    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offSet = new Vector2(0, BackgroundSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.mainTextureOffset += offSet * Time.deltaTime; 
    }
}
