using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Gameplay Variables + Unchanging References")]
    public float moveSpeed = 5;
    public Transform movePoint;
    public GameObject MetalDetector;
    public GameObject MetalDetectorRange;

    public string currentDirection = "";

    public Sprite[] PlayerPositions;
    public Sprite[] MetalDetectorPositions;

    [Header("References to Tilemaps")]
    public Grid masterGrid;
    public Tilemap groundTilemap;
    public Tilemap treasureTilemap;
    public Tilemap treasureDisplay; 
    public LayerMask whatStopsMovement;
    public LayerMask walkableGround;
    public LayerMask treasureSpace;

    [Header("Hidden Gameplay Variables")]
    private bool isOnTreasure = false;
    private IEnumerator displayTreasureCoroutine;
    
    
    


    void Start()
    {
        movePoint.parent = null;
        masterGrid = GameObject.FindGameObjectWithTag("MasterGrid").GetComponent<Grid>();
        groundTilemap = GameObject.FindGameObjectWithTag("Ground").GetComponent<Tilemap>();
        treasureTilemap = GameObject.FindGameObjectWithTag("Treasure").GetComponent<Tilemap>();
        treasureDisplay = GameObject.FindGameObjectWithTag("TreasureDisplay").GetComponent<Tilemap>();

        currentDirection = "down";
        UpdatePlayerSprites(currentDirection);
    }

    // Update is called once per frame
    void Update()
    {
        //TreasureCheck();

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed*Time.deltaTime);

        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
            MovePointer();

        //If player presses space, game checks for treasure
        PlayerSearchForTreasure();
        
    }

    private void PlayerSearchForTreasure()
    {
        if (Input.GetKeyDown("space"))
        {
            if (isOnTreasure)
            {
                Debug.Log("Found Treasure!");
                //Get the tile used for the treasure
                Vector3Int playerLoc = masterGrid.WorldToCell(transform.position);
                TileBase treasureTile = treasureTilemap.GetTile(playerLoc);
                treasureTilemap.SetTile(playerLoc, null);
                displayTreasureCoroutine = ShowTreasureThenRemove(playerLoc + new Vector3Int(0, 1, 0), treasureTile);
                StartCoroutine(displayTreasureCoroutine);
				TreasureCount--;
				if (TreasureCount <= 0)
				{
					clearStage();
					stageNo++;
					newStage();
				}
            }
            else
            {
                Debug.Log("No Treasure! Time Lost!");
            }
        }
    }

    IEnumerator ShowTreasureThenRemove(Vector3Int location, TileBase treasure)
    {
        //Debug.Log("Running ShowTreasureThenRemove");
        treasureDisplay.SetTile(location, treasure);
        yield return new WaitForSeconds(1f);
        treasureDisplay.SetTile(location, null);
        //Debug.Log("Ending ShowTreasureThenRemove");
        yield return null;
    }
    private void TreasureCheck()
    {
        //Checks for if over Treasure
        if (Physics2D.OverlapCircle(movePoint.position, 0.2f, treasureSpace))
            isOnTreasure = true;
        else
            isOnTreasure = false;
            
        if (isOnTreasure)
        {
            Debug.Log("Standing on Treasure");
        }
    }

    private void MovePointer()
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            string orientation = "";
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                int dir = (int)Input.GetAxisRaw("Horizontal");

                if (dir == -1)
                {
                    MetalDetector.transform.localPosition = new Vector3Int(-1, 0, 0);
                    MetalDetectorRange.transform.rotation = Quaternion.Euler(Vector3.forward * -90);
                    orientation = "left";
                }
                else if (dir == 1)
                {
                    MetalDetector.transform.localPosition = new Vector3Int(1, 0, 0);
                    MetalDetectorRange.transform.rotation = Quaternion.Euler(Vector3.forward * 90);

                    orientation = "right";
                }
                
                UpdatePlayerSprites(orientation);

                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, whatStopsMovement) 
                    && (Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, walkableGround))
                    && orientation == currentDirection)
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }

                currentDirection = orientation;
            }

            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                int dir = (int)Input.GetAxisRaw("Vertical");
                
                if (dir == -1)
                {
                    MetalDetector.transform.localPosition = new Vector3(-0.06f, -1, 0);
                    MetalDetectorRange.transform.rotation = Quaternion.Euler(Vector3.forward * 0);
                    orientation = "down";

                }
                else if (dir == 1)
                {
                    MetalDetector.transform.localPosition = new Vector3Int(0, 1, 0);
                    MetalDetectorRange.transform.rotation = Quaternion.Euler(Vector3.forward * 180);
                    orientation = "up";
                }

                UpdatePlayerSprites(orientation);
                
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.2f, whatStopsMovement)
                    && (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.2f, walkableGround))
                    && currentDirection == orientation)
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }

                currentDirection = orientation;
            }
        }
        TreasureCheck();

    }

    private void UpdatePlayerSprites(string dir)
    {
        if (dir == "up")
        {
            transform.GetComponent<SpriteRenderer>().sprite = PlayerPositions[0];
            MetalDetector.GetComponent<SpriteRenderer>().sprite = MetalDetectorPositions[0];
        }
        else if (dir == "down")
        {
            transform.GetComponent<SpriteRenderer>().sprite = PlayerPositions[1];
            MetalDetector.GetComponent<SpriteRenderer>().sprite = MetalDetectorPositions[1];
        }
        else if (dir == "left")
        {
            transform.GetComponent<SpriteRenderer>().sprite = PlayerPositions[2];
            MetalDetector.GetComponent<SpriteRenderer>().sprite = MetalDetectorPositions[2];
        }
        else if (dir == "right")
        {
            transform.GetComponent<SpriteRenderer>().sprite = PlayerPositions[3];
            MetalDetector.GetComponent<SpriteRenderer>().sprite = MetalDetectorPositions[3];
        }
    }

}
