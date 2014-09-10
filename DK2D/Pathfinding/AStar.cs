using System;
using System.Collections.Generic;
using System.Linq;

using SFML.Window;

namespace DK2D.Pathfinding
{
    internal class AStar
    {
        private readonly Func<Vector2i, bool> _isPassable;
        private readonly Func<Vector2i, IEnumerable<Vector2i>> _getSuccessors;

        public AStar(Func<Vector2i, bool> isPassable, Func<Vector2i, IEnumerable<Vector2i>> getSuccessors)
        {
            _isPassable = isPassable;
            _getSuccessors = getSuccessors;
        }

        public List<Vector2i> FindPath(Vector2i start, Vector2i end)
        {
            var open = new List<Node>();
            open.Add(new Node { Position = start });

            var closed = new List<Node>();

            do
            {
                Node currentNode = open.OrderBy(node => node.F).First();
                open.Remove(currentNode);

                if (currentNode.Position.X == end.X && currentNode.Position.Y == end.Y)
                {
                    // Path found
                    var path = new List<Vector2i>();
                    Node next = currentNode;

                    do
                    {
                        path.Add(next.Position);
                        next = next.Predecessor;
                    }
                    while (next != null);

                    path.Reverse();
                    return path;
                }

                closed.Add(currentNode);

                ExpandNode(currentNode, end, open, closed);
            }
            while (open.Count > 0);

            return new List<Vector2i>();
        }

        private void ExpandNode(Node currentNode, Vector2i end, List<Node> open, List<Node> closed)
        {
            foreach (Node successor in Successors(currentNode))
            {
                bool successorIsClosed = closed.Any(closedNode => closedNode.Position.IsSameAs(successor.Position));
                if (successorIsClosed)
                {
                    continue;
                }

                if (!_isPassable(successor.Position))
                {
                    continue;
                }

                var tentativeG = currentNode.G + 1;

                bool successorIsOpen = open.Any(openNode => openNode.Position.IsSameAs(successor.Position));
                if (successorIsOpen && tentativeG >= successor.G)
                {
                    continue;
                }

                successor.Predecessor = currentNode;
                successor.G = tentativeG;
                successor.H = successor.Position.DistanceTo(end);

                if (!successorIsOpen)
                {
                    open.Add(successor);
                }
            }
        }

        private IEnumerable<Node> Successors(Node node)
        {
            foreach (Vector2i successor in _getSuccessors(node.Position))
            {
                yield return new Node { Position = successor };
            }
        }

        private class Node
        {
            public Vector2i Position { get; set; }

            /// <summary>
            /// Gibt an, wie lang der Pfad vom Start zum Ziel unter Verwendung des betrachteten Knotens im günstigsten Fall ist.
            /// </summary>
            public float F
            {
                get
                {
                    return G + H;
                }
            }

            /// <summary>
            /// Die bisherigen Kosten vom Startknoten aus, um x zu erreichen.
            /// </summary>
            public float G { get; set; }

            /// <summary>
            /// geschätzten Kosten von x bis zum Zielknoten.
            /// </summary>
            public float H { get; set; }

            public Node Predecessor { get; set; }
        }
    }
}
