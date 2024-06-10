using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos;
    private float length;
    private GameObject cam;
    [SerializeField] private float parallaxEffect;


    void Start()
    {
        cam = GameObject.Find("FollowPlayer");
        startPos = transform.position.x;
       // length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    
    void Update()
    {
      //  float temp = (cam.transform.position.x * (1 - parallaxEffect));

        float distace = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos+ distace, transform.position.y, transform.position.z);
        //transform.position = new Vector3(cam.transform.position, transform.position.y, 0);
        
        //if (temp > startPos + length)
        //{
        //    startPos += length;
        //}
        //else if (temp < startPos + length)
        //{
        //    startPos -= length;
        //}
    }
}
