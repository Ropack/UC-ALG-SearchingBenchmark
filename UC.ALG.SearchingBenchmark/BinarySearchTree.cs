using System;
using System.IO;

namespace UC.ALG.SearchingBenchmark
{
    public class BinarySearchTree
    {
        public Vertex Root { get; private set; }

        // Creates empty tree.
        public BinarySearchTree()
        {
            Root = new Vertex(new int?[] { null, null });
        }

        // Searches for +x+. Returns +Vertex+ containing +x+ or +null+ if +x+ is not found.
        public Vertex Find(int? x)
        {
            var node = SearchFor(x);
            return (node.Value != null) ? node : null;
        }

        // Inserts +x+ into +self+. If +x+ is already there, this method does nothing.
        // Returns +Vertex+ containing +x+.
        public Vertex Insert(int? x)
        {
            var node = SearchFor(x);
            if (node.Value == null)
            {
                var left = new Vertex(new[] {node.Interval[0], x
            });
                var right = new Vertex(new[]{x, node.Interval[1]});
                node.Left = left;
                node.Right = right;
                left.Parent = node;
                right.Parent = node;
                node.Interval = null;
                node.Value = x;
            }

            return node;
        }

        // Deletes +x+ from +self+. If +self+ doesn't contain +x+, this method does nothing.
        // Returns +true+ if +x+ was deleted, returns false if +x+ was not found.
        public bool Delete(int? x)
        {
            var node = SearchFor(x);
            if (node.Value != null)
            {
                // we found x
                if (node.Left.Value == null)
                {
                    // node.Left is leaf
                    var leftEdge = node.Left.Interval[0];
                    node = DeleteSpecial(node, node.Left);

                    // update the leftest interval in the subtree of the right-side child of the original node
                    while (node.Value != null)
                    {
                        node = node.Left;
                    }
                    node.Interval[0] = leftEdge;
                }
                else
                {
                    // node.left isn't leaf
                    var rightest = node.Left;
                    while (rightest.Right.Value != null)
                    {
                        rightest = rightest.Right;
                    }
                    node.Value = rightest.Value;
                    DeleteSpecial(rightest, rightest.Right);

                    // update the leftest interval in the subtree of the right-side child of the original node
                    var leftest = node.Right;
                    while (leftest.Value != null)
                    {
                        leftest = leftest.Left;
                    }
                    leftest.Interval[0] = node.Value;
                }

                return true;
            }

            return false;
        }

        // Returns linearized string representation of +self+.
        public override string ToString()
        {
            return Root.ToSubtreeS();
        }



        // Only for internal use, shouldn't be used outside this class.
        // Searches for value +x+ and returns either inner vertex holding +x+
        // or leaf holding interval containing +x+.
        private Vertex SearchFor(int? x)
        {
            var node = Root;
            while (node.Value != x && node.Value != null)
            {
                node = (x < node.Value) ? node.Left : node.Right;
            }

            return node;
        }

        // Only for internal use, shouldn't be used outside this class.
        // Deletes +x+, using its child +y+. Requirements: +y+ is child of +x+,
        // +y+ is leaf (otherwise this method's behavior is undefined).
        // Returns +Vertex+ which takes +x+'s places in the tree.
        private Vertex DeleteSpecial(Vertex x, Vertex y)
        {
            // precondition: y is child of x, y is leaf
            var z = (x.Left == y) ? x.Right : x.Left; // let z be the other child of x
            if (x != Root)
            {
                // z isn't root
                z.Parent = x.Parent;
                if (x == x.Parent.Right)
                {
                    x.Parent.Right = z;
                }
                else
                {
                    x.Parent.Left = z;
                }
            }
            else
            {
                // z is root
                z.Parent = null;
                Root = z;
            }

            return z;
        }


        // Constants used in to_svg method.

        // Left offset of the tree in the whole SVG picture.
        public const int START_LEFT = 0;

        // Height of one level of the tree.
        public const int LEVEL_HEIGHT = 60;

        // Width of a node representing an interval.
        public const int INTERVAL_WIDTH = 68;

        // Width of a node representing a value.
        public const int VALUE_WIDTH = 40;

        // Height of a node
        public const int NODE_HEIGHT = 24;

        // "Magical" vertical offset that makes text inside nodes "better vertically centered".
        public const int MAGICAL_OFFSET = 7;

        // Horizontal space between two nodes that are horizontally "next to each other".
        public const int HORIZONTAL_SPACE = 12;

        // Saves SVG representation of +self+ into +filename+.
        // The picture is then best viewed in Inkscape.
        public void ToSvg(string filename)
        {
            using (var f = new StreamWriter(filename))
            {
                f.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                f.WriteLine("<svg xmlns=\"http://www.w3.org/2000/svg\">");
                f.WriteLine(Root.ToSvg(START_LEFT, 0).Svg);
                f.WriteLine("</svg>");
            }
        }
    }


    // Represents a single vertex of a binary search tree.
    public class Vertex
    {
        public Vertex Left { get; set; }

        public Vertex Right { get; set; }

        public Vertex Parent { get; set; }

        public int? Value { get; set; }

        public int?[] Interval { get; set; }
        // Creates a vertex. +value+ is either an object being inserted into
        // a binary search tree or a two-item array representing an interval.
        // This method should be used only from within the +BinarySearchTree+ class.
        public Vertex(object value)
        {
            Left = null;
            Right = null;
            Parent = null;
            if (value is int?[] x)
            {
                Interval = x;
                Value = null;
            }
            else
            {
                Value = (int?)value;
                Interval = null;
            }
        }

        // Returns string representation of self.
        public override string ToString()
        {
            return Value != null ? $"{Value}" : $"({Interval[0]},{Interval[1]})";
        }

        // Returns linearized string representation of self and all its children.
        public string ToSubtreeS()
        {
            var s = "";
            if (Value != null)
            {
                s += Value.ToString();
                if (Left.Value != null || Right.Value != null)
                {
                    s += "(" + Left.ToSubtreeS() + "," + Right.ToSubtreeS() + ")";
                }
            }

            return s;
        }

        // Builds (in the form of string) SVG representation of +self+ and all its children.
        //  left: left offset of the leftest node in the whole subtree
        //  level: level in the tree (root has a level of 0)
        // Returns three-item array, the first item being SVG string itself. The other two
        // items are needed for recursion. The second item is a horizontal offset of the center of +self+
        // (used for drawing of edges). The third item is a horizontal offset representing the rightest
        // point used when drawing +self+'s whole subtree.
        public SvgObject ToSvg(double left, int level)
        {
            // y offset of self
            var y = (level + 0) * BinarySearchTree.LEVEL_HEIGHT + BinarySearchTree.MAGICAL_OFFSET +
                    BinarySearchTree.NODE_HEIGHT / 2.0;
            var svg = "";
            if (Value == null)
            {
                // self represents an interval
                // string representation of the left bound
                var leftEdge = Interval[0] == null ? "−∞" : (Interval[0] < 0 ? $"−{-Interval[0]}" : Interval[0].ToString());
                // string representation of the right bound
                var rightEdge = Interval[1] == null ? "∞" : (Interval[1] < 0 ? $"−{-Interval[1]}" : Interval[1].ToString());
                // textual representation of the interval
                svg +=
                    $"  <text x=\"{left + BinarySearchTree.INTERVAL_WIDTH / 2.0}\" y=\"{y}\" style=\"text-anchor: middle; font-size: 20;\">{leftEdge},{rightEdge}</text>\n";
                // rectangle around the interval
                svg +=
                    $"  <rect x=\"{left}\" y=\"{y - BinarySearchTree.MAGICAL_OFFSET - BinarySearchTree.NODE_HEIGHT / 2.0}\" width=\"{BinarySearchTree.INTERVAL_WIDTH}\" height=\"{BinarySearchTree.NODE_HEIGHT}\" style=\"fill: none; stroke: black; stroke-width: 2;\"/>\n";
                return new SvgObject()
                {
                    Svg = svg,
                    Center = left + BinarySearchTree.INTERVAL_WIDTH / 2.0,
                    Right = left + BinarySearchTree.INTERVAL_WIDTH
                };
            }
            else
            {
                // self represents a value
                // recursion for the left child
                //string left_svg, center, right;
                var leftSvgObject = Left.ToSvg(left, level + 1);
                svg += leftSvgObject.Svg;
                // horizontal offset around which self's text and rectangle will be centered
                var textCenter = leftSvgObject.Right + BinarySearchTree.HORIZONTAL_SPACE / 2.0;
                // edge to the left child
                svg +=
                    $"  <line x1=\"{textCenter}\" y1=\"{y - BinarySearchTree.MAGICAL_OFFSET + BinarySearchTree.NODE_HEIGHT / 2.0}\" x2=\"{leftSvgObject.Center}\" y2=\"{y - BinarySearchTree.MAGICAL_OFFSET - BinarySearchTree.NODE_HEIGHT / 2.0 + BinarySearchTree.LEVEL_HEIGHT}\" style=\"stroke-width: 2; stroke: black;\"/>\n";
                // string representation of the value
                var val = Value < 0 ? $"−{-Value}" : Value.ToString();
                // textual representation of the value
                svg +=
                    $"  <text x=\"{textCenter}\" y=\"{y}\" style=\"text-anchor: middle; font-size: 20;\">{val}</text>\"\n";
                // rounded rectangle around the value
                svg +=
                    $"  <rect x=\"{textCenter - BinarySearchTree.VALUE_WIDTH / 2.0}\" y=\"{y - BinarySearchTree.MAGICAL_OFFSET - BinarySearchTree.NODE_HEIGHT / 2.0}\" width=\"{BinarySearchTree.VALUE_WIDTH}\" height=\"{BinarySearchTree.NODE_HEIGHT}\" rx=\"20\" ry=\"20\" style=\"fill: none; stroke: black; stroke-width: 2;\"/>\n";
                // recursion for the right child
                var rightSvgObject = Right.ToSvg(leftSvgObject.Right + BinarySearchTree.HORIZONTAL_SPACE, level + 1);
                //edghe to the right child
                svg +=
                    $"  <line x1=\"{textCenter}\" y1=\"{y - BinarySearchTree.MAGICAL_OFFSET + BinarySearchTree.NODE_HEIGHT / 2.0}\" x2=\"{rightSvgObject.Center}\" y2=\"{y - BinarySearchTree.MAGICAL_OFFSET - BinarySearchTree.NODE_HEIGHT / 2.0 + BinarySearchTree.LEVEL_HEIGHT}\" style=\"stroke-width: 2; stroke: black;\"/>\n";
                svg += rightSvgObject.Svg;
                return new SvgObject() { Svg = svg, Center = textCenter, Right = rightSvgObject.Right };
            }

        }
    }

    public class SvgObject
    {
        public string Svg { get; set; }
        public double Center { get; set; }
        public double Right { get; set; }
    }
}