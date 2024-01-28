using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    public int size = 10;                               //!@ Move into settings
    public Vector2Int holes = new Vector2Int(25, 75);   //!@ Move into settings
    //public float waitingTimeBeforeStart = 1f;
    //public bool timeLimited = true;
    //public float timeIteration = 0.1f;
    //public int stepIteration = 10;
    //public bool generate = false;

    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _ceilPrefab;
    [SerializeField] private GameObject _floorPrefab;
    //[SerializeField] private GameObject _cubePrefab;
    private Cell[] _cells;
    private float _wallSize;
    private DisjointSet _sets;

    public Vector2Int playerSize = new Vector2Int(5, 15);   //!@ Move into settings
    public Vector2Int itemSize = new Vector2Int(5, 15);     //!@ Move into settings
    private List<int> playerSpawns;
    private List<int> itemSpawns;

    public void GenerateMaze()
    {
        //if (generate)
        //{
        //generate = false;
        _cells = new Cell[size * size];
        SpawnEntireGrid(size);
        //StartCoroutine(RanMaze());
        RanMaze();
        //GenerateSpawns();
        //}
    }

    private void SpawnEntireGrid(int size)
    {
        //deleting all walls in order to generate a new maze
        Transform parent = this.gameObject.transform;
        GameObject[] prefabs = (GameObject.FindGameObjectsWithTag("prefabs"));
        foreach (GameObject prefab in prefabs) GameObject.Destroy(prefab);


        //spawning walls
        _wallSize = _wallPrefab.transform.localScale.y;
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                Vector3 positionCell = new Vector3(x * _wallSize, 0, z * _wallSize);
                Cell newCell = new Cell(x, z, size, positionCell);
                _cells[x * size + z] = newCell;

                Quaternion wallRotation = Quaternion.Euler(0f, 90f, 0f);
                Vector3 positionWall = positionCell + new Vector3(_wallSize / 2f, 0f, 0f);
                GameObject newWall = Instantiate(_wallPrefab, positionWall, wallRotation, parent);
                newCell.AddWall(1, newWall);

                positionWall = positionCell + new Vector3(0f, 0f, _wallSize / 2f);
                newWall = Instantiate(_wallPrefab, positionWall, Quaternion.identity, parent);
                newCell.AddWall(0, newWall);

                if (x == 0)
                {
                    Quaternion wallRotated = Quaternion.Euler(0f, 90f, 0f);
                    positionWall = positionCell + new Vector3(-_wallSize / 2f, 0f, 0f);
                    newWall = Instantiate(_wallPrefab, positionWall, wallRotated, parent);
                    newCell.AddWall(3, newWall);
                }
                else
                {
                    newCell.AddWall(3, _cells[(x - 1) * size + z].GetWall(1));
                }

                if (z == 0)
                {
                    positionWall = positionCell + new Vector3(0f, 0f, -_wallSize / 2f);
                    newWall = Instantiate(_wallPrefab, positionWall, Quaternion.identity, parent);
                    newCell.AddWall(2, newWall);
                }
                else
                {
                    newCell.AddWall(2, _cells[x * size + z - 1].GetWall(0));
                }

                //Floor generation
                Quaternion fcRot = Quaternion.Euler(90f, 0f, 0f);
                Vector3 positionFloor = positionCell + new Vector3(0f, -_wallSize / 2f, 0f);
                GameObject newFloor = Instantiate(_floorPrefab, positionFloor, fcRot, parent);
                newCell.AddFloor(newFloor);

                //Ceiling generation
                Vector3 positionceil = positionCell + new Vector3(0f, _wallSize / 2f, 0f);
                GameObject newceil = Instantiate(_ceilPrefab, positionceil, fcRot, parent);
                if (newceil != null)
                {
                    /*
                    MeshRenderer MR = newceil.GetComponent<MeshRenderer>();                    
                    if (MR != null)
                    {
                        bool state = (!GameManager.instance.DEBUG);
                        MR.enabled = state;
                    }
                    */
                    newCell.AddCeil(newceil);
                }
            }
        }
    }


    //private IEnumerator RanMaze()
    private void RanMaze()
    {
        _sets = new DisjointSet(size * size);
        for (int i = 0; i < size * size; i++) _sets.MakeSet(i);
        //yield return new WaitForSeconds(waitingTimeBeforeStart);
        int source = 0; //low left corner
        int target = size * size - 1; //top right corner
        int iterations = 0; //only for 'visualizing' purposes 
        while (_sets.FindSet(source) != _sets.FindSet(target))
        {
            int randomIndexCell = Random.Range(0, size * size);
            Cell randomCell = _cells[randomIndexCell];
            int destroyIndex = 0;
            int wallCount = Random.Range(1, 4);

            for (destroyIndex = 0; destroyIndex < wallCount; destroyIndex++)
            {
                int randomWall = Random.Range(0, 4);
                DestroyWall(randomCell, randomIndexCell, randomWall, true);
            }

            iterations++;
            //if(timeLimited && iterations%stepIteration==0)yield return new WaitForSeconds(timeIteration);
        }
        //yield return new WaitForSeconds(0.1f);
        //yield return null;
        MakeHoles();
        //StartCoroutine(Dfs());
    }

    private void CreateSpawns()
    {
        int max = size * size;
        int maxPlayers = GetValueInRange(playerSize);
        int maxItems = GetValueInRange(itemSize);

        //!@ Insert checks here for islands
        playerSpawns = new List<int>(maxPlayers);
        itemSpawns = new List<int>(maxPlayers);

        int i = 0;
        int index = 0;
        bool found = false;
        bool found2 = false;
        for (i = 0; i < maxPlayers; i++)
        {
            index = UnityEngine.Random.Range(0, max);
            found = (playerSpawns.Contains(index));
            if (!found){playerSpawns[i] = index;}
        }

        for (i = 0; i < maxItems; i++)
        {
            index = UnityEngine.Random.Range(0, max);
            found = itemSpawns.Contains(index);
            found2 = playerSpawns.Contains(index);
            if (!found && !found2) { playerSpawns[i] = index; }
        }
    }

    private void DestroyWall(Cell randomCell, int randomIndexCell, int randomWall, bool checks)
    {
        GameObject obj = randomCell.GetWall(randomWall);
        if (obj == null){return;}

        if (checks)
        {
            int indexNeighbour = -1;
            if (!IsPerimetralWall(randomIndexCell, randomWall))
            {
                if (randomWall == 0) indexNeighbour = randomCell.GetIndex() + 1;
                if (randomWall == 1) indexNeighbour = randomCell.GetIndex() + size;
                if (randomWall == 2) indexNeighbour = randomCell.GetIndex() - 1;
                if (randomWall == 3) indexNeighbour = randomCell.GetIndex() - size;
            }

            if (indexNeighbour >= 0 && indexNeighbour < size * size)
            {
                if (_sets.FindSet(indexNeighbour) != _sets.FindSet(randomIndexCell)) //not reachable
                {
                    randomCell.DestroyWall(randomWall);
                    _sets.UnionSet(indexNeighbour, randomIndexCell);
                }
            }
        }
        else
        {
            if (!IsPerimetralWall(randomIndexCell, randomWall))
            {
                randomCell.DestroyWall(randomWall);
            }
        }
    }

    private void MakeHoles()
    {
        int i = 0;
        int max = size * size;
        int hole = GetValueInRange(holes);
        for (i = 0; i < hole; i++)
        {
            int index = UnityEngine.Random.Range(0, max);
            int wallindex = 0;
            for (wallindex = 0; wallindex < 4; wallindex++)
            {
                DestroyWall(_cells[index], index, wallindex, false);
            }
        }
    }

    private bool IsPerimetralWall(int cellIndex, int wallIndex)
    {
        if (cellIndex % size == 0 && wallIndex == 2) return true;
        if (cellIndex % size == size - 1 && wallIndex == 0) return true;
        if ((cellIndex / size) == size - 1 && wallIndex == 1) return true;
        return (cellIndex / size) == 0 && wallIndex == 3;
    }

    private int GetValueInRange(Vector2Int rng)
    {
        int max = size * size;
        int low = (int)(max * ((float)(rng.x) / 100f));
        int hi = (int)(max * ((float)(rng.y) / 100f));
        int result = UnityEngine.Random.Range(low, hi + 1);
        return result;
    }


    private IEnumerator Dfs()
    {
        Stack<Cell> stack = new Stack<Cell>();
        var predecessors = new int[size * size];
        for (int i = 0; i <= size * size - 1; i++) predecessors[i] = -1;

        var source = _cells[0];
        stack.Push(source);
        Cell node = source;

        while (predecessors[size * size - 1] == -1)
        {
            var indexNode = node.GetIndex();
            var z = indexNode % size;
            var x = (indexNode - z) / size;
            if (x > 0 && predecessors[indexNode - size] == -1 && _sets.FindSet(indexNode - size) == _sets.FindSet(0))
            {
                if (node.GetWall(3) == null)
                {
                    predecessors[indexNode - size] = indexNode;
                    stack.Push(_cells[indexNode - size]);
                }
            }

            if (x < size - 1)
            {
                if (predecessors[indexNode + size] == -1 && _sets.FindSet(indexNode + size) == _sets.FindSet(0))
                {
                    if (node.GetWall(1) == null)
                    {
                        predecessors[indexNode + size] = indexNode;
                        stack.Push(_cells[indexNode + size]);
                    }
                }
            }

            if (z < size - 1)
            {
                if (predecessors[indexNode + 1] == -1 && _sets.FindSet(indexNode + 1) == _sets.FindSet(0))
                {
                    if (node.GetWall(0) == null)
                    {
                        predecessors[indexNode + 1] = indexNode;
                        stack.Push(_cells[indexNode + 1]);
                    }
                }
            }

            if (z > 0)
            {
                if (predecessors[indexNode - 1] == -1 && _sets.FindSet(indexNode - 1) == _sets.FindSet(0))
                {
                    if (node.GetWall(2) == null)
                    {
                        predecessors[indexNode - 1] = indexNode;
                        stack.Push(_cells[indexNode - 1]);
                    }
                }
            }
            node = stack.Pop();
        }

        //visualizing path
        /*
        var backtrackerIndex = size * size - 1;
        while (backtrackerIndex!=0) //0 is index of the source
        {
            Instantiate(_cubePrefab, _cells[backtrackerIndex].GetWorldPosition(), Quaternion.identity);
            backtrackerIndex = predecessors[backtrackerIndex];
        }
        Instantiate(_cubePrefab, _cells[backtrackerIndex].GetWorldPosition(), Quaternion.identity);
        */
        yield return null;
    }
}