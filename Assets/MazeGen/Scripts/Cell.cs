using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private GameObject[] _walls;
    private GameObject _floor;
    private GameObject _ceil;
    private int _index;
    private Vector3 _worldPosition;
    public Cell(int x, int y, int sizeMaze, Vector3 worldPosition)
    {
        _index = x * sizeMaze + y;
        _walls = new GameObject[4];
        _worldPosition = worldPosition;
    }
    
    public int GetIndex()
    {
        return _index;
    }

    #region Walls
    public void AddWall(int index,GameObject wall)
    {
        _walls[index] = wall;
    }
    
    public GameObject GetWall(int index)
    {
        GameObject w = _walls[index];
        return w;
    }

    public void DestroyWall(int index)
    {
        GameObject w = _walls[index];
        DestroyThing(w);
    }

    private void DestroyThing(GameObject t)
    {
        bool exists = (t != null);
        if (exists) { GameObject.Destroy(t); }
    }
    #endregion

    #region Ceil
    public void AddCeil(GameObject ceil){_ceil = ceil;}
    public GameObject GetCeil(){return _ceil;}
    public void DestroyCeil(){DestroyThing(_ceil);}
    #endregion

    #region Floor
    public void AddFloor(GameObject floor) { _floor = floor; }
    public GameObject GetFloor() { return _floor; }
    public void DestroyFloor() { DestroyThing(_floor); }
    #endregion

    public Vector3 GetWorldPosition()
    {
        return _worldPosition;
    }
}
