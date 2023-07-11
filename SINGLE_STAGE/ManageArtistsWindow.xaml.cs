using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SINGLE_STAGE.CRUD_logic;
using SINGLE_STAGE.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace SINGLE_STAGE
{
    /// <summary>
    /// Interaction logic for ManageArtistsWindow.xaml
    /// </summary>
    public partial class ManageArtistsWindow : Window, INotifyPropertyChanged
    {
        #region Class Members

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

        #endregion

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
                .Include(artist => artist.Appearances)
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

            BACKButton.Visibility = Visibility.Visible;
            CANCButton.Visibility = Visibility.Hidden;

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

            CRUDWindowWPF.ReturnToMainWindowAndClose(this);
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            CRUDWindowWPF.ReturnToMainWindowAndClose(this);
        }

        private void DG01SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonsInManageMode();
        }

        private void ButtonsInManageMode()
        {
            BACKButton.Visibility = Visibility.Hidden;
            CANCButton.Visibility = Visibility.Visible;

            CREAButton.IsEnabled = false;
            EDITButton.IsEnabled = true;
            SAVEButton.IsEnabled = false;
            DELEButton.IsEnabled = true;
        }

        private void ButtonsInEditMode()
        {
            BACKButton.Visibility = Visibility.Hidden;
            CANCButton.Visibility = Visibility.Visible;

            CREAButton.IsEnabled = false;
            EDITButton.IsEnabled = false;
            SAVEButton.IsEnabled = true;
            DELEButton.IsEnabled = false;

            DG01.IsEnabled = false;
        }

        private void CREAButtonClicked(object sender, RoutedEventArgs e)
        {
            ButtonsInEditMode();
            OpenUserInputFields();

            TempArtist = new();

            // clear user inputs fields which are not bound to the temp artist
            //
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

            MessageBoxResult answer01 = MessageBoxResult.No;
            MessageBoxResult answer02 = MessageBoxResult.No;

            answer01 = MessageBox.Show(
                "Are you sure you want to delete the selected artist?",
                "Confirm artist deletion.",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer01 == MessageBoxResult.Yes)
            {
                // check if the artist has booked appearances
                List<Appearance> AppearancesBookedByArtist = new(
                    SelectedArtist.Appearances.ToArray()
                    );

                // if the artist has booked appearances
                if (!AppearancesBookedByArtist.IsNullOrEmpty())
                {
                    int countOfAppearances = AppearancesBookedByArtist.Count();

                    string messageToUser =
                        $"The selected artist has\n" +
                        $"{countOfAppearances} appearances booked.\n" +
                        $"Deleting this artist will delete all of their booked appearances.\n" +
                        $"Are you sure you want to delete the selected artist?";

                    // ask follow up question
                    answer02 = MessageBox.Show(messageToUser,
                        "If you do this, you will get what you deserve.",
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                }
                else
                {
                    // artist has no bookings and can be deleted without follow up questions asked
                    answer02 = MessageBoxResult.Yes;
                }

                if (answer01 == MessageBoxResult.Yes &&
                    answer02 == MessageBoxResult.Yes)
                {
                    // proceed with artist deletion (cascading delete in SQL is used)
                    _context.Remove(SelectedArtist);
                    _context.SaveChanges();
                    LoadAllArtists();
                }
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
