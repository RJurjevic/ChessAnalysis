using System.Diagnostics.Meyer.Contracts;
using System.Text;

namespace ChessAnalysis
{
    public class Config
    {
        private string _engine = "vafra_v14.12.2_x86-64_avx2_windows.exe";
        private int _engineHash = 1024;
        private int _engineThreads = 2;
        private string _engineSyzygyPath = @"C:\Users\Hostmaster\Downloads\RJ\Syzygy";
        private int _moveMarginDubious = -60;
        private int _moveMarginBad = -100;
        private int _moveMarginBlunder = -200;
        private int _moveMarginGood = 10;
        private int _moveMarginExcellent = 30;
        private int _scoreEqual = 30;
        private int _scoreEdge = 100;
        private int _scoreBetter = 200;
        private int _halfmoveStart = 9;
        private int _halfmoveEnd = 999;
        private int _engineMoveTime = 60;

        private void ReadConfig()
        {
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead("config.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                int maxLines = 15;
                String[] lines = new String[maxLines];
                int i = 0;
                String line;
                while ((line = streamReader.ReadLine()) != null && i < maxLines)
                {
                    lines[i] = line;
                    i++;
                }
                _engine = lines[0];
                int val;
                if (int.TryParse(lines[1], out val))
                {
                    _engineHash = val;
                }
                if (int.TryParse(lines[2], out val))
                {
                    _engineThreads = val;
                }
                _engineSyzygyPath = lines[3];
                if (int.TryParse(lines[4], out val))
                {
                    _moveMarginDubious = val;
                }
                if (int.TryParse(lines[5], out val))
                {
                    _moveMarginBad = val;
                }
                if (int.TryParse(lines[6], out val))
                {
                    _moveMarginBlunder = val;
                }
                if (int.TryParse(lines[7], out val))
                {
                    _moveMarginGood = val;
                }
                if (int.TryParse(lines[8], out val))
                {
                    _moveMarginExcellent = val;
                }
                if (int.TryParse(lines[9], out val))
                {
                    _scoreEqual = val;
                }
                if (int.TryParse(lines[10], out val))
                {
                    _scoreEdge = val;
                }
                if (int.TryParse(lines[11], out val))
                {
                    _scoreBetter = val;
                }
                if (int.TryParse(lines[12], out val))
                {
                    _halfmoveStart = val;
                }
                if (int.TryParse(lines[13], out val))
                {
                    _halfmoveEnd = val;
                }
                if (int.TryParse(lines[14], out val))
                {
                    _engineMoveTime = val;
                }
                Contract.Assert(14 < maxLines, "14 < maxLines");
            }
        }

        private static readonly Lazy<Config> instance = new Lazy<Config>(() => new Config());

        public Config()
        {
            ReadConfig();
        }

        public static Config Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public string GetEngine()
        {
            Contract.Require(_engine != null, "_engine != null");
            Contract.Ensure(_engine != null, "_engine != null");
            return _engine;
        }

        public int GetEngineHash()
        {
            return _engineHash;
        }

        public int GetEngineThreads()
        {
            return _engineThreads;
        }

        public string GetEngineSyzygyPath()
        {
            Contract.Require(_engineSyzygyPath != null, "_engineSyzygyPath != null");
            Contract.Ensure(_engineSyzygyPath != null, "_engineSyzygyPath != null");
            return _engineSyzygyPath;
        }

        public int GetMoveMarginDubious()
        {
            return _moveMarginDubious;
        }

        public int GetMoveMarginBad()
        {
            return _moveMarginBad;
        }

        public int GetMoveMarginBlunder()
        {
            return _moveMarginBlunder;
        }

        public int GetMoveMarginGood()
        {
            return _moveMarginGood;
        }

        public int GetMoveMarginExcellent()
        {
            return _moveMarginExcellent;
        }

        public int GetScoreEqual()
        {
            return _scoreEqual;
        }

        public int GetScoreEdge()
        {
            return _scoreEdge;
        }

        public int GetScoreBetter()
        {
            return _scoreBetter;
        }

        public int GetHalfmoveStart()
        {
            return _halfmoveStart;
        }

        public int GetHalfmoveEnd()
        {
            return _halfmoveEnd;
        }

        public int GetEngineMoveTime()
        {
            return _engineMoveTime * 1000;
        }

        public void OutputConfigs()
        {
            ChessConsole.Instance.Status(String.Format("engine: {0}", _engine));
            ChessConsole.Instance.Status(String.Format("engine hash (MB): {0}", _engineHash));
            ChessConsole.Instance.Status(String.Format("engine threads: {0}", _engineThreads));
            ChessConsole.Instance.Status(String.Format("engine Syzygy path: {0}", _engineSyzygyPath));
            ChessConsole.Instance.Status(String.Format("move margin dubious (centipawns): {0}", _moveMarginDubious));
            ChessConsole.Instance.Status(String.Format("move margin bad (centipawns): {0}", _moveMarginBad));
            ChessConsole.Instance.Status(String.Format("move margin blunder (centipawns): {0}", _moveMarginBlunder));
            ChessConsole.Instance.Status(String.Format("move margin good (centipawns): {0}", _moveMarginGood));
            ChessConsole.Instance.Status(String.Format("move margin excellent (centipawns): {0}", _moveMarginExcellent));
            ChessConsole.Instance.Status(String.Format("score equal (centipawns): {0}", _scoreEqual));
            ChessConsole.Instance.Status(String.Format("score edge (centipawns): {0}", _scoreEdge));
            ChessConsole.Instance.Status(String.Format("score better (centipawns): {0}", _scoreBetter));
            ChessConsole.Instance.Status(String.Format("halfmove start: {0}", _halfmoveStart));
            ChessConsole.Instance.Status(String.Format("halfmove end: {0}", _halfmoveEnd));
            ChessConsole.Instance.Status(String.Format("engine move time (seconds): {0}", _engineMoveTime));
        }
    }
}
