using System;
using System.Linq;

public class ErrorRateCalculator
{
    public static double CalculateCER(string reference, string hypothesis)
    {
        if (string.IsNullOrEmpty(reference) || string.IsNullOrEmpty(hypothesis))
        {
            throw new ArgumentException("Input strings cannot be null or empty.");
        }

        int editDistance = ComputeEditDistance(reference, hypothesis);
        int referenceLength = reference.Length;

        return (double)editDistance / referenceLength;
    }

    public static double CalculateWER(string[] referenceWords, string[] hypothesisWords)
    {
        if (referenceWords == null || hypothesisWords == null || referenceWords.Length == 0 || hypothesisWords.Length == 0)
        {
            throw new ArgumentException("Input arrays cannot be null or empty.");
        }

        int[][] distanceMatrix = ComputeWordEditDistanceMatrix(referenceWords, hypothesisWords);
        int editDistance = distanceMatrix[referenceWords.Length][hypothesisWords.Length];

        return (double)editDistance / referenceWords.Length;
    }

    private static int ComputeEditDistance(string str1, string str2)
    {
        int[,] dp = new int[str1.Length + 1, str2.Length + 1];

        for (int i = 0; i <= str1.Length; i++)
        {
            for (int j = 0; j <= str2.Length; j++)
            {
                if (i == 0)
                {
                    dp[i, j] = j;
                }
                else if (j == 0)
                {
                    dp[i, j] = i;
                }
                else if (str1[i - 1] == str2[j - 1])
                {
                    dp[i, j] = dp[i - 1, j - 1];
                }
                else
                {
                    dp[i, j] = 1 + Math.Min(dp[i, j - 1], Math.Min(dp[i - 1, j], dp[i - 1, j - 1]));
                }
            }
        }

        return dp[str1.Length, str2.Length];
    }

    private static int[][] ComputeWordEditDistanceMatrix(string[] referenceWords, string[] hypothesisWords)
    {
        int[][] dp = new int[referenceWords.Length + 1][];

        for (int i = 0; i <= referenceWords.Length; i++)
        {
            dp[i] = new int[hypothesisWords.Length + 1];
            dp[i][0] = i;
        }

        for (int j = 0; j <= hypothesisWords.Length; j++)
        {
            dp[0][j] = j;
        }

        for (int i = 1; i <= referenceWords.Length; i++)
        {
            for (int j = 1; j <= hypothesisWords.Length; j++)
            {
                if (referenceWords[i - 1] == hypothesisWords[j - 1])
                {
                    dp[i][j] = dp[i - 1][j - 1];
                }
                else
                {
                    dp[i][j] = 1 + Math.Min(dp[i][j - 1], Math.Min(dp[i - 1][j], dp[i - 1][j - 1]));
                }
            }
        }

        return dp;
    }
}

class Program
{
    static void Main()
    {
        // Example usage
        string reference = "hello";
        string hypothesis = "hola";

        double cer = ErrorRateCalculator.CalculateCER(reference, hypothesis);
        Console.WriteLine($"CER: {cer:P}");

        string[] referenceWords = "this is a test".Split(' ');
        string[] hypothesisWords = "this a test".Split(' ');

        double wer = ErrorRateCalculator.CalculateWER(referenceWords, hypothesisWords);
        Console.WriteLine($"WER: {wer:P}");
    }
}
