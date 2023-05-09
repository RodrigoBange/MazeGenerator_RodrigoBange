using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CellPooler : MonoBehaviour
{
    public ObjectPool<CellController> pool;

    [SerializeField]
    CellController cellPrefab;

    private void Awake()
    {
        // Initialize pool
        pool = new ObjectPool<CellController>(CreateCell, OnTakeCellFromPool, OnReturnCellToPool);
    }

    /// <summary>
    /// Creates a new CellController object.
    /// </summary>
    /// <returns>Returns a CellController object.</returns>
    private CellController CreateCell()
    {
        var cell = Instantiate(cellPrefab);
        cell.transform.parent = gameObject.transform;
        return cell;
    }

    /// <summary>
    /// Activates a CellController object in the pool.
    /// </summary>
    /// <param name="cell">The cell to activate.</param>
    private void OnTakeCellFromPool(CellController cell)
    {
        cell.gameObject.SetActive(true);
    }

    /// <summary>
    /// Disabled a CellController object in the pool.
    /// </summary>
    /// <param name="cell">The cell to deactivate.</param>
    private void OnReturnCellToPool(CellController cell)
    {
        cell.gameObject.SetActive(false);
    }
}
