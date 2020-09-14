﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour, ICommandController
{
    public event Action OnCommandStart;
    public event Action OnCommandAnimationEnd;

    [SerializeField] float movementSpeed = 2f;

    Unit unit;
    Stack<Tile> path = new Stack<Tile>();

    private void Awake() => unit = GetComponent<Unit>();

    //TODO should the movement controller do this?

    public void CreatePath(Tile destinationTile, List<Tile> selectableTiles)
    {
        ResetPath();

        if (!selectableTiles.Contains(destinationTile) || destinationTile.HasUnitOver()) return;

        Tile currentTileToCheck = destinationTile;

        while (currentTileToCheck != null)
        {
            if (currentTileToCheck != unit.CurrentTile)
                path.Push(currentTileToCheck);

            currentTileToCheck = currentTileToCheck.parent;

            currentTileToCheck?.TileMaterialController.SetMaterialByType(CommandType.Hover);
        }

        destinationTile.TileMaterialController.SetMaterialByType(CommandType.Target);
    }

    private void ResetPath()
    {
        if (path == null) return;

        foreach (var tile in path)
        {
            if (!tile.HasUnitOver())
                tile.TileMaterialController.SetMaterialByType(CommandType.Move);
            else
                tile.TileMaterialController.SetMaterialByType(CommandType.Default);
        }

        path.Clear();
    }

    public void MoveDirectlyTo(Tile to)
    {
        unit.transform.position = to.transform.position;
    }

    public void StartMove(Tile destinationTile)
    {
        if (path != null)
        {
            StartCoroutine(MoveUnit());
            OnCommandStart?.Invoke();
        }
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
        OnCommandAnimationEnd?.Invoke();
        unit.UpdateCurrentTile();
    }

    public bool IsSelectionViable(Tile selectedTile)
    {
        if (selectedTile.HasUnitOver())
            return false;

        return true;
    }
}