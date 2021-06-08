using System;
using System.Collections.Generic;
using System.IO;

namespace dejkstra_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            (int, int) sizeGraph = (0, 0);
            List<(int, int, double)> edgesList = Read(ref sizeGraph);
            double[,] matrix = AdjMatrix(sizeGraph, edgesList);
            Console.WriteLine("Введите 1. Растояние до всехвершин 2. Растояние до одной");
            int check = Convert.ToInt32(Console.ReadLine());
            DijkstraAlgorithm(matrix, sizeGraph, check);
        }
        static List<(int, int, double)> Read(ref (int, int) sizeGraph)
        { 
            List<(int, int, double)> list = new List<(int, int, double)>();
            StreamReader read = new StreamReader("test.txt");
            string[] size = read.ReadLine()?.Split(' ');
            if (size != null)
            {
                sizeGraph = (Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));
                for (int i = 0; i < sizeGraph.Item2; ++i)
                {
                    size = read.ReadLine()?.Split(' ');
                    if (size != null)
                        list.Add((Convert.ToInt32(size[0]), Convert.ToInt32(size[1]), Convert.ToDouble(size[2])));
                }
            }
            return list;
        }
        static double[,] AdjMatrix((int, int) sizeMatrix, List<(int, int, double)> edgeList)
        {
            double[,] matrixA = new Double[sizeMatrix.Item1, sizeMatrix.Item1];
            for (int i = 0; i < sizeMatrix.Item1; i++)
            for (int j = 0; j < sizeMatrix.Item1; j++)
                    if (i == j)
                        matrixA[i, j] = 0;
                    else
                        matrixA[i, j] = double.PositiveInfinity;
            foreach (var item in edgeList)
                matrixA[item.Item1 - 1, item.Item2 - 1] = item.Item3;
            return matrixA;
        }
        static void DijkstraAlgorithm(double[,] matrix, (int, int) sizeG, int check)
        {
            List<(int, double, bool)> list = new List<(int, double, bool)>();
            List<int> visitedEdge = new List<int>();
            List<int> preIndex = new List<int>();
            double sumValue = 0;
            Console.WriteLine("Стартовая вершина");
            int start = Convert.ToInt32(Console.ReadLine());
            FirstValue(ref list, sizeG, start, ref visitedEdge);
            double lengthMatrix = Math.Pow(matrix.Length, 0.5);
            for (int i = 0; i < lengthMatrix; i++)
                preIndex.Add(0);
            if (check == 1)
            {
                while (visitedEdge.Count != lengthMatrix)
                    CheckForDijkstra(lengthMatrix, ref list, matrix, ref sumValue, ref visitedEdge, ref preIndex);
                Console.WriteLine($"Из вершини {start}");
                foreach (var item in list)
                    Console.WriteLine($"В вершину {item.Item1} -- {item.Item2}");
            }
            else if (check == 2)
            {
                Console.WriteLine("Финишная вершина");
                int finish = Convert.ToInt32(Console.ReadLine());
                while (SearchInList(visitedEdge, finish))
                {
                    CheckForDijkstra(lengthMatrix, ref list, matrix, ref sumValue, ref visitedEdge, ref preIndex);
                }
                SearchWay(finish, start, preIndex);
                Console.WriteLine($"Из вершини {start} в {finish} == {list[finish - 1].Item2}");
            }
        }
        static void CheckForDijkstra(double lengthMatrix, ref List<(int, double, bool)> list, double[,] matrix, ref double sumValue, ref List<int> visitedEdge, ref List<int> preIndex)
        {
            for (int j = 0; j < lengthMatrix; j++)
            {
                if (matrix[visitedEdge[visitedEdge.Count - 1] - 1,j] < 0)
                    throw new System.ArgumentException("Ошыбка!!! Значение отрицательное");
                if (matrix[visitedEdge[visitedEdge.Count - 1] - 1,j] + sumValue < list[j].Item2)
                    preIndex[j] = visitedEdge[visitedEdge.Count - 1] - 1;
                list[j] = (list[j].Item1, MinDuo(matrix[visitedEdge[visitedEdge.Count - 1] - 1,j] + sumValue, list[j].Item2), list[j].Item3);
            }
            visitedEdge.Add(Minimum(ref list));
            sumValue = list[visitedEdge[visitedEdge.Count - 1] - 1].Item2;
        }
        static void SearchWay(int finish, int start, List<int> list)
        {
            List<int> array = new List<int>();
            while (finish != start)
            {
                array.Add(finish);
                finish = list[finish - 1] + 1;
            }
            array.Add(start);
            Console.WriteLine("Путь");
            for (int i = array.Count - 1; i >= 0; --i)
            {
                Console.WriteLine($"{array[i],3}");
            }
        }
        static void FirstValue(ref List<(int, double, bool)> list, (int, int) sizeG, int start, ref List<int> visited)
        {
            visited.Add(start);
            for (int i = 0; i < sizeG.Item1; i++)
                if (i + 1 == start) 
                    list.Add((i + 1, 0, true));
                else
                    list.Add((i + 1, Double.PositiveInfinity, false));
        }
        static bool SearchInList(List<int> list, int n)
                 {
                     foreach (var item in list)
                         if (item == n)
                             return false;
                     return true;
                 }
        static int Minimum(ref List<(int, double, bool)> array)
        {
            double minimum = double.PositiveInfinity;
            int count = 0;
            for (int i = 0; i < array.Count; ++i)
                if (minimum > array[i].Item2 && !array[i].Item3)
                {
                    minimum = array[i].Item2;
                    count = i;
                }
            array[count] = (array[count].Item1, array[count].Item2, true);
            return count + 1;
        }
        static double MinDuo(double a, double b)
        {
            if (a < b)
                return a;
            return b;
        }
    }
}