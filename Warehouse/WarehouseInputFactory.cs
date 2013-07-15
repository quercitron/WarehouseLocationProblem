using System.IO;

namespace Warehouse
{
    public static class WarehouseInputFactory
    {
        public static WarehouseInputData Create(string path)
        {
            var data = new WarehouseInputData();
            using (var reader = File.OpenText(path))
            {
                var line = reader.ReadLine().Split();
                data.N = int.Parse(line[0]);
                data.M = int.Parse(line[1]);
                data.Warehouses = new Warehouse[data.N];
                for (int i = 0; i < data.N; i++)
                {
                    line = reader.ReadLine().Split();
                    data.Warehouses[i].Id = i;
                    data.Warehouses[i].Cap = int.Parse(line[0]);
                    data.Warehouses[i].S = double.Parse(line[0]);
                }
                data.T = new double[data.M,data.N];
                data.Consumers = new Consumer[data.M];
                for (int i = 0; i < data.M; i++)
                {
                    line = reader.ReadLine().Split();
                    data.Consumers[i].Id = i;
                    data.Consumers[i].Demand = int.Parse(line[0]);
                    line = reader.ReadLine().Split();
                    for (int j = 0; j < data.N; j++)
                    {
                        data.T[i, j] = double.Parse(line[j]);
                    }
                }
            }
            return data;
        }
    }
}
