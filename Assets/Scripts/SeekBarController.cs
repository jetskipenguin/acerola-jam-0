using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBarController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    private int screenWidth;
    private int transformWidth;
    private int numTransforms;
    private float clipLength;
    private float clipTime;

    // Start is called before the first frame update
    void Start()
    {
        screenWidth = Screen.width;
        transformWidth = (int) Math.Floor(transform.GetComponent<RectTransform>().rect.width);
        clipLength = audioSource.clip.length;
        
        // find num of transforms that would fill up the screen horizontally
        numTransforms = screenWidth / transformWidth;
    }

    private void FixedUpdate()
    {
        // move seek bar across the screen
        clipTime = audioSource.time;
        int currTransformNum = (int) Math.Floor(numTransforms * (clipTime / clipLength));
        transform.position = new Vector3(currTransformNum * transformWidth, transform.position.y, transform.position.z);
    }
}
