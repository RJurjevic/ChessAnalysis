using System.Diagnostics.Meyer.Contracts;

namespace ChessAnalysis
{
    public class ChessAnalysis
    {
        private ChessGame? _game = null;

        private bool ChangesGameOutcome(int scoreLast, int score)
        {
            bool changes = false;
            bool change = (scoreLast > 0 && score > 0) ? true : false;
            if (Math.Abs(scoreLast) <= Config.Instance.GetScoreEqual())
            {
                if (Math.Abs(score) > Config.Instance.GetScoreEqual())
                {
                    changes = true;
                }
            }
            else if (Math.Abs(scoreLast) > Config.Instance.GetScoreEqual() && Math.Abs(scoreLast) <= Config.Instance.GetScoreEdge())
            {
                if (Math.Abs(score) <= Config.Instance.GetScoreEqual() || Math.Abs(score) > Config.Instance.GetScoreEdge() || change)
                {
                    changes = true;
                }
            }
            else if (Math.Abs(scoreLast) > Config.Instance.GetScoreEdge() && Math.Abs(scoreLast) <= Config.Instance.GetScoreBetter())
            {
                if (Math.Abs(score) <= Config.Instance.GetScoreEdge() || Math.Abs(score) > Config.Instance.GetScoreBetter() || change)
                {
                    changes = true;
                }
            }
            else if (Math.Abs(scoreLast) > Config.Instance.GetScoreBetter())
            {
                if (Math.Abs(score) <= Config.Instance.GetScoreBetter() || change)
                {
                    changes = true;
                }
            }
            return changes;
        }

        private string SetGlyphMove(int delta, bool bestplayLast, int halfmove, int halfmoveStart, int scoreLast, int score)
        {
            string glyphMove = "";
            if (halfmove > halfmoveStart && ChangesGameOutcome(scoreLast, score) == true)
            {
                Contract.Assert(halfmove > halfmoveStart, "halfmove >= halfmoveStart");
                if (bestplayLast == false)
                {
                    if (delta < Config.Instance.GetMoveMarginDubious())
                    {
                        glyphMove = "$6";
                        if (delta < Config.Instance.GetMoveMarginBad())
                        {
                            glyphMove = "$2";
                            if (delta < Config.Instance.GetMoveMarginBlunder())
                            {
                                glyphMove = "$4";
                            }
                        }
                    }
                }
                if (bestplayLast == true)
                {
                    if (delta > 0)
                    {
                        glyphMove = "$5";
                        if (delta > Config.Instance.GetMoveMarginGood())
                        {
                            glyphMove = "$1";
                            if (delta > Config.Instance.GetMoveMarginExcellent())
                            {
                                glyphMove = "$3";
                            }
                        }
                    }
                }
            }
            return glyphMove;
        }

        private string SetGlypScore(int delta, int scoreLast, int score, int halfmove, int halfmoveStart)
        {
            string glyphScore = "";
            if (halfmove >= halfmoveStart && (ChangesGameOutcome(scoreLast, score) == true || halfmove == halfmoveStart))
            {
                Contract.Assert(halfmove >= halfmoveStart, "halfmove >= halfmoveStart");
                if (Math.Abs(score) <= Config.Instance.GetScoreEqual())
                {
                    glyphScore = "$10";
                }
                if (Math.Abs(score) > Config.Instance.GetScoreEqual() && Math.Abs(score) <= Config.Instance.GetScoreEdge())
                {
                    if (halfmove % 2 == 0)
                    {
                        glyphScore = (score > 0) ? "$14" : "$15";
                    }
                    else
                    {
                        glyphScore = (score > 0) ? "$15" : "$14";
                    }
                }
                if (Math.Abs(score) > Config.Instance.GetScoreEdge() && Math.Abs(score) <= Config.Instance.GetScoreBetter())
                {
                    if (halfmove % 2 == 0)
                    {
                        glyphScore = (score > 0) ? "$16" : "$17";
                    }
                    else
                    {
                        glyphScore = (score > 0) ? "$17" : "$16";
                    }
                }
                if (Math.Abs(score) > Config.Instance.GetScoreBetter())
                {
                    if (halfmove % 2 == 0)
                    {
                        glyphScore = (score > 0) ? "$18" : "$19";
                    }
                    else
                    {
                        glyphScore = (score > 0) ? "$19" : "$18";
                    }
                }
            }
            return glyphScore;
        }

        private string GetTheMove(int halfmove, string comment, int fullmove, string moveLast, string glyphMoveSeparator, string glyphMove, string glyphScoreSeparator, string glyphScore)
        {
            Contract.Require(comment != null, "engineComment != null");
            Contract.Require(moveLast != null, "moveLast != null");
            Contract.Require(glyphMoveSeparator != null, "glyphMoveSeparator != null");
            Contract.Require(glyphMove != null, "glyphMove != null");
            Contract.Require(glyphScoreSeparator != null, "glyphScoreSeparator != null");
            Contract.Require(glyphScore != null, "glyphScore != null");
            string theMove = "";
            if (halfmove % 2 == 0)
            {
                if (comment.Length > 0)
                {
                    theMove = String.Format("{0}... {1}{2}{3}{4}{5}{6}{7}{8}", fullmove, moveLast, glyphMoveSeparator, glyphMove, glyphScoreSeparator, glyphScore, " { ", comment, " }");
                }
                else
                {
                    theMove = String.Format("{0}... {1}{2}{3}{4}{5}", fullmove, moveLast, glyphMoveSeparator, glyphMove, glyphScoreSeparator, glyphScore);
                }
            }
            else
            {
                if (comment.Length > 0)
                {
                    theMove = String.Format("{0}. {1}{2}{3}{4}{5}{6}{7}{8}", fullmove, moveLast, glyphMoveSeparator, glyphMove, glyphScoreSeparator, glyphScore, " { ", comment, " }");
                }
                else
                {
                    theMove = String.Format("{0}. {1}{2}{3}{4}{5}", fullmove, moveLast, glyphMoveSeparator, glyphMove, glyphScoreSeparator, glyphScore);
                }
            }
            return theMove;
        }

        private string AnnotateTheMove(int delta, bool bestplayLast, int scoreLast, int score, int halfmove, int halfmoveStart, int halfmoveEnd, int depth, int fullmove, string moveLast)
        {
            Contract.Require(moveLast != null, "moveLast != null");
            string engineComment = "";
            string glyphMove = SetGlyphMove(delta, bestplayLast, halfmove, halfmoveStart, scoreLast, score);
            string glyphScore = SetGlypScore(delta, scoreLast, score, halfmove, halfmoveStart);
            string glyphMoveSeparator = (glyphMove.Length > 0) ? " " : "";
            string glyphScoreSeparator = (glyphScore.Length > 0) ? " " : "";
            if (halfmove >= halfmoveStart && halfmove <= halfmoveEnd)
            {
                int sign = 1;
                if (halfmove % 2 != 0)
                {
                    sign = -1;
                }
                engineComment = (sign * score).ToString("+0;-0;0") + "/" + depth.ToString();
            }
            ChessConsole.Instance.Debug(String.Format("engineComment: {0}", engineComment));
            return GetTheMove(halfmove, "", fullmove, moveLast, glyphMoveSeparator, glyphMove, glyphScoreSeparator, glyphScore);
        }

        private string DoAnalyze(List<ChessGame.ChessPosition> positions, int halfmoveStart, int halfmoveEnd, int engineMoveTime)
        {
            Contract.Require(positions != null, "positions != null");
            Contract.Require(halfmoveStart >= 0, "halfmoveStart >= 0");
            Contract.Require(halfmoveEnd >= halfmoveStart, "halfmoveEnd >= halfmoveStart");
            Contract.Require(engineMoveTime > 0, "engineMoveTime > 0");
            string pgnMoves = "";
            int halfmove = 0;
            int fullmove = 0;
            int scoreLast = 0;
            int depthLast = 0;
            string moveLast = "";
            bool bestplayLast = false;
            string analysisLast = "";
            foreach (ChessGame.ChessPosition position in positions)
            {
                int score = 0;
                int depth = 0;
                bool bestplay = false;
                string analysisThe = "";
                int delta = 0, deltaMax = 0;
                ChessConsole.Instance.Header(String.Format("{0}", "___"));
                ChessConsole.Instance.Debug(String.Format("halfmove: {0}", halfmove));
                ChessConsole.Instance.Debug(String.Format("fullmove: {0}", fullmove));
                string fen = position.GetPosition();
                ChessConsole.Instance.Status(String.Format("fen: {0}", fen));
                if (halfmove >= halfmoveStart && halfmove <= halfmoveEnd)
                {
                    ChessEngine engine = new ChessEngine();
                    Contract.Assert(engine != null, "engine != null");
                    int engineMoveTimeNew = engineMoveTime;
                    delta = deltaMax = 0;
                AnalyzeWithStockfish:
                    ChessConsole.Instance.Status(String.Format("{0} {1}", "Analyze", engineMoveTimeNew));
                    ChessEngine.ChessAnalysis analysis = engine.AnalyzeWithStockfish(engineMoveTimeNew, fen, "");
                    Contract.Assert(analysis != null, "analysis != null");
                    ChessConsole.Instance.Debug(String.Format("bestmove: {0}", analysis.GetBestmove()));
                    ChessConsole.Instance.Debug(String.Format("bestplay: {0}", analysis.GetBestmove() == position.GetMove().GetUci()));
                    score = analysis.GetScore();
                    depth = analysis.GetDepth();
                    ChessConsole.Instance.Debug(String.Format("score: {0}", score));
                    ChessConsole.Instance.Debug(String.Format("depth: {0}", depth));
                    bestplay = analysis.GetBestmove() == position.GetMove().GetUci();
                    analysisThe = analysis.GetAnalysis();
                    if (halfmove > halfmoveStart)
                    {
                        delta = -(score + scoreLast);
                        ChessConsole.Instance.Debug(String.Format("delta: {0}", delta));
                        if (bestplayLast == true && delta > deltaMax && deltaMax <= Config.Instance.GetMoveMarginExcellent())
                        {
                            deltaMax = Math.Max(deltaMax, delta);
                            engineMoveTimeNew *= 2;
                            goto AnalyzeWithStockfish;
                        }
                        ChessConsole.Instance.Debug(String.Format("lastmove: {0}", moveLast));
                        ChessConsole.Instance.Debug(String.Format("lastbestplay: {0}", bestplayLast));
                        ChessConsole.Instance.Debug(String.Format("lastanalysis: {0}", analysisLast));
                    }
                    ChessConsole.Instance.Info(String.Format("analysis: {0}", analysis.GetAnalysis()));
                }
                if (fullmove > 0 && moveLast.Length > 0)
                {
                    string theMove = AnnotateTheMove(delta, bestplayLast, scoreLast, score, halfmove, halfmoveStart, halfmoveEnd, depth, fullmove, moveLast);
                    ChessConsole.Instance.Move(theMove);
                    pgnMoves += theMove + " ";
                    ChessConsole.Instance.Info(String.Format("pgnMoves: {0}", pgnMoves));
                }
                if (halfmove >= halfmoveStart && halfmove <= halfmoveEnd)
                {
                    scoreLast = score;
                    depthLast = depth;
                    bestplayLast = bestplay;
                    analysisLast = analysisThe;
                }
                moveLast = position.GetMove().GetSan();
                string san = position.GetMove().GetSan();
                ChessConsole.Instance.Debug(String.Format("move: {0}", san));
                halfmove++;
                fullmove = (halfmove + 1) / 2;
                if (fullmove > 0 && san.Length > 0)
                {
                    if (halfmove % 2 == 0)
                    {
                        ChessConsole.Instance.Debug(String.Format("{0}... {1}", fullmove, moveLast));
                    }
                    else
                    {
                        ChessConsole.Instance.Debug(String.Format("{0}. {1}", fullmove, moveLast));
                    }
                }
            }
            return pgnMoves;
        }

        public ChessAnalysis(ChessGame game)
        {
            Contract.Require(game != null, "game != null");
            _game = game;
            Contract.Ensure(_game != null, "_game != null");
        }

        public string Analyze(string file, int halfmoveStart, int halfmoveEnd, int engineMoveTime)
        {
            Contract.Require(file != null, "file != null");
            Contract.Require(halfmoveStart >= 0, "halfmoveStart >= 0");
            Contract.Require(halfmoveEnd >= halfmoveStart, "halfmoveEnd >= halfmoveStart");
            Contract.Require(engineMoveTime > 0, "engineMoveTime > 0");
            Contract.Require(_game != null, "_game != null");
            ChessGame game = _game;
            ChessConsole.Instance.Info(String.Format("{0}", "Loading game..."));
            game.LoadFromPgn(file);
            string pgn = game.GetPgn();
            Contract.Assert(pgn != null, "pgn != null");
            ChessConsole.Instance.Info(String.Format("pgn: {0}", pgn));
            ChessConsole.Instance.Info(String.Format("{0}", "Getting positions..."));
            List<ChessGame.ChessPosition> positions = game.GetPositions();
            Contract.Assert(positions != null, "positions != null");
            ChessConsole.Instance.Header(String.Format("{0}", "Analyzing..."));
            string pgnMoves = DoAnalyze(positions, halfmoveStart, halfmoveEnd, engineMoveTime);
            string annotatedPgn = game.GetAnnotatedPgn(pgnMoves);
            ChessConsole.Instance.Status(String.Format("annotatedPgn: {0}", annotatedPgn));
            return annotatedPgn;
        }
    }
}
