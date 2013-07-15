namespace Warehouse
{
    public interface IWarehouseSolver
    {
        WarehouseSolution Solve(WarehouseInputData data);
    }
}
