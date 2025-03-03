using System.Diagnostics.Meyer.Contracts;
using System.Text;

namespace ChessAnalysis
{
    public class Config
    {
        private string _engine = "vafra_v14.12.2_x86-64_avx2_windows.exe";
        private int _engineHash = 512;
        private int _engineThreads = 1;
        private string _engineSyzygyPath = @"";
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
                var lines = new List<string>();
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    // Remove comments (anything after '#')
                    int commentIndex = line.IndexOf('#');
                    if (commentIndex != -1)
                    {
                        line = line.Substring(0, commentIndex);
                    }
                    // Trim spaces and ignore empty lines
                    line = line.Trim();
                    if (!string.IsNullOrEmpty(line))
                    {
                        lines.Add(line);
                    }
                }
                if (lines.Count < 15)
                {
                    throw new Exception("Config file does not contain enough valid configuration lines.");
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
            ChessConsole.Instance.Status($"Engine:                     {_engine}");
            ChessConsole.Instance.Status($"Engine Hash (MB):           {_engineHash}");
            ChessConsole.Instance.Status($"Engine Threads:             {_engineThreads}");
            ChessConsole.Instance.Status($"Engine Syzygy Path:         {_engineSyzygyPath}");
            ChessConsole.Instance.Status($"Move Margin Dubious (?!):   {_moveMarginDubious} centipawns");
            ChessConsole.Instance.Status($"Move Margin Bad (?):        {_moveMarginBad} centipawns");
            ChessConsole.Instance.Status($"Move Margin Blunder (??):   {_moveMarginBlunder} centipawns");
            ChessConsole.Instance.Status($"Move Margin Good (!):       {_moveMarginGood} centipawns");
            ChessConsole.Instance.Status($"Move Margin Excellent (!!): {_moveMarginExcellent} centipawns");
            ChessConsole.Instance.Status($"Score Equal (=):            {_scoreEqual} centipawns");
            ChessConsole.Instance.Status($"Score Edge (+/= or =/+):    {_scoreEdge} centipawns");
            ChessConsole.Instance.Status($"Score Better (+= or =+):    {_scoreBetter} centipawns (moderate advantage)");
            ChessConsole.Instance.Status($"Score >{_scoreBetter} (+- or -+):      >{_scoreBetter} Winning (+- or -+) (decisive advantage)");
            ChessConsole.Instance.Status($"Halfmove Start:             {_halfmoveStart}");
            ChessConsole.Instance.Status($"Halfmove End:               {_halfmoveEnd}");
            ChessConsole.Instance.Status($"Engine Move Time:           {_engineMoveTime} seconds");
        }
    }
}
