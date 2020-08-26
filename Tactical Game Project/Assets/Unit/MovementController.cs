using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Action OnMovementEnd;
    
    [SerializeField] float movementSpeed = 2f;

    Unit unit;
    Stack<Tile> path = new Stack<Tile>();

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void Start() { }

    public void Move(Tile destinationTile)
    {
        SetPath(destinationTile);
    }

    void SetPath(Tile destinationTile)
    {
        path.Clear();

        Tile currentTileToCheck = destinationTile;

        while (currentTileToCheck != null)
        {
            if (currentTileToCheck != unit.CurrentTile)
                path.Push(currentTileToCheck);

            currentTileToCheck = currentTileToCheck.parent;
        }

        if(path != null)
            StartCoroutine(MoveUnit());
    }

    IEnumerator MoveUnit()
    {
        if (path.Count > 0)
        {
            Tile tile = path.Peek(); // peek gets the first item in the stack but doesnt remove it.(Pop() does remove)
            Vector3 targetTilePosition = tile.transform.position;

            while (true)
            {
                if (transform.position == targetTilePosition)
                {
                    path.Pop();

                    if (path.Count <= 0)
                    {
                        EndMove();

                        yield break;
                    }

                    tile = path.Peek();
                    targetTilePosition = tile.transform.position;
                    //yield return StartCoroutine(RotateTowards(tile.transform));
                }

                transform.position = Vector3.MoveTowards(transform.position, 
                                                            targetTilePosition, 
                                                            movementSpeed * Time.deltaTime);
                transform.LookAt(2 * transform.position - targetTilePosition);
                yield return null;
            }
        }
    }

    private void EndMove()
    {
        OnMovementEnd();
        unit.UpdateCurrentTile();
    }
}

