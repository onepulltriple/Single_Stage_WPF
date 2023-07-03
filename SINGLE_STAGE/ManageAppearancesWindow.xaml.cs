using SINGLE_STAGE.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace SINGLE_STAGE
{
    /// <summary>
    /// Interaction logic for ManageAppearancesWindow.xaml
    /// </summary>
    public partial class ManageAppearancesWindow : Window, INotifyPropertyChanged
    {
        readonly SingleStageContext _context;

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Appearance> _listOfAppearances;
        public List<Appearance> ListOfAppearances
        {
            get { return _listOfAppearances; }
            set
            {
                _listOfAppearances = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListOfAppearances)));
            }
        }

        private Appearance _selectedAppearance;
        public Appearance SelectedAppearance
        {
            get { return _selectedAppearance; }
            set
            {
                _selectedAppearance = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAppearance)));
            }
        }

        private Appearance _tempAppearance;
        public Appearance TempAppearance
        {
            get { return _tempAppearance; }
            set
            {
                _tempAppearance = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TempAppearance)));
            }
        }

        private List<Performance> _listOfPerformances;
        public List<Performance> ListOfPerformances
        {
            get { return _listOfPerformances; }
            set
            {
                _listOfPerformances = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListOfPerformances)));
            }
        }

        private Performance _selectedPerformance;
        public Performance SelectedPerformance
        {
            get { return _selectedPerformance; }
            set
            {
                _selectedPerformance = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPerformance)));
            }
        }

        private List<Artist> _listOfArtists;
        public List<Artist> ListOfArtists
        {
            get { return _listOfArtists; }
            set
            {
                _listOfArtists = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListOfArtists)));
            }
        }

        private Artist _selectedArtist;
        public Artist SelectedArtist
        {
            get { return _selectedArtist; }
            set
            {
                _selectedArtist = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedArtist)));
            }
        }


        public ManageAppearancesWindow()
        {
            InitializeComponent();
            DataContext = this;
            _context = new();

            LoadAllAppearances();
            LoadAllPerformances();
            LoadAllArtists();
        }

        private void LoadAllAppearances()
        {
            ListOfAppearances = new(_context.Appearances
                .OrderBy(Appearance => Appearance.Performance.StartTime)
                .ToArray()
                );

            ResetButtons();

            CloseUserInputFields();
        }

        private void LoadAllPerformances()
        {
            ListOfPerformances = new(_context.Performances
                .OrderBy(performance => performance.StartTime)
                .ToArray()
                );
        }

        private void LoadAllArtists()
        {
            ListOfArtists = new(_context.Artists
                .OrderBy(artist => artist.Name)
                .ToArray()
                );
        }

        private void CloseUserInputFields()
        {
            UI01.IsEnabled = false;
            UI02.IsEnabled = false;
            UI03.IsEnabled = false;
            UI04.IsEnabled = false;
            UI05.IsEnabled = false;
            CB01.IsEnabled = false;
            UI07.IsEnabled = false;
            CB02.IsEnabled = false;
        }

        private void OpenUserInputFields()
        {
            UI01.IsEnabled = true;
            UI02.IsEnabled = true;
            UI03.IsEnabled = true;
            UI04.IsEnabled = true;
            UI05.IsEnabled = true;
            CB01.IsEnabled = true;
            UI07.IsEnabled = true;
            CB02.IsEnabled = true;
        }

        private void ResetButtons()
        {
            DG01.UnselectAll();
            DG01.IsEnabled = true;

            CREAButton.IsEnabled = true;
            EDITButton.IsEnabled = false;
            SAVEButton.IsEnabled = false;
            DELEButton.IsEnabled = false;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            if (SAVEButton.IsEnabled ||
                EDITButton.IsEnabled && DELEButton.IsEnabled)
            {
                ResetButtons();
                CloseUserInputFields();
                return;
            }

            MainWindow main = new();
            main.Show();
            this.Close();
        }

        private void DG01SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonsInManageMode();
        }

        private void ButtonsInManageMode()
        {
            CREAButton.IsEnabled = false;
            EDITButton.IsEnabled = true;
            SAVEButton.IsEnabled = false;
            DELEButton.IsEnabled = true;
        }

        private void CREAButtonClicked(object sender, RoutedEventArgs e)
        {
            ButtonsInEditMode();
            OpenUserInputFields();

            TempAppearance = new();

            // set user inputs fields which are not bound to the temp Appearance
            CB01.SelectedItem = null;
            CB02.SelectedItem = null;
        }

        private void ButtonsInEditMode()
        {
            CREAButton.IsEnabled = false;
            EDITButton.IsEnabled = false;
            SAVEButton.IsEnabled = true;
            DELEButton.IsEnabled = false;

            DG01.IsEnabled = false;
        }

        private void EDITButtonClicked(object sender, RoutedEventArgs e)
        {
            if (SelectedAppearance == null)
            {
                MessageBox.Show("Please select an Appearance.");
                return;
            }

            ButtonsInEditMode();
            OpenUserInputFields();

            TempAppearance = TransferProperties(new Appearance(), SelectedAppearance);

            // fill out user inputs fields which are not bound to the temp Appearance
            SelectedArtist = TempAppearance.Artist;
            SelectedPerformance = TempAppearance.Performance;

        }

        private void SAVEButtonClicked(object sender, RoutedEventArgs e)
        {
            bool ChecksWerePassed = PerformChecksOnUserInput();
            TempAppearance.Artist = SelectedArtist;
            TempAppearance.Performance = SelectedPerformance;

            // when editing an existing Appearance
            if (ChecksWerePassed && SelectedAppearance != null)
            {
                SelectedAppearance = TransferProperties(SelectedAppearance, TempAppearance);
                _context.Update(SelectedAppearance);
                _context.SaveChanges();
                LoadAllAppearances();
                return;
            }

            // when editing a new Appearance
            if (ChecksWerePassed)
            {
                _context.Add(TempAppearance);
                _context.SaveChanges();
                LoadAllAppearances();
                return;
            }
        }

        private void DELEButtonClicked(object sender, RoutedEventArgs e)
        {
            if (SelectedAppearance == null)
            {
                MessageBox.Show("Please select an Appearance.");
                return;
            }

            MessageBoxResult answer = MessageBoxResult.No;

            answer = MessageBox.Show("Are you sure you want to delete the selected Appearance?", "If you do this, you will get what you deserve.", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (answer == MessageBoxResult.Yes)
            {
                _context.Remove(SelectedAppearance);
                _context.SaveChanges();
                LoadAllAppearances();
            }

            ResetButtons();
        }

        private bool PerformChecksOnUserInput()
        {
            // block if any field is empty
            if (TempAppearance.RoyaltyAdvance == null ||
                TempAppearance.RoyaltyAtEnd == null ||
                CB01.SelectedItem == null ||
                CB02.SelectedItem == null)
            {
                MessageBox.Show("Please fill out all fields.");
                return false;
            }

            // if all checks have been passed
            return true;
        }

        private Appearance TransferProperties(Appearance toFill, Appearance origin)
        {
            toFill.Id = origin.Id;
            toFill.RoyaltyAdvance = origin.RoyaltyAdvance;
            toFill.RoyaltyAtEnd = origin.RoyaltyAtEnd;
            toFill.ArtistId = origin.ArtistId;
            toFill.PerformanceId = origin.PerformanceId;

            toFill.Artist = origin.Artist;
            toFill.Performance = origin.Performance;

            return toFill;
        }
    }
}
