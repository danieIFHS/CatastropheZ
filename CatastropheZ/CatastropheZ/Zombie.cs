using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatastropheZ
{
    public class Zombie
    {
        public Rectangle rect;
        public Texture2D text;
        public Vector2 position;
        public float rotation;
        List<Point> path;

        public Zombie()
        {
            rect = new Rectangle(8, 300, 25, 25);
            text = Globals.Textures["Placeholder"];
            position = new Vector2(rect.X, rect.Y);

            int tileSize = 40;
            AStarPathfinder pathfinder = new AStarPathfinder(Globals.ActiveLevel.PathfindingData);
            Point zombieGridPos = new Point((int)(position.X / tileSize), (int)(position.Y / tileSize));

            Point targetGridPos = Point.Zero;
            for (int x = 0; x < Globals.ActiveLevel.PathfindingData.GetLength(0); x++)
            {
                for (int y = 0; y < Globals.ActiveLevel.PathfindingData.GetLength(1); y++)
                {
                    if (Globals.ActiveLevel.PathfindingData[x, y].CollisionType == 2)
                    {
                        targetGridPos = new Point(x, y);
                        break;
                    }
                }
            }
            Console.WriteLine("Zombie Grid Pos: " + zombieGridPos);
            Console.WriteLine("Target Grid Pos: " + targetGridPos);
            bool startWalkable = Globals.ActiveLevel.PathfindingData[zombieGridPos.X, zombieGridPos.Y].CollisionType != 0;
            bool targetWalkable = Globals.ActiveLevel.PathfindingData[targetGridPos.X, targetGridPos.Y].CollisionType != 0;
            Console.WriteLine("Start Walkable: " + startWalkable);
            Console.WriteLine("Target Walkable: " + targetWalkable);

            path = pathfinder.FindPath(zombieGridPos, targetGridPos);
        }

        public void Update()
        {
            if (path != null && path.Count > 0)
            {
                Point nextCell = path[0];
                Vector2 nextPos = new Vector2(nextCell.X * 40, nextCell.Y * 40);

                float speed = 1.5f;
                Vector2 direction = nextPos - position;
                if (direction != Vector2.Zero)
                {
                    rotation = (float)Math.Atan2(direction.Y, direction.X);
                }
                if (direction.Length() < speed)
                {
                    position = nextPos;
                    path.RemoveAt(0);
                }
                else
                {
                    direction.Normalize();
                    position += direction * speed;
                }

                rect.X = (int)position.X;
                rect.Y = (int)position.Y;
            }
        }

        public void Draw()
        {
            Globals.Batch.Draw(
                text,
                new Rectangle(rect.X + 12, rect.Y + 12, rect.Width, rect.Height),
                new Rectangle(0, 0, text.Width, text.Height),
                Color.Black,
                rotation,
                new Vector2(text.Width / 2, text.Height / 2),
                SpriteEffects.None,
                1);

        }
    }


    public class Node
    {
        public int X;
        public int Y;
        public bool Walkable;
        public int gCost; // cost from start node
        public int hCost; // heuristic cost to target
        public int fCost { get { return gCost + hCost; } } // final cost 
        public Node parent;

        public Node(int x, int y, bool walkable)
        {
            X = x;
            Y = y;
            Walkable = walkable;
        }
    }

    public class AStarPathfinder
    {
        private int gridWidth; 
        private int gridHeight;
        private Node[,] nodes;

        public AStarPathfinder(Tile[,] tileGrid)
        {
            gridWidth = tileGrid.GetLength(0);
            gridHeight = tileGrid.GetLength(1);
            nodes = new Node[gridWidth, gridHeight];

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    bool walkable = tileGrid[x, y].CollisionType != 0;
                    nodes[x, y] = new Node(x, y, walkable);
                }
            }
        }

        public List<Point> FindPath(Point start, Point target)
        {
            Node startNode = nodes[start.X, start.Y];
            Node targetNode = nodes[target.X, target.Y];

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);
            startNode.gCost = 0;
            startNode.hCost = GetHeuristic(startNode, targetNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost ||
                       (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                foreach (Node neighbour in GetNeighbours(currentNode))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                        continue;

                    int tentativeGCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (tentativeGCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = GetHeuristic(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }

            return null;
        }

        private int GetHeuristic(Node a, Node b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private int GetDistance(Node a, Node b)
        {
            return 1;
        }

        private List<Point> RetracePath(Node startNode, Node endNode)
        {
            List<Point> path = new List<Point>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(new Point(currentNode.X, currentNode.Y));
                currentNode = currentNode.parent;
            }
            path.Reverse(); 
            return path;
        }
        private List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            if (node.Y - 1 >= 0)
                neighbours.Add(nodes[node.X, node.Y - 1]);

            if (node.Y + 1 < gridHeight)
                neighbours.Add(nodes[node.X, node.Y + 1]);

            if (node.X - 1 >= 0)
                neighbours.Add(nodes[node.X - 1, node.Y]);

            if (node.X + 1 < gridWidth)
                neighbours.Add(nodes[node.X + 1, node.Y]);

            return neighbours;
        }
    }
}
