using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class Program
{
    public static void Main(string[] args)
    {
        //INFO: Documentation > https://benchmarkdotnet.org/articles/overview.html
        var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
    }
    
    [MemoryDiagnoser]
    public class StringTest
    {
        [Benchmark]
        public string StringConcat()
        {
            var text = string.Empty;

            for (int i = 0; i < 1000; i++)
            {
                text += i;
            }

            return text;
        }

        [Benchmark]
        public string StringBuilder()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < 1000; i++)
            {
                sb.Append(i);
            }

            return sb.ToString();
        }
    }
}