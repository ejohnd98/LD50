using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessScroller : MonoBehaviour
{
    public Vector2 dimensions;
    public float counter = 0.0f;
    public float moveSpeed = 1.0f;
    Vector3 posA, posB;

    private void Start() {
        posA = transform.position;
        posB = transform.position + (Vector3.down * dimensions.y);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(posA, posB, counter);
        counter += Time.deltaTime * moveSpeed;
        if(counter >= 1.0){
            counter = 0.0f;
        }
    }
}
