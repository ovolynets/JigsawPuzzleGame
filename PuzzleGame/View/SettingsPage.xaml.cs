using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using PuzzleGame.Model;

namespace PuzzleGame.View
{
    public sealed partial class SettingsPage
    {
        public int SelectedPuzzleSizeIndex { get; set; } = 0;

        public ObservableCollection<Tuple<int, int>> BoardSizes { get; set; } = new ObservableCollection<Tuple<int, int>>();

        public bool IsSoundEnabled => GameSettings.IsSoundEnabled;

        public SettingsPage()
        {
            InitializeComponent();
            Loaded += SettingsPage_Loaded;
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var boardSize in GameSettings.BoardSizes)
            {
                BoardSizes.Add(boardSize);
            }
            SizeSelector.SelectedIndex = GameSettings.SelectedBoardSizeIndex;
        }

        private void SizeSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var tuple = comboBox?.SelectedItem as Tuple<int, int>;
            if (tuple == null) return;

            ApplicationData.Current.LocalSettings.Values["boardSizeIndex"] = comboBox?.SelectedIndex;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null) return;

            GameSettings.IsSoundEnabled = checkBox.IsChecked ?? true;

            ApplicationData.Current.LocalSettings.Values["soundEnabled"] = checkBox.IsChecked ?? true;
        }

        private void MainMenu_OnClick(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof (MainPage));
        }

        private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch == null) return;

            GameSettings.IsSoundEnabled = toggleSwitch.IsOn;

            ApplicationData.Current.LocalSettings.Values["soundEnabled"] = toggleSwitch.IsOn;
        }
    }
}
