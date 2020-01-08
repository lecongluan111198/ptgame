using UnityEngine;
using System.Collections.Generic;

public class Node
{
    public float f;
    public float g;
    public float h;
    public Vector2 pos;
    public Node parent;
    public int inside = 0;
}

public class PathFinding
{
    public static Node[,] nodeData;

    public static void _InitNodeData()
    {
        PathFinding.nodeData = new Node[Number.MAP_SIZE, Number.MAP_SIZE];

        for (int i = 0; i < Number.MAP_SIZE; i++)
        {
            for (int j = 0; j < Number.MAP_SIZE; j++)
            {
                PathFinding.nodeData[i, j] = new Node()
                {
                    f = 0,
                    g = 0,
                    h = 0,
                    pos = new Vector2(i, j),
                    parent = null,
                    inside = 0
                };
            }
        }
    }

    public static List<Vector2> _PathFinding(Node start, Node end)
    {
        PathFinding._ClearNodeData();
        int idBuildingOfDestination = MapData.instance.data[(int)end.pos.x, (int)end.pos.y];

        List<Vector2> path = new List<Vector2>();

        // The set of nodes to be evaluated
        List<Node> close = new List<Node>();

        // The set of nodes already eveluated
        List<Node> open = new List<Node>();

        // Add the start node to open
        start.g = 0;
        start.h = PathFinding._Cost(start.pos, end.pos);
        start.f = start.g + start.h;
        open.Add(start);
        start.inside = 0;

        while (open.Count > 0)
        {
            Node current = PathFinding._MinF(open);

            open.Remove(current);
            current.inside = 0;

            close.Add(current);
            current.inside = 2;

            // Path has been found
            if (current.pos == end.pos)
            {
                //Debug.Log("FIND PATH SUCCESS");

                PathFinding._Show(path, current);

                break;
            }

            // For each in neighbour nodes
            for (int i = (int)current.pos.x - 1; i <= current.pos.x + 1; i++)
            {
                for (int j = (int)current.pos.y - 1; j <= current.pos.y + 1; j++)
                {
                    if (i < 0 || j < 0 || i >= Number.MAP_SIZE || j >= Number.MAP_SIZE)
                    {
                        continue;
                    }

                    if (MapData.instance.data[i, j] != -1 && MapData.instance.data[i, j] != idBuildingOfDestination)
                    {
                        continue;
                    }

                    // Setup node
                    PathFinding.nodeData[i, j].g = PathFinding._Cost(start.pos, PathFinding.nodeData[i, j].pos);
                    PathFinding.nodeData[i, j].h = PathFinding._Cost(PathFinding.nodeData[i, j].pos, end.pos);
                    PathFinding.nodeData[i, j].f = PathFinding.nodeData[i, j].g + PathFinding.nodeData[i, j].h;

                    // If it is in open set
                    if (PathFinding.nodeData[i, j].inside == 1)
                    {
                        if (PathFinding.nodeData[i, j].g > current.g + PathFinding._Cost(PathFinding.nodeData[i, j].pos, current.pos))
                        {
                            PathFinding.nodeData[i, j].g = current.g + PathFinding._Cost(PathFinding.nodeData[i, j].pos, current.pos);
                            PathFinding.nodeData[i, j].f = PathFinding.nodeData[i, j].g + PathFinding.nodeData[i, j].h;
                            PathFinding.nodeData[i, j].parent = current;
                        }
                    }
                    
                    // If it is not in anything
                    if (PathFinding.nodeData[i, j].inside == 0)
                    {
                        PathFinding.nodeData[i, j].g = current.g + PathFinding._Cost(PathFinding.nodeData[i, j].pos, current.pos);
                        PathFinding.nodeData[i, j].f = PathFinding.nodeData[i, j].g + PathFinding.nodeData[i, j].h;
                        PathFinding.nodeData[i, j].parent = current;
                        open.Add(PathFinding.nodeData[i, j]);
                        PathFinding.nodeData[i, j].inside = 1;
                    }

                    // If it is not in close set
                    if (PathFinding.nodeData[i, j].inside == 2)
                    {
                        if (PathFinding.nodeData[i, j].g > current.g + PathFinding._Cost(PathFinding.nodeData[i, j].pos, current.pos))
                        {
                            open.Remove(nodeData[i, j]);
                            nodeData[i, j].inside = 0;

                            close.Add(nodeData[i, j]);
                            nodeData[i, j].inside = 2;
                        }
                    }
                }
            }
        }

        // for (int k = 0; k < path.Count; k++)
        // {
        //     Debug.Log(path[k]);
        // }

        return path;
    }

    public static void _Show(List<Vector2> path, Node node)
    {
        if (node != null)
        {
            path.Add(node.pos);
            PathFinding._Show(path, node.parent);
        }
        else
        {
            // Debug.Log("IS NULL");
        }
    }

    public static Node _MinF(List<Node> open)
    {
        Node minF = open[0];
        
        for (int i = 1; i < open.Count; i++)
        {
            if (minF.f > open[i].f)
            {
                minF = open[i];
            }
        }

        return minF;
    }

    public static float _Cost(Vector2 a, Vector2 b)
    {
        return Mathf.Sqrt((b.x-a.x) * (b.x-a.x) + (b.y-a.y) * (b.y-a.y));
    }

    public static void _ClearNodeData()
    {
        for (int i = 0; i < Number.MAP_SIZE; i++)
        {
            for (int j = 0; j < Number.MAP_SIZE; j++)
            {
                PathFinding.nodeData[i, j].f = PathFinding.nodeData[i, j].g = PathFinding.nodeData[i, j].h = 0;
                PathFinding.nodeData[i, j].parent = null;
                PathFinding.nodeData[i, j].inside = 0;
            }
        }
    }
}