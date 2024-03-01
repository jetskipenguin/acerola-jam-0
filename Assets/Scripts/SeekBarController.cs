using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBarController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    private int screenWidth;
    private int screenHeight;
    private int transformWidth;
    private int numTransforms;
    private float clipLength;
    private float clipTime;

    // Start is called before the first frame update
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        transformWidth = (int) Math.Floor(transform.GetComponent<RectTransform>().rect.width);
        clipLength = audioSource.clip.length;
        
        // find num of transforms that would fill up the screen horizontally
        numTransforms = screenWidth / transformWidth;
    }

    private void FixedUpdate()
    {
        clipTime = audioSource.time;
        Debug.Log(clipTime / clipLength);
        int currTransformNum = (int) Math.Floor(numTransforms * (clipTime / clipLength));
        Debug.Log(currTransformNum);
        transform.position = new Vector3(currTransformNum * transformWidth, transform.position.y, transform.position.z);
    
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
