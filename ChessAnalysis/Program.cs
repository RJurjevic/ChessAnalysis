using System.Diagnostics.Meyer.Contracts;
using System.Text;

namespace ChessAnalysis
{
    public class Program
    {
        static void Main(string[] args)
        {
            ChessConsole.Instance.OutputBadgeToConsole();
            if (args.Length > 0)
            {
                try
                {
                    Contract.Assert(args[0] != null, "args[0] != null");
                    Contract.Assert(args[0].Length > 0, "args[0].Length > 0");
                    string file = args[0];
                    ChessConsole.Instance.Info(String.Format("file: {0}", file));
                    Contract.Assert(file != null, "file != null");
                    Contract.Assert(file.Length > 0, "file.Length > 0");
                    ChessGame game = new ChessGame();
                    Contract.Assert(game != null, "game != null");
                    ChessAnalysis analysis = new ChessAnalysis(game);
                    Contract.Assert(analysis != null, "analysis != null");
                    Config.Instance.OutputConfigs();
                    string annotatedPgn = analysis.Analyze(file, Config.Instance.GetHalfmoveStart(), Config.Instance.GetHalfmoveEnd(), Config.Instance.GetEngineMoveTime());
                    System.IO.File.WriteAllText(System.IO.Path.GetFileNameWithoutExtension(file) + "_annotated.pgn", annotatedPgn, Encoding.Unicode);
                    ChessConsole.Instance.Header(String.Format("{0}", "Analysis completed"));
                }
                catch (Exception e)
                {
                    ChessConsole.Instance.Error($"Exception: {e.Message}");
                    ChessConsole.Instance.Debug($"StackTrace:\n{e.StackTrace}");
                }
            }
        }
    }
}
