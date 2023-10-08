// MazeGen = Aldous-Broder algorithm
// https://en.wikipedia.org/wiki/Maze_generation_algorithm
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeGen : MonoBehaviour
{    
    public GameObject wall;
    public bool[,] visited;
    public bool[,] hwalls; 
    public bool[,] vwalls; 
    public int size;
    private int unvisited = 0;

    public bool generateWallMaze = false;
    public bool generatePillarMaze = false;
    public float scale = 5;

    int rand(int size){
        return (int) Random.Range(0.0f, (float)size - 0.001f);
    }

    int[] pick(int[,] neighbors){
        int n = rand(4);
        // https://stackoverflow.com/questions/27427527/how-to-get-a-complete-row-or-column-from-2d-array-in-c-sharp/51241629#51241629
        return Enumerable.Range(0, 2)
                .Select(x => neighbors[n, x])
                .ToArray();
    }

    bool checkValid(int[] point){
        bool ival = 0 <= point[0] && point[0] < size;
        bool jval = 0 <= point[1] && point[1] < size;
        return ival && jval;
    }

    int[] pickValid(int[,] neighbors){
        var n = pick(neighbors);
        bool b = checkValid(n);
        while(!b){
            n = pick(neighbors);
            b = checkValid(n);
        }
        return n;
    }

    int[] pickNeighbor(int[] p){
        var i = p[0];
        var j = p[1];
        int[,] neighbors = new int[,]{
             {i-1 , j}
            ,{i+1 , j}
            ,{i   , j+1}
            ,{i   , j-1}
        };
        // always 4 possible neighbors (some are invalid, we check that)
        return pickValid(neighbors);
    }

    void visit(int[] p){
        visited[ p[0] , p[1] ] = true;
        unvisited++;
        printCurr(p);
    }


    string pointString(int[] p){
        return p[0].ToString() + "," + p[1].ToString();
    }

    void printCurr(int[] p){
        string s = pointString(p);
        // Debug.Log("visiting: " + unvisited.ToString() + " : " + s);
    }

    bool isvisited(int[] p){
        return visited[ p[0] , p[1] ];
    }

    void removeWall(int[] p1, int[] p2){
        int dx = p2[0] - p1[0];
        int dy = p2[1] - p1[1];
        if(dx == -1){
            var i = p1[0];
            var j = p1[1];
            vwalls[ i, j ] = false;
            // Debug.Log("Removing vwall " + i.ToString() + "," + j.ToString());
        }
        if(dy == -1){
            var i = p1[0];
            var j = p1[1];
            hwalls[ i, j ] = false;
            // Debug.Log("Removing hwall " + i.ToString() + "," + j.ToString());
        }
        if(dx == 1){
            var i = p1[0]+dx;
            var j = p1[1];
            vwalls[ i, j ] = false;
            // Debug.Log("Removing vwall " + i.ToString() + "," + j.ToString());
        }
        if(dy == 1){
            var i = p1[0];
            var j = p1[1]+dy;
            hwalls[ i, j ] = false;
            // Debug.Log("Removing hwall " + i.ToString() + "," + j.ToString());
        }
    }

    void AldousBroder(){
        // start by picking a random cell.
        // int[] p = {rand(size), rand(size)};
        int[] p = {0,0};
        visit(p);

        while(unvisited < size*size){
            var next = pickNeighbor(p);
            if(!isvisited(next)){
                visit(next);
                removeWall(p, next);
            }
            p = next;
        }
    }

    void fill(bool[,] M, bool b){
        for (int i = 0; i < size+1; i++)
        {
            for (int j = 0; j < size+1; j++)
            {
                M[i,j] = b;
            }
        }
    }

    void instantiateWallMaze(){
        for (int i = 0; i < size+1; i++)
        {
            for (int j = 0; j < size+1; j++)
            {
                if( hwalls[i,j] && i < size){
                    var prefab = Instantiate(wall, new Vector3(i*scale, 0, j*scale-scale/2), Quaternion.identity);
                    prefab.transform.localScale = new Vector3(scale, scale/4, scale/4);
                    prefab.name = "hwall_{" + i.ToString() + "," + j.ToString()+"}";
                }
                if( vwalls[i,j] && j < size ){
                    var prefab = Instantiate(wall, new Vector3(i*scale-scale/2, 0, j*scale), Quaternion.identity);
                    prefab.transform.localScale = new Vector3(scale/4, scale/4, scale);
                    prefab.name = "vwall_{" + i.ToString() + "," + j.ToString()+"}";
                }
            }
        }
    }

    void instantiatePillarMaze(){
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if( !hwalls[i,j] && i < size){
                    var prefab = Instantiate(wall, new Vector3(i*scale, 0, j*scale-scale/2), Quaternion.identity);
                    prefab.transform.localScale = new Vector3(scale/8, scale/40, scale);
                    prefab.name = "vwall_{" + i.ToString() + "," + j.ToString()+"}";
                }
                if( !vwalls[i,j] && j < size ){
                    var prefab = Instantiate(wall, new Vector3(i*scale-scale/2, 0, j*scale), Quaternion.identity);
                    prefab.transform.localScale = new Vector3(scale, scale/40, scale/8);
                    prefab.name = "hwall_{" + i.ToString() + "," + j.ToString()+"}";
                }
                var pillar = Instantiate(wall, new Vector3(i*scale, -scale*5/2, j*scale), Quaternion.identity);
                pillar.transform.localScale = new Vector3(scale/4, scale*5, scale/4);
            }
        }
    }

    void Start(){
        visited = new bool[size,size];
        hwalls = new bool[size+1,size+1];
        vwalls = new bool[size+1,size+1];
        fill(hwalls, true);
        fill(vwalls, true);
        AldousBroder();
        if(generateWallMaze) instantiateWallMaze();
        if(generatePillarMaze) instantiatePillarMaze();
    }

}
