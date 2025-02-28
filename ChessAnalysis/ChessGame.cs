using Chess;
using System.Diagnostics.Meyer.Contracts;

namespace ChessAnalysis
{
    public class ChessGame
    {
        public class ChessMove
        {
            private string _fen = "";
            private string _uci = "";
            private string _san = "";

            public ChessMove(string fen, string uci, string san)
            {
                Contract.Require(fen != null, "fen != null");
                Contract.Require(uci != null, "uci != null");
                Contract.Require(san != null, "san != null");
                _fen = fen;
                _uci = uci;
                _san = san;
                Contract.Ensure(_fen != null, "_fen != null");
                Contract.Ensure(_uci != null, "_uci != null");
                Contract.Ensure(_san != null, "_san != null");
            }

            public string GetPosition()
            {
                return _fen;
            }

            public string GetUci()
            {
                return _uci;
            }

            public string GetSan()
            {
                return _san;
            }
        }

        public class ChessPosition
        {
            private string _fen = "";
            private ChessMove? _move = null;

            public ChessPosition(string fen, ChessMove move)
            {
                Contract.Require(fen != null, "fen != null");
                Contract.Require(move != null, "move != null");
                _fen = fen;
                _move = move;
                Contract.Ensure(_fen != null, "_fen != null");
                Contract.Ensure(_move != null, "_move != null");
            }

            public string GetPosition()
            {
                return _fen;
            }

            public ChessMove GetMove()
            {
                return _move;
            }
        }

        private string _pgn = "";
        private ChessBoard _board = new ChessBoard();
        private List<ChessPosition> _positions = new List<ChessPosition>();

        private string MoveToUcimove(Move move)
        {
            Contract.Require(move != null, "move != null");
            string ucimove = "";
            string chessmove = move.ToString();
            string[] parts = chessmove.Trim('{', '}').Split(new string[] { " - " }, StringSplitOptions.None);
            if (parts.Length >= 3)
            {
                ucimove = parts[1] + parts[2];
            }
            Contract.Ensure(ucimove != null, "ucimove != null");
            Contract.Ensure(ucimove.Length == 4, "ucimove.Length == 4");
            return ucimove;
        }

        private void GeneratePositions()
        {
            Contract.Require(_board != null, "_board != null");
            Contract.Require(_positions != null, "_positions != null");
            List<string> moves = _board.MovesToSan;
            ChessBoard board = new ChessBoard();
            foreach (string move in moves)
            {
                Move m = board.ParseFromSan(move);
                ChessMove mv = new ChessMove(board.ToFen(), MoveToUcimove(m), move);
                Contract.Assert(mv != null, "mv != null");
                ChessPosition pos = new ChessPosition(board.ToFen(), mv);
                _positions.Add(pos);
                board.Move(move);
            }
            _positions.Add(new ChessPosition(board.ToFen(), new ChessMove(board.ToFen(), "", "")));
        }

        public ChessGame()
        {
        }

        public void LoadFromPgn(string file)
        {
            Contract.Require(file != null, "file != null");
            _pgn = System.IO.File.ReadAllText(file);
            Contract.Assert(_pgn != null, "_pgn != null");
            _board = ChessBoard.LoadFromPgn(_pgn);
            Contract.Assert(_board != null, "_board != null");
            GeneratePositions();
            Contract.Assert(_positions != null, "_positions != null");
            Contract.Ensure(_pgn != null, "_pgn != null");
            Contract.Ensure(_positions != null, "_positions != null");
        }

        public string GetPgn()
        {
            Contract.Assert(_pgn != null, "_pgn != null");
            return _pgn;
        }

        public string GetAnnotatedPgn(string pgnMoves)
        {
            Contract.Require(pgnMoves != null, "pgnMoves != null");
            Contract.Require(_board != null, "_board != null");
            string pgn = "";
            ChessBoard board = _board;
            IReadOnlyDictionary<string, string> headers = board.Headers;
            if (headers.ContainsKey("Event"))
            {
                pgn += "[Event \"" + headers["Event"] + "\"]" + Environment.NewLine;
            }
            if (headers.ContainsKey("Site"))
            {
                pgn += "[Site \"" + headers["Site"] + "\"]" + Environment.NewLine;
            }
            if (headers.ContainsKey("Date"))
            {
                pgn += "[Date \"" + headers["Date"] + "\"]" + Environment.NewLine;
            }
            if (headers.ContainsKey("Round"))
            {
                pgn += "[Round \"" + headers["Round"] + "\"]" + Environment.NewLine;
            }
            if (headers.ContainsKey("White"))
            {
                pgn += "[White \"" + headers["White"] + "\"]" + Environment.NewLine;
            }
            if (headers.ContainsKey("Black"))
            {
                pgn += "[Black \"" + headers["Black"] + "\"]" + Environment.NewLine;
            }
            if (headers.ContainsKey("Result"))
            {
                pgn += "[Result \"" + headers["Result"] + "\"]" + Environment.NewLine;
            }
            if (headers.ContainsKey("WhiteElo"))
            {
                pgn += "[WhiteElo \"" + headers["WhiteElo"] + "\"]" + Environment.NewLine;
            }
            if (headers.ContainsKey("BlackElo"))
            {
                pgn += "[BlackElo \"" + headers["BlackElo"] + "\"]" + Environment.NewLine;
            }
            if (headers.ContainsKey("PlyCount"))
            {
                pgn += "[PlyCount \"" + headers["PlyCount"] + "\"]" + Environment.NewLine;
            }
            if (Config.Instance.GetEngine().Length > 0)
            {
                pgn += "[Annotator \"" + Config.Instance.GetEngine() + " " + (Config.Instance.GetEngineMoveTime() / 1000).ToString() + "\"]" + Environment.NewLine;
            }
            pgn += Environment.NewLine;
            pgn += pgnMoves.TrimEnd();
            string result = "*";
            if (board.IsEndGame)
            {
                //EndgameType endgameType = board.EndGame.EndgameType;
                if (board.EndGame.WonSide != null)
                {
                    PieceColor wonSide = board.EndGame.WonSide;
                    if (wonSide == PieceColor.White)
                    {
                        result = "1-0";
                    }
                    if (wonSide == PieceColor.Black)
                    {
                        result = "0-1";
                    }
                }
                else
                {
                    result = "1/2-1/2";
                }
                
            }
            pgn += " " + result;
            return pgn;
        }

        public List<ChessPosition> GetPositions()
        {
            Contract.Assert(_positions != null, "_positions != null");
            Contract.Ensure(_positions != null, "_positions != null");
            return _positions;
        }
    }
}
