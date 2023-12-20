using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Available,
    Current,
    Completed,
    Finish,
    Obstacle
}

public class MazeNode : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject floorWithHole;

    public string finishNode = "FinishNode";

    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].gameObject.SetActive(false);
    }


    public void SetState(NodeState state)
    {



        switch (state)
        {
            case NodeState.Available:
                floor.GetComponent<MeshRenderer>().material.color = Color.white;
                break;
            case NodeState.Current:
                floor.GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;
            case NodeState.Completed:
                floor.GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            case NodeState.Finish:
                floor.GetComponent<MeshRenderer>().material.color = Color.blue;
                floor.tag = finishNode;
                floorWithHole.gameObject.SetActive(false);
                break;
            case NodeState.Obstacle:
                floor.gameObject.SetActive(false);
                break;
        }
    }
}
