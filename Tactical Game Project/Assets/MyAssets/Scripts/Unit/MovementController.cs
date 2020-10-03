using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FernandaDev
{
    public class MovementController : MonoBehaviour, ICommandController
    {
        public event Action OnCommandAnimationEnd;
        public Unit myUnit { get; private set; }
        public CommandType commandType { get; private set; }
        [SerializeField] float movementSpeed = 2f;
        Stack<Tile> path = new Stack<Tile>();

        private void Awake()
        {
            myUnit = GetComponent<Unit>();
            commandType = CommandType.Move;
        }

        public void MoveDirectlyTo(Tile to)
        {
            myUnit.transform.position = to.transform.position;
            myUnit.UpdateCurrentTile();
        }

        public void StartMove(Tile destinationTile)
        {
            CreatePath(destinationTile);

            if (path != null)
            {
                StartCoroutine(MoveUnit());
                //OnCommandStart?.Invoke();
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
            myUnit.UpdateCurrentTile();
        }

        public void CreatePath(Tile destinationTile)
        {
            if (path.Count > 0)
                path.Clear();

            //if (!selectableTiles.Contains(destinationTile) || !IsSelectionViable(destinationTile)) return;

            Tile currentTileToCheck = destinationTile;

            while (currentTileToCheck != null)
            {
                if (currentTileToCheck != myUnit.CurrentTile)
                    path.Push(currentTileToCheck);

                currentTileToCheck = currentTileToCheck.searchData.parent;
            }
        }

        public bool IsSelectionViable(Tile selectedTile)
        {
            if (selectedTile.HasUnitOver())
                return false;

            return true;
        }
    }
}