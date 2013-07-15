using System;
using System.Globalization;
using System.Threading;

using Warehouse;

namespace WarehouseLauncher
{
    public class Program
    {
        public static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var path = "wl_100_10";

            var data = WarehouseInputFactory.Create(path);

            var solver = new GreedyHeuristic();

            var result = solver.Solve(data);

            Console.WriteLine(result.Cost);
            Console.WriteLine(string.Join(" ", result.Solution));
            Console.ReadLine();
        }
    }
}
