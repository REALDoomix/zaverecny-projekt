using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;

    private void Start()
    {
        StartCoroutine(GenerateMaze(mazeSize));
    }

     IEnumerator GenerateMaze(Vector2Int size)
    {
        List<MazeNode> nodes = new List<MazeNode>();
        // Creating nodes
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 nodePos = new Vector3(x - (size.x / 2f), 0, y - (size.y / 2f));
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);

                yield return null;
            }
        }
        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        // Choosing the starting node
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);
        currentPath[0].SetState(NodeState.Current);

        while (completedNodes.Count < nodes.Count)
        {
            // Check nodes next to the current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count -1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            // Check the position of the current node and pick next possible direction and nodes
            // Next moves - 1 = positiveX | 2 = negativeX | 3 = positiveY | 4 = negativeY

            if(currentNodeX < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y])&& !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }    

            if (currentNodeX > 0)
            {
                //Check node to the left of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) && !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            
            if (currentNodeY < size.y - 1)
            {
                //Check node above the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) && !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }

            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) && !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }
        }
    }
}
