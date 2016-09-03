using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using PuzzleGame.Annotations;
using PuzzleGame.Model;

namespace PuzzleGame.View
{
    public sealed partial class GameBoardPage : INotifyPropertyChanged
    {
        public GameBoardPage()
        {
            InitializeComponent();
            Loaded += GameBoardPage_Loaded;
            SizeChanged += SizeChangedEventHandler;
        }

        private void SizeChangedEventHandler(object sender, SizeChangedEventArgs e)
        {
            foreach (var piece in _pieces)
            {
                var ttv = SourceImage.TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));
                piece.TargetPosition = new Position
                {
                    X = ttv.X + piece.ColId*_pieceWidth,
                    Y = ttv.Y + piece.RowId*_pieceHeight
                };
            }
            foreach (var piece in _pieces.Where(p => p.IsPlaced))
            {
                piece.Position = piece.TargetPosition;
                Canvas.SetLeft(piece.Path, piece.Position.X);
                Canvas.SetTop(piece.Path, piece.Position.Y);
            }
            Bindings.Update();
        }

        private ImageFile _imageFile;
        public ImageFile ImageFile
        {
            get { return _imageFile; }
            set
            {
                _imageFile = value;
                RaisePropertyChanged();
            }
        }

        private BitmapImage _sourcePicture;

        private void GameBoardPage_Loaded(object sender, RoutedEventArgs e)
        {
            CreateJigsawPuzzle();
            NextImageButton.Visibility = Visibility.Collapsed;
        }

        private Piece _currentPiece;

        private List<Piece> _pieces = new List<Piece>();
        private double _pieceWidth;
        private double _pieceHeight;

        private void CreateJigsawPuzzle()
        {
            var selectedImage = _imageFile == null
                ? StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/sample/sample.jpg")).AsTask().Result
                : DataService.GalleryFolder.GetFileAsync(_imageFile.FileName).AsTask().Result;

            var stream = selectedImage.OpenReadAsync().AsTask().Result;
            _sourcePicture = new BitmapImage { DecodePixelWidth = (int) SourceImage.ActualWidth};
            _sourcePicture.SetSource(stream);

            if (_sourcePicture == null)
            {
                return;
            }

            var boardSize = GameSettings.BoardSizes[GameSettings.SelectedBoardSizeIndex];

            _pieceWidth = SourceImage.ActualWidth / boardSize.Item1;
            _pieceHeight = SourceImage.ActualHeight / boardSize.Item2;

            ConstructJigsawPieces();
        }

        private void ConstructJigsawPieces()
        {
            _currentPiece = null;
            _pieces = new List<Piece>();

            var pieceId = 0;

            var rand = new Random();

            var boardSize = GameSettings.BoardSizes[GameSettings.SelectedBoardSizeIndex];
            var boardSizeCol = boardSize.Item1;
            var boardSizeRow = boardSize.Item2;

            for (var row = 0; row < boardSizeRow; row++)
            {
                for (var col = 0; col < boardSizeCol; col++)
                {

                    var connections = new[] { (int)ConnectionType.Tab, (int)ConnectionType.Blank };

                    var upperConnection = (int)ConnectionType.None;
                    var rightConnection = (int)ConnectionType.None;
                    var bottomConnection = (int)ConnectionType.None;
                    var leftConnection = (int)ConnectionType.None;

                    if (row > 0)
                        upperConnection = -1*_pieces[(row - 1)*boardSizeCol + col].BottomConnection;

                    if (col < boardSizeCol - 1)
                        rightConnection = connections[rand.Next(2)];

                    if (row < boardSizeRow - 1)
                        bottomConnection = connections[rand.Next(2)];

                    if (col > 0)
                        leftConnection = -1*_pieces.Last().RightConnection;

                    var ttv = SourceImage.TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));
                    var piece = new Piece
                    {
                        Id = pieceId,
                        Width = _pieceWidth,
                        Height = _pieceHeight,
                        RowId = row,
                        ColId = col,
                        Position = new Position
                        {
                            X = rand.Next(200),
                            Y = 100+rand.Next(500)
                        },
                        TargetPosition = new Position
                        {
                            X = ttv.X + col * _pieceWidth,
                            Y = ttv.Y + row * _pieceHeight
                        },
                        UpperConnection = upperConnection,
                        RightConnection = rightConnection,
                        BottomConnection = bottomConnection,
                        LeftConnection = leftConnection
                    };

                    var path = piece.CreatePathGeometry();

                    var shiftX = 0;
                    if (leftConnection == 1)
                        shiftX = 16;
                    else if (leftConnection == -1)
                        shiftX = 8;

                    var shiftY = 0;
                    if (upperConnection == 1)
                        shiftY = 16;
                    else if (upperConnection == -1)
                        shiftY = 8;

                    path.Fill = new ImageBrush
                    {
                        AlignmentX = AlignmentX.Left,
                        AlignmentY = AlignmentY.Top,
                        ImageSource = _sourcePicture,
                        Stretch = Stretch.None,
                        Transform = new TranslateTransform {
                            X = -col * _pieceWidth + shiftX,
                            Y = - row * _pieceHeight + shiftY
                        }
                    };

                    path.StrokeThickness = 3;
                    path.Stroke = new SolidColorBrush(Colors.Black);

                    path.ManipulationMode = ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                    path.ManipulationDelta += UIElement_OnManipulationDelta;
                    path.ManipulationStarted += UIElement_OnManipulationStarted;
                    path.ManipulationCompleted += UIElement_OnManipulationCompleted;

                    _pieces.Add(piece);

                    GameBoard.Children.Add(piece.Path);
                    Canvas.SetLeft(piece.Path, piece.Position.X);
                    Canvas.SetTop(piece.Path, piece.Position.Y);

                    pieceId++;
                }
            }
        }

        private void UIElement_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            var image = (Path)sender;
            foreach (var piece in _pieces.Where(piece => piece.Path != image).Where(piece => !piece.IsPlaced))
            {
                piece.Path.Visibility = Visibility.Collapsed;
            }
        }

        private void UIElement_OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var image = (Path)sender;
            foreach (var piece in _pieces.Where(piece => piece.Path != image))
            {
                piece.Path.Visibility = Visibility.Visible;
            }
        }

        private void UIElement_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var image = (Path)sender;

            _currentPiece = _pieces.Find(p => p.Path == image);

            if (_currentPiece != null && !_currentPiece.IsPlaced)
            {

                _currentPiece.Position.X += e.Delta.Translation.X;
                _currentPiece.Position.Y += e.Delta.Translation.Y;

                Canvas.SetLeft(image, Canvas.GetLeft(image) + e.Delta.Translation.X);
                Canvas.SetTop(image, Canvas.GetTop(image) + e.Delta.Translation.Y);

                if (_currentPiece.DistanceToTarget() < GameSettings.SnapTolerance)
                {
                    Canvas.SetLeft(image, _currentPiece.TargetPosition.X);
                    Canvas.SetTop(image, _currentPiece.TargetPosition.Y);
                    _currentPiece.IsPlaced = true;
                    image.ManipulationMode = ManipulationModes.None;
                    var zIndexMin = _pieces.Where(x => x != _currentPiece).Select(w => Canvas.GetZIndex(w.Path)).Min();
                    Canvas.SetZIndex(image, zIndexMin - 1);
                    PlaySound("131333__kaonaya__clap-3.wav", 200);
                    VerifyCompletion();
                }
            }
        }

        private async void PlaySound(string fileName, int delay)
        {
            if (!GameSettings.IsSoundEnabled)
            {
                return;
            }
            MediaElement.Source = new Uri(string.Format("ms-appx:///Assets/sounds/{0}", fileName));
            await Task.Delay(TimeSpan.FromMilliseconds(delay));
            await MediaElement.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MediaElement.Play() );
        }

        private void VerifyCompletion()
        {
            if (_pieces.All(p => p.IsPlaced))
            {
                PlaySound("273925__lemonjolly__hooray-yeah.wav", 1000);
                NextImageButton.Visibility = Visibility.Visible;
                SourceImage.Opacity = 1;
//                ImageAppearance.Begin();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ImageFile = e.Parameter as ImageFile;
        }

        private void BackButton_Pressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof (FileListPage));
        }

        private void NextButton_Pressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(GameBoardPage), DataService.ChooseNext(ImageFile));
        }

        private void HomeButton_Pressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
