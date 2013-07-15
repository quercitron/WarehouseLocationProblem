namespace Warehouse
{
    public class WarehouseInputData
    {
        public int N { get; set; }

        public int M { get; set; }

        public Warehouse[] Warehouses { get; set; }

        public Consumer[] Consumers { get; set; }

        public double[,] T { get; set; }
    }

    public struct Warehouse
    {
        public int Id { get; set; }

        public int Cap { get; set; }

        public double S { get; set; }
    }

    public struct Consumer
    {
        public int Id { get; set; }

        public int Demand { get; set; }
    }
}
