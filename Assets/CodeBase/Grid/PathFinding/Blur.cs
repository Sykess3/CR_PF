using UnityEngine;

namespace CodeBase.Grid.PathFinding
{
    public static class Blur
    {
        public static int[,] Execute<TType>(TType[,] matrix, int blurSize) where TType : IBlurItem
        {
            int kernelSize = blurSize * 2 + 1;
            int kernelExtents = (kernelSize - 1) / 2;

            int[,] horizontalPass = HorizontalPass(matrix, kernelExtents);
            return VerticalPath(matrix, horizontalPass, kernelExtents, kernelSize);
        }

        private static int[,] HorizontalPass<TType>(TType[,] matrix, int kernelExtents) where TType : IBlurItem
        {
            int rowsCount = matrix.GetLength(0);
            int columnsCount = matrix.GetLength(1);
            int[,] horizontalPass = new int[columnsCount, rowsCount];
            for (int y = 0; y < rowsCount; y++)
            {
                for (int x = -kernelExtents; x <= kernelExtents; x++)
                {
                    int clampedX = Mathf.Clamp(x, 0, kernelExtents);
                    horizontalPass[0, y] += matrix[clampedX, y].BlurValue;
                }

                for (int x = 1; x < columnsCount; x++)
                {
                    int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, columnsCount);
                    int addIndex = Mathf.Clamp(x + kernelExtents, 0, columnsCount - 1);

                    horizontalPass[x, y] = 
                        horizontalPass[x - 1, y] 
                        - matrix[removeIndex, y].BlurValue 
                        + matrix[addIndex, y].BlurValue;
                }
            }

            return horizontalPass;
        }
        private static int[,] VerticalPath<TType>(TType[,] matrix,int[,] horizontalPass, int kernelExtents, int kernelSize) where TType : IBlurItem
        {
            int rowsCount = horizontalPass.GetLength(0);
            int columnsCount = horizontalPass.GetLength(1);
            
            int[,] resultArray = new int[columnsCount, rowsCount];
            int[,] verticalPass = new int[columnsCount, rowsCount];
            for (int x = 0; x < columnsCount; x++)
            {
                for (int y = -kernelExtents; y <= kernelExtents; y++)
                {
                    int clampedY = Mathf.Clamp(y, 0, kernelExtents);
                    verticalPass[x, 0] += horizontalPass[x, clampedY];
                }
                
                int blurredPenalty = Mathf.RoundToInt((float)verticalPass [x, 0] / (kernelSize * kernelSize));
                resultArray[x, 0] = blurredPenalty;
                
                for (int y = 1; y < rowsCount; y++)
                {
                    int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, rowsCount);
                    int addIndex = Mathf.Clamp(y + kernelExtents, 0, rowsCount - 1);

                    verticalPass[x, y] = 
                        verticalPass[x, y - 1] 
                        - horizontalPass[x, removeIndex] 
                        + horizontalPass[x, addIndex];
                    
                    resultArray[x, y] = Mathf.RoundToInt((float) verticalPass[x, y] / (kernelSize * kernelSize));
                }
            }

            return resultArray;
        }
    }
}