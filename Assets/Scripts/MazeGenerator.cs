using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] GameObject objectPrefab;
    private Vector2Int mazeSize;
    [SerializeField]GameObject mainCamera;


    private void Start()
    {
        mazeSize = new Vector2Int(PlayerPrefs.GetInt("size"), PlayerPrefs.GetInt("size"));
        GenerateMazeInstant(mazeSize);
        if(PlayerPrefs.GetInt("size") == 20){
             mainCamera.GetComponent<Camera>().orthographicSize = 10;
        }
    }

    void randomObstacle(){
        
    }

    void GenerateMazeInstant(Vector2Int size)
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

            }
        }
        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        // Choosing the starting node
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);

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
            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);

                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }                
                currentPath.Add(chosenNode);
            }

            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);
                currentPath.RemoveAt(currentPath.Count -1);
            }

        }
        // Choose a random completed node to place the object
        MazeNode randomCompletedNodeFinish = completedNodes[Random.Range(0, completedNodes.Count)];
        // Change the color of the chosen node for better visibility of the finish node
        randomCompletedNodeFinish.SetState(NodeState.Finish);
        // Instantiate the object prefab at the position of the chosen node with chosen rotation and offset
        Quaternion rotation = Quaternion.Euler(0, 180, 0);
        Vector3 finishPosition = randomCompletedNodeFinish.transform.position + new Vector3(0, 10, 0);
        Instantiate(objectPrefab, finishPosition, rotation);
        

        for(int i = 0; i<(size.x * size.y / 10); i++){
            MazeNode randomCompletedNode = completedNodes[Random.Range(0, completedNodes.Count)];
            if(randomCompletedNode.transform.position != randomCompletedNodeFinish.transform.position && randomCompletedNode.transform.position != new Vector3(0, 0, 0)){
                Debug.Log(randomCompletedNodeFinish.transform.position + "   " + randomCompletedNode.transform.position);
                randomCompletedNode.SetState(NodeState.Obstacle);
            }
        
        }
    }
}
