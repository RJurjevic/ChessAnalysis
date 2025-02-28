using System.Diagnostics;
using System.Diagnostics.Meyer.Contracts;
using System.Text;
using System.Text.RegularExpressions;

namespace ChessAnalysis
{
    public class ChessEngine
    {
        public class ChessAnalysis
        {
            private string _output = "";
            private string _analysis = "";

            private string ParseBestmove(string analysis)
            {
                Contract.Require(analysis != null, "analysis != null");
                string bestmove = "";
                string pattern = @"bestmove\s+(\w+)";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(analysis);
                if (match.Success)
                {
                    bestmove = match.Groups[1].Value;
                }
                return bestmove;
            }

            private int ParseScore(string analysis)
            {
                Contract.Require(analysis != null, "analysis != null");
                int score = 0;
                string pattern = @"score\s+cp\s+(-?\d+)";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(analysis);
                if (match.Success)
                {
                    int s = 0;
                    int.TryParse(match.Groups[1].Value, out s);
                    score = s;
                }
                else
                {
                    pattern = @"score\s+mate\s+(-?\d+)";
                    regex = new Regex(pattern);
                    match = regex.Match(analysis);
                    if (match.Success)
                    {
                        int m = 0;
                        int.TryParse(match.Groups[1].Value, out m);
                        int s = (m > 0) ? 32000 : -32000;
                        score = s;
                    }
                }
                return score;
            }

            private int ParseDepth(string analysis)
            {
                Contract.Require(analysis != null, "analysis != null");
                int depth = 0;
                string pattern = @"depth\s+(-?\d+)";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(analysis);
                if (match.Success)
                {
                    int d = 0;
                    int.TryParse(match.Groups[1].Value, out d);
                    depth = d;
                }
                return depth;
            }

            public ChessAnalysis(string output, string analysis)
            {
                Contract.Require(output != null, "output != null");
                Contract.Require(analysis != null, "analysis != null");
                _output = output;
                _analysis = analysis;
                Contract.Ensure(_output != null, "_output != null");
                Contract.Ensure(_analysis != null, "_analysis != null");
            }

            public string GetOutput()
            {
                return _output;
            }

            public string GetAnalysis()
            {
                return _analysis;
            }

            public string GetBestmove()
            {
                Contract.Require(_analysis != null, "_analysis != null");
                return ParseBestmove(_analysis);
            }

            public int GetScore()
            {
                Contract.Require(_analysis != null, "_analysis != null");
                return ParseScore(_analysis);
            }

            public int GetDepth()
            {
                Contract.Require(_analysis != null, "_analysis != null");
                return ParseDepth(_analysis);
            }
        }

        private ChessAnalysis ParseOutput(string output)
        {
            Contract.Require(output != null, "output != null");
            char[] separators = new[] { '\r', '\n' };
            string[] lines = output.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string info = "";
            string bestmove = "";
            foreach (string line in lines)
            {
                if (line.StartsWith("info "))
                {
                    info = line;
                }
                if (line.StartsWith("bestmove "))
                {
                    bestmove = line;
                }
            }
            Contract.Ensure(output != null, "output != null");
            return new ChessAnalysis(output, String.Format("{0}{1}{2}", info, "; ", bestmove));
        }

        public ChessEngine()
        {
        }

        public ChessAnalysis AnalyzeWithStockfish(int engineMoveTime, string fen, string ucimoves)
        {
            Contract.Require(engineMoveTime != null, "engineMoveTime != null");
            Contract.Require(fen != null, "fenText != null");
            Contract.Require(ucimoves != null, "ucimoves != null");
            string output = "";
            StringBuilder outputBuilder;
            ProcessStartInfo processStartInfo;
            Process process;
            outputBuilder = new StringBuilder();
            processStartInfo = new ProcessStartInfo();
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "";
            processStartInfo.FileName = Config.Instance.GetEngine();
            process = new Process();
            process.StartInfo = processStartInfo;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += new DataReceivedEventHandler
            (
                delegate (object sender, DataReceivedEventArgs e)
                {
                    outputBuilder.Append(String.Format(@"{0}{1}", e.Data, Environment.NewLine));
                }
            );
            process.Start();
            process.BeginOutputReadLine();
            process.StandardInput.WriteLine(String.Format("setoption name Hash value {0}", Config.Instance.GetEngineHash()));
            process.StandardInput.Flush();
            process.StandardInput.WriteLine(String.Format("setoption name Threads value {0}", Config.Instance.GetEngineThreads()));
            process.StandardInput.Flush();
            process.StandardInput.WriteLine(String.Format("setoption name SyzygyPath value {0}", Config.Instance.GetEngineSyzygyPath()));
            process.StandardInput.Flush();
            process.StandardInput.WriteLine("uci");
            process.StandardInput.Flush();
            process.StandardInput.WriteLine("isready");
            process.StandardInput.Flush();
            Thread.Sleep(2000);
            if (fen.Length > 0)
            {
                if (ucimoves.Length > 0)
                {
                    process.StandardInput.WriteLine(String.Format(@"position fen {0} moves {1}", fen, ucimoves));
                }
                else
                {
                    process.StandardInput.WriteLine(String.Format(@"position fen {0}", fen));
                }
            }
            else
            {
                process.StandardInput.WriteLine("position startpos");
            }
            process.StandardInput.Flush();
            process.StandardInput.WriteLine(String.Format(@"go movetime {0}", engineMoveTime.ToString()));
            process.StandardInput.Flush();
            Thread.Sleep(engineMoveTime + 5000);
            process.StandardInput.WriteLine("quit");
            Thread.Sleep(1000);
            process.StandardInput.Flush();
            process.WaitForExit();
            process.CancelOutputRead();
            output = outputBuilder.ToString();
            return ParseOutput(output);
        }
    }
}
