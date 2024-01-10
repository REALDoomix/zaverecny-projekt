using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Finish,
    Obstacle
}

public class MazeNode : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] GameObject floor;
    [SerializeField] MeshRenderer floorWithHole;

    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].gameObject.SetActive(false);
    }

    
    public void SetState(NodeState state)
    {
        switch (state)
        {
            case NodeState.Finish:
                floor.GetComponent<MeshRenderer>().material.color = Color.blue;
                floorWithHole.gameObject.SetActive(false);
                this.gameObject.tag = "Finish";
                break;
            case NodeState.Obstacle:
                floor.gameObject.SetActive(false);
                break;
        }
    }
}
