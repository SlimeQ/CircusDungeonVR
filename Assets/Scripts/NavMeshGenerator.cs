using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshGenerator : MonoBehaviour
{
    public LoadScene scene;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ResetScene();
        //if (Input.GetKeyDown(KeyCode.N))
        //{

        //}
    }

    private void ResetScene()
    {
        bool reset = Input.GetKeyDown(KeyCode.Tab);
        if(reset){scene.LoadSceneDelayed(0f);}
    }

    public void InitNavMesh()
    {
        NavMeshSurface surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }
}
