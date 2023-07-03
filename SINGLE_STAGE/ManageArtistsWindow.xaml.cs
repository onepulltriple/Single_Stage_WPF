using SINGLE_STAGE.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SINGLE_STAGE
{
    /// <summary>
    /// Interaction logic for ManageArtistsWindow.xaml
    /// </summary>
    public partial class ManageArtistsWindow : Window, INotifyPropertyChanged
    {
        readonly SingleStageContext _context;

        public event PropertyChangedEventHandler PropertyChanged;

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

        private Artist _tempArtist;
        public Artist TempArtist
        {
            get { return _tempArtist; }
            set
            {
                _tempArtist = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TempArtist)));
            }
        }

        public ManageArtistsWindow()
        {
            InitializeComponent();
            DataContext = this;
            _context = new();

            LoadAllArtists();
        }

        private void LoadAllArtists()
        {
            ListOfArtists = new(_context.Artists
                .OrderBy(artist => artist.Name)
                .ToArray()
                );

            ResetButtons();

            CloseUserInputFields();
        }

        private void CloseUserInputFields()
        {
            UI01.IsEnabled = false;
            UI02.IsEnabled = false;
        }

        private void OpenUserInputFields()
        {
            UI01.IsEnabled = true;
            UI02.IsEnabled = true;
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

            TempArtist = new();

            // clear user inputs fields which are not bound to the temp artist
            //
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
            if (SelectedArtist == null)
            {
                MessageBox.Show("Please select an artist.");
                return;
            }

            ButtonsInEditMode();
            OpenUserInputFields();

            TempArtist = TransferProperties(new Artist(), SelectedArtist);

            // fill out user inputs fields which are not bound to the temp artist
            //
        }

        private void SAVEButtonClicked(object sender, RoutedEventArgs e)
        {
            bool ChecksWerePassed = PerformChecksOnUserInput();

            // when editing an existing artist
            if (ChecksWerePassed && SelectedArtist != null)
            {
                SelectedArtist = TransferProperties(SelectedArtist, TempArtist);
                _context.Update(SelectedArtist);
                _context.SaveChanges();
                LoadAllArtists();
                return;
            }

            // when editing a new artist
            if (ChecksWerePassed)
            {
                _context.Add(TempArtist);
                _context.SaveChanges();
                LoadAllArtists();
                return;
            }
        }

        private void DELEButtonClicked(object sender, RoutedEventArgs e)
        {
            if (SelectedArtist == null)
            {
                MessageBox.Show("Please select an artist.");
                return;
            }

            MessageBoxResult answer = MessageBoxResult.No;

            answer = MessageBox.Show("Are you sure you want to delete the selected artist?", "If you do this, you will get what you deserve.", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (answer == MessageBoxResult.Yes)
            {
                _context.Remove(SelectedArtist);
                _context.SaveChanges();
                LoadAllArtists();
            }

            ResetButtons();
        }

        private bool PerformChecksOnUserInput()
        {
            // block if any field is empty
            if (TempArtist.Name == null)
            {
                MessageBox.Show("Please fill out all fields.");
                return false;
            }

            // if all checks have been passed
            return true;
        }

        private Artist TransferProperties(Artist toFill, Artist origin)
        {
            toFill.Id = origin.Id;
            toFill.Name = origin.Name;

            return toFill;
        }
    }
}
