namespace AdventOfCode._2025.Solutions;

using System.Numerics;
using AdventOfCode._2025;
using AdventOfCode.Extensions;

internal partial class Puzzle08 : Puzzle2025<Puzzle08>
{
    internal override void Part1()
    {
        PriorityQueue<(int b1, int b2), float> pq = new(1000);
        List<Vector3> points = [];

        const int NumClosestToProcess = 1000;
        const int TopToMultiply = 3;

        foreach (var line in this.File)
        {
            var coords = line.Split(',').Select(float.Parse).ToArray();
            points.Add(new Vector3(coords[0], coords[1], coords[2]));
        }

        for (var i = 0; i < points.Count - 1; i++)
        {
            for (var j = i + 1; j < points.Count; j++)
            {
                pq.Enqueue((i, j), Vector3.Distance(points[i], points[j]));
            }
        }

        var uf = new UnionFind(points.Count);
        for (var i = 0; i < NumClosestToProcess; i++)
        {
            var conn = pq.Dequeue();

            Console.WriteLine($"Attaching box {points[conn.b1]} and {points[conn.b2]}");
            uf.Union(conn.b1, conn.b2);
        }


        Dictionary<int, int> circuits = [];
        for (var i = 0; i < uf.Connections.Length; i++)
        {
            var parent = uf.FindParent(i);
            if (circuits.ContainsKey(parent))
            {
                circuits[parent]++;
            }
            else
            {
                circuits[parent] = 1;
            }
        }

        var ans = 1L;
        foreach (var val in circuits.Values.OrderByDescending(x => x).Take(TopToMultiply))
        {
            ans *= val;
        }

        ans.Dump("ans");
    }

    internal override void Part2()
    {
        PriorityQueue<(int b1, int b2), float> pq = new(1000);
        List<Vector3> points = [];

        foreach (var line in this.File)
        {
            var xyz = line.Split(',').Select(float.Parse).ToArray();
            points.Add(new Vector3(xyz[0], xyz[1], xyz[2]));
        }

        for (var i = 0; i < points.Count - 1; i++)
        {
            for (var j = i + 1; j < points.Count; j++)
            {
                pq.Enqueue((i, j), Vector3.Distance(points[i], points[j]));
            }
        }

        var uf = new UnionFind(points.Count);
        (Vector3 l1, Vector3 l2) lastPair = (points[0], points[1]);
        while (!uf.AllConnected())
        {
            var (b1, b2) = pq.Dequeue();

            Console.WriteLine($"Attaching box {points[b1]} and {points[b2]}");
            uf.Union(b1, b2);
            lastPair = (points[b1], points[b2]);
        }


        ((long)lastPair.l1.X * (long)lastPair.l2.X).Dump("ans");
    }

    class UnionFind
    {
        public int[] Connections { get; private set; }

        public UnionFind(int size)
        {
            this.Connections = new int[size];
            for (var i = 0; i < size; i++)
            {
                this.Connections[i] = i;
            }
        }

        public void Union(int x, int y)
        {
            var xp = this.FindParent(x);
            var yp = this.FindParent(y);

            if (xp > yp)
            {
                this.Connections[xp] = yp;
            }
            else
            {
                this.Connections[yp] = xp;
            }
        }

        public int FindParent(int x)
        {
            while (this.Connections[x] != x)
            {
                x = this.Connections[x];
            }

            return x;
        }

        public bool AllConnected()
        {
            return this.Connections.All(x => this.FindParent(x) == 0);
        }

    }
}
