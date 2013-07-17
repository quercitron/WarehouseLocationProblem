using System.Collections.Generic;
using System.Linq;

namespace Warehouse
{
    public class GreedyHeuristic : IWarehouseSolver
    {
        public WarehouseSolution Solve(WarehouseInputData data)
        {
            var n = data.N;
            var m = data.M;

            var sortedWarehouses = data.Warehouses.OrderBy(wh => wh.S).ToArray();

            var solution = new WarehouseSolution { Cost = 1e30, Solution = new int[m]};
            var solutionFound = false;
            for (int mid = 1; mid <= n; mid++)
            {
                var links = new List<Link>(mid * m);
                for (int i = 0; i < mid; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        var link = new Link
                            {
                                WId = sortedWarehouses[i].Id,
                                CId = j,
                                Cost = data.T[j, sortedWarehouses[i].Id]
                            };
                        links.Add(link);
                    }
                }

                links = links.OrderBy(link => link.Cost).ToList();
                var cur = new int[n];
                var ans = new int[m];
                var usage = new bool[n];
                for (int i = 0; i < m; i++)
                {
                    ans[i] = -1;
                }

                double cost = 0;
                foreach (var link in links)
                {
                    if (ans[link.CId] < 0 && cur[link.WId] + data.Consumers[link.CId].Demand <= data.Warehouses[link.WId].Cap)
                    {
                        ans[link.CId] = link.WId;
                        cur[link.WId] += data.Consumers[link.CId].Demand;

                        cost += data.T[link.CId, link.WId];
                        if (!usage[link.WId])
                        {
                            usage[link.WId] = true;
                            cost += data.Warehouses[link.WId].S;
                        }
                    }
                }

                var isFeasible = ans.All(x => x >= 0);

                if (isFeasible)
                {
                    solutionFound = true;
                    if (cost < solution.Cost)
                    {
                        solution.Cost = cost;
                        solution.Solution = ans;
                    }
                }
            }

            return solution;
        }
    }

    public class Link
    {
        public int WId { get; set; }
        public int CId { get; set; }
        public double Cost { get; set; }
    }
}
