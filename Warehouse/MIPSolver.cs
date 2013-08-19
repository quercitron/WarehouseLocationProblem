using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimplexMethod;

namespace Warehouse
{
    public class MipSolver : IWarehouseSolver
    {
        private double[,] _baseA;

        private double[] _baseB;

        private double[] _baseC;

        private List<double[]> _addA = new List<double[]>();

        private List<double> _addB = new List<double>();

        private List<double> _addC = new List<double>();

        private readonly SimplexMethod.SimplexMethod _simplexSolver = new SimplexMethod.SimplexMethod();

        private SimplexMethod.SimplexMethodResult _bestResult = null;

        private readonly double Eps = 1e-9;

        public WarehouseSolution Solve(WarehouseInputData data)
        {
            var tv = data.N * data.M;
            var te = data.N + 2 * data.M;
            _baseA = new double[te, tv];
            _baseB = new double[te];
            for (int j = 0; j < data.M; j++)
            {
                var id = 2 * j;
                for (int i = 0; i < data.N; i++)
                {
                    _baseA[id, i * data.M + j] = 1;
                    _baseA[id + 1, i * data.M + j] = -1;
                }
                _baseB[id] = 1;
                _baseB[id + 1] = -1;
            }
            for (int i = 0; i < data.N; i++)
            {
                var id = 2 * data.M + i;
                for (int j = 0; j < data.M; j++)
                {
                    _baseA[id, i * data.M + j] = data.Consumers[j].Demand;
                }
                _baseB[id] = data.Warehouses[i].Cap;
            }
            _baseC = new double[tv];
            for (int i = 0; i < data.N; i++)
            {
                for (int j = 0; j < data.M; j++)
                {
                    _baseC[i * data.M + j] = -data.T[j, i];
                }
            }
            //var result = _simplexSolver.Simplex(_baseA, _baseB, _baseC);

            Run();

            var result = new int[data.M];
            for (int j = 0; j < data.M; j++)
            {
                for (int i = 0; i < data.N; i++)
                {
                    if (_bestResult.Solution[i * data.M + j] > 1 - Eps)
                    {
                        result[j] = i;
                    }
                }
            }

            return new WarehouseSolution
                {
                    Cost = -_bestResult.Value + data.Warehouses.Sum(w => w.S),
                    Solution = result,
                    SolutionFound = true
                };
        }

        private void Run()
        {
            var basem = _baseA.GetLength(0);
            var m = basem + _addA.Count();
            var n = _baseA.GetLength(1);

            var A = new double[m,n];
            var b = new double[m];
            for (int i = 0; i < basem; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = _baseA[i, j];
                }
                b[i] = _baseB[i];
            }
            for (int i = 0; i < _addA.Count; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[basem + i, j] = _addA[i][j];
                }
                b[basem + i] = _addB[i];
            }
            var c = new double[n];
            for (int i = 0; i < n; i++)
            {
                c[i] = _baseC[i];
            }

            var result = _simplexSolver.Simplex(A, b, c);

            if (result.Status == SimplexMethodResultStatus.Ok)
            {
                if (_bestResult != null && _bestResult.Value > result.Value)
                {
                    return;
                }

                var isOk = true;
                for (int i = 0; i < n; i++)
                {
                    var d = Math.Abs(result.Solution[i] - Math.Round(result.Solution[i]));
                    if (d > Eps)
                    {
                        isOk = false;
                        break;
                    }
                }

                if (isOk)
                {
                    if (_bestResult == null || _bestResult.Value < result.Value)
                    {
                        _bestResult = result;
                    }
                    return;
                }

                int id = -1;
                var bestd = 0D;
                for (int i = 0; i < n; i++)
                {
                    var d = Math.Abs(result.Solution[i] - Math.Round(result.Solution[i]));
                    if (id == -1 || bestd < d)
                    {
                        id = i;
                        bestd = d;
                    }
                }

                var arow = new double[n];
                arow[id] = 1;
                _addA.Add(arow);
                _addB.Add(0);

                Run();

                _addA.RemoveAt(_addA.Count - 1);
                _addB.RemoveAt(_addB.Count - 1);

                arow = new double[n];
                arow[id] = -1;
                _addA.Add(arow);
                _addB.Add(-1);

                Run();

                _addA.RemoveAt(_addA.Count - 1);
                _addB.RemoveAt(_addB.Count - 1);
            }
        }
    }
}
