using System;
using System.Globalization;
using System.Threading;

using Warehouse;

namespace WarehouseLauncher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var path = args[0];

            var data = WarehouseInputFactory.Create(path);

            var solver = new GreedyHeuristic();

            var result = solver.Solve(data);

            Console.WriteLine("{0} 0", result.Cost);
            Console.WriteLine(string.Join(" ", result.Solution));
        }
    }
}
