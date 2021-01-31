using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DetectorRange : MonoBehaviour
{
    public LayerMask treasureSpace;
    public TilemapCollider2D treasureMap;
    public Vector2 ColliderSize;
    private bool isDetectingTreasure = false;
    
    void Start()
    {
        treasureMap = GameObject.FindGameObjectWithTag("Treasure").GetComponent<TilemapCollider2D>();
    }
    void Update()
    {
        //Debug.Log(name + " isDetectingTreasure = " + isDetectingTreasure);

        if (transform.GetComponent<BoxCollider2D>().IsTouching(treasureMap))
            isDetectingTreasure = true;
        else
            isDetectingTreasure = false;        

    }

    public bool GetTreasureStatus()
    {
        return isDetectingTreasure;
    }
}
