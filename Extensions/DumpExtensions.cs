using System.Text;

namespace AdventOfCode.Extensions
{
    internal static class DumpExtensions
    {
        private const string Null = "null";

        internal static string Dump(this string @string, string? title = null) => DumpCore(@string, title);
        internal static string Dump<T>(this T number, string? title = null) where T : struct, IConvertible => DumpCore(number, title);

        internal static string Dump<T>(this T[] array, string? title = null)
        {
            DumpTitleIfNotNull(title);

            return array.InnerDump(print: true);
        }

        internal static string Dump<T>(this T[][] matrix, string? title = null, string? delimiter = null)
        {
            if (matrix is null) return ReturnNull(title);

            var matrixAsString = string.Format("[{0}]", string.Join(delimiter ?? Environment.NewLine, matrix.Select(r => r?.InnerDump(print: false) ?? Null)));

            DumpTitleIfNotNull(title);
            Console.WriteLine(matrixAsString);

            return matrixAsString;
        }

        internal static string Dump<T>(this T[,] matrix, string? title = null, char? delimiter = null)
        {
            if (matrix is null) return ReturnNull(title);

            var finalDelimiter = delimiter ?? '|';
            StringBuilder sb = new();
            sb.Append($"{finalDelimiter} ");

            var n = matrix.GetLength(0);
            var m = matrix.GetLength(1);
            for (var row = 0; row < n; row++)
            {
                for (var col = 0; col < m; col++)
                {
                    sb.Append($"{matrix[row, col]} {finalDelimiter} ");
                }

                if (row == n - 1) continue;

                sb.AppendLine();
                sb.Append($"{finalDelimiter} ");
            }

            var matrixAsString = sb.ToString();

            DumpTitleIfNotNull(title);
            Console.WriteLine(matrixAsString);

            return matrixAsString;
        }

        private static string DumpCore<T>(T arg, string? title = null)
        {
            var str = arg is string s
                ? s
                : (arg?.ToString() ?? Null);

            if (title is not null)
                Console.WriteLine($"{title}: {str}");
            else
                Console.WriteLine(str);

            return str;
        }

        private static string InnerDump<T>(this T[] array, bool print)
        {
            var arrayAsString = string.Format("[{0}]", string.Join(", ", array));

            if (print)
                Console.WriteLine(arrayAsString);

            return arrayAsString;
        }

        private static string ReturnNull(string? title = null)
        {
            if (title is not null)
                Console.WriteLine($"{title}: {Null}");
            else
                Console.WriteLine(Null);

            return Null;
        }

        private static void DumpTitleIfNotNull(string? title = null)
        {
            if (title is not null) Console.WriteLine($"{title}:");
        }
    }
}
