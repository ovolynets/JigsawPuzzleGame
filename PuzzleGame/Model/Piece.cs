using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace PuzzleGame.Model
{


    public class Piece
    {

        public static readonly double Ampl = 80 / 100.0;
        private static readonly double[] Coords = {
            0, 0, 35, 15, 37, 5,
            37, 5, 40, 0, 38, -5,
            38, -5, 20, -20, 50, -20,
            50, -20, 80, -20, 62, -5,
            62, -5, 60, 0, 63, 5,
            63, 5, 65, 15, 100, 0
        };

        public int Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Position TargetPosition { get; set; }
        public Position Position { get; set; }
        public bool IsPlaced { get; set; }
        public Path Path { get; set; }

        public int UpperConnection { get; set; }
        public int RightConnection { get; set; }
        public int BottomConnection { get; set; }
        public int LeftConnection { get; set; }
        public int RowId { get; set; }
        public int ColId { get; set; }

        public Path CreatePathGeometry()
        {
            var upperPoints = new PointCollection();
            var rightPoints = new PointCollection();
            var bottomPoints = new PointCollection();
            var leftPoints = new PointCollection();

            var scaleX = Width/100.0;
            var scaleY = Height/100.0;

            for (var i = 0; i < Coords.Length/2; i++)
            {
                upperPoints.Add(new Point(scaleX * Coords[i*2], 0 + Ampl * Coords[i*2 + 1]*UpperConnection));
                rightPoints.Add(new Point(Width - Ampl*Coords[i*2 + 1]*RightConnection, scaleY * Coords[i*2]));
                bottomPoints.Add(new Point(Width - scaleX * Coords[i*2], Height - Ampl * Coords[i*2 + 1]*BottomConnection));
                leftPoints.Add(new Point(0 + Ampl * Coords[i*2 + 1]*LeftConnection, Height - scaleY * Coords[i*2]));
            }

            var pathFigure = new PathFigure()
            {
                IsClosed = true,
                StartPoint = new Point(0, 0)
            };

            pathFigure.Segments.Add(new PolyBezierSegment {Points = upperPoints});
            pathFigure.Segments.Add(new PolyBezierSegment {Points = rightPoints});
            pathFigure.Segments.Add(new PolyBezierSegment {Points = bottomPoints});
            pathFigure.Segments.Add(new PolyBezierSegment {Points = leftPoints});


            var pathFigure2 = new PathFigure()
            {
                IsClosed = true,
                StartPoint = new Point(0, 0)
            };

            var geom = new PathGeometry();
            geom.Figures.Add(pathFigure2);
            geom.Figures.Add(pathFigure);
            geom.FillRule = FillRule.Nonzero;

            Path = new Path {Data = geom};
            return Path;
        }

        public double DistanceToTarget()
        {
            var x = Position.X - TargetPosition.X;
            var y = Position.Y - TargetPosition.Y;

            return x*x + y*y;
        }
    }

    public class Position
    {
        public double X { get; set; }
        public double Y { get; set; }

    }

    public enum ConnectionType
    {
        None = 0,
        Blank = -1,
        Tab = 1
    }
}
