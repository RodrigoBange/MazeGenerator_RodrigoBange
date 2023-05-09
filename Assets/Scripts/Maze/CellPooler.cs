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

    private CellController CreateCell()
    {
        var cell = Instantiate(cellPrefab);
        cell.transform.parent = gameObject.transform;
        return cell;
    }

    private void OnTakeCellFromPool(CellController cell)
    {
        cell.gameObject.SetActive(true);
    }

    private void OnReturnCellToPool(CellController cell)
    {
        cell.gameObject.SetActive(false);
    }
}
