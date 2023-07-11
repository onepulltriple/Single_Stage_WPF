using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SINGLE_STAGE.CRUD_logic;
using SINGLE_STAGE.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace SINGLE_STAGE
{
    /// <summary>
    /// Interaction logic for ManagePerformancesWindow.xaml
    /// </summary>
    public partial class ManagePerformancesWindow : Window, INotifyPropertyChanged
    {
        #region Class Members

        readonly SingleStageContext _context;

        public event PropertyChangedEventHandler PropertyChanged;

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

        private Performance _tempPerformance;
        public Performance TempPerformance
        {
            get { return _tempPerformance; }
            set
            {
                _tempPerformance = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TempPerformance)));
            }
        }

        private string _enteredStartTime;
        public string EnteredStartTime
        {
            get { return _enteredStartTime; }
            set
            {
                _enteredStartTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EnteredStartTime)));
            }
        }

        private string _enteredEndTime;
        public string EnteredEndTime
        {
            get { return _enteredEndTime; }
            set
            {
                _enteredEndTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EnteredEndTime)));
            }
        }

        private List<Cavent> _listOfCavents;
        public List<Cavent> ListOfCavents
        {
            get { return _listOfCavents; }
            set
            {
                _listOfCavents = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListOfCavents)));
            }
        }

        private Cavent _selectedCavent;
        public Cavent SelectedCavent
        {
            get { return _selectedCavent; }
            set
            {
                _selectedCavent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCavent)));
            }
        }

        #endregion

        public ManagePerformancesWindow()
        {
            InitializeComponent();
            DataContext = this;
            _context = new();

            LoadAllPerformances();
            LoadAllCavents();
        }

        private void LoadAllPerformances()
        {
            ListOfPerformances = new(_context.Performances
                .OrderBy(performance => performance.StartTime)
                .Include(performance => performance.Appearances)
                .ToArray()
                );

            ResetButtons();

            CloseUserInputFields();
        }

        private void LoadAllCavents()
        {
            ListOfCavents = new(_context.Cavents
                .OrderBy(cavent => cavent.StartTime)
                .ToArray()
                );
        }

        private void CloseUserInputFields()
        {
            UI01.IsEnabled = false;
            UI02.IsEnabled = false;
            UI03.IsEnabled = false;
            DP01.IsEnabled = false;
            UI05.IsEnabled = false;
            UI06.IsEnabled = false;
            UI07.IsEnabled = false;
            DP02.IsEnabled = false;
            UI09.IsEnabled = false;
            UI10.IsEnabled = false;
            UI11.IsEnabled = false;
            CB01.IsEnabled = false;
        }

        private void OpenUserInputFields()
        {
            UI01.IsEnabled = true;
            UI02.IsEnabled = true;
            UI03.IsEnabled = true;
            DP01.IsEnabled = true;
            UI05.IsEnabled = true;
            UI06.IsEnabled = true;
            UI07.IsEnabled = true;
            DP02.IsEnabled = true;
            UI09.IsEnabled = true;
            UI10.IsEnabled = true;
            //UI11.IsEnabled = true;
            //CB01.IsEnabled = true;
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

        private void CB01SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // when the selected item is switched to null, do nothing further
            if (CB01.SelectedItem == null)
                return;

            // open remaining user input fields, since an event has been selected
            OpenUserInputFields();

            // fill in user input fields based on the selected event
            DP01.SelectedDate = SelectedCavent.StartTime;
            EnteredStartTime = SelectedCavent.StartTime.ToString("HH:mm"); 
            DP02.SelectedDate = SelectedCavent.EndTime;
            EnteredEndTime = SelectedCavent.EndTime.ToString("HH:mm");
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

            // open combo box only to select an event first
            CB01.SelectedItem = null;
            UI11.IsEnabled = true;
            CB01.IsEnabled = true;

            TempPerformance = new();

            // clear user inputs fields which are not bound to the temp performance
            DP01.SelectedDate = null;
            EnteredStartTime = null;
            DP02.SelectedDate = null;
            EnteredEndTime = null;
        }

        private void EDITButtonClicked(object sender, RoutedEventArgs e)
        {
            if (SelectedPerformance == null)
            {
                MessageBox.Show("Please select a performance.");
                return;
            }

            ButtonsInEditMode();
            UI11.IsEnabled = true;
            CB01.IsEnabled = true;
            OpenUserInputFields();

            TempPerformance = TransferProperties(new Performance(), SelectedPerformance);

            // fill out user inputs fields which are not bound to the temp performance
            DP01.SelectedDate = TempPerformance.StartTime;
            EnteredStartTime = TempPerformance.StartTime.ToString("HH:mm");
            DP02.SelectedDate = TempPerformance.EndTime;
            EnteredEndTime = TempPerformance.EndTime.ToString("HH:mm");
            CB01.SelectedItem = TempPerformance.Cavent;
        }

        private void SAVEButtonClicked(object sender, RoutedEventArgs e)
        {
            bool ChecksWerePassed = PerformChecksOnUserInput();

            // if checks were passed, transfer properties of CB01 to the temp performance
            if (ChecksWerePassed)
            {
                //TempPerformance.Cavent = SelectedCavent;
                TempPerformance.CaventId = SelectedCavent.Id;
            }

            // when editing an existing event
            if (ChecksWerePassed && SelectedPerformance != null)
            {
                SelectedPerformance = TransferProperties(SelectedPerformance, TempPerformance);
                _context.Update(SelectedPerformance);
                _context.SaveChanges();
                LoadAllPerformances();
                return;
            }

            // when editing a new event
            if (ChecksWerePassed)
            {
                _context.Add(TempPerformance);
                _context.SaveChanges();
                LoadAllPerformances();
                return;
            }
        }

        private void DELEButtonClicked(object sender, RoutedEventArgs e)
        {
            if (SelectedPerformance == null)
            {
                MessageBox.Show("Please select a performance.");
                return;
            }

            MessageBoxResult answer01 = MessageBoxResult.No;
            MessageBoxResult answer02 = MessageBoxResult.No;

            answer01 = MessageBox.Show(
                "Are you sure you want to delete the selected performance?",
                "Confirm performance deletion.",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer01 == MessageBoxResult.Yes)
            {
                // check if the performance contains appearances
                List<Appearance> AppearancesBookedForPerformance = new(
                    SelectedPerformance.Appearances.ToArray()
                    );

                // if the performance contains appearances
                if (!AppearancesBookedForPerformance.IsNullOrEmpty())
                {
                    int countOfAppearances = AppearancesBookedForPerformance.Count();
                    
                    string messageToUser =
                        $"The selected performance has\n" +
                        $"{countOfAppearances} appearances booked.\n" +
                        $"Deleting this performance will delete all of its booked appearances.\n" +
                        $"Are you sure you want to delete the selected performance?";

                    // ask follow up question
                    answer02 = MessageBox.Show(messageToUser,
                        "If you do this, you will get what you deserve.",
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                }
                else
                {
                    // performance is empty and can be deleted without follow up questions asked
                    answer02 = MessageBoxResult.Yes;
                }

                if (answer01 == MessageBoxResult.Yes &&
                    answer02 == MessageBoxResult.Yes)
                {
                    // proceed with performance deletion (cascading delete in SQL is used)
                    _context.Remove(SelectedPerformance);
                    _context.SaveChanges();
                    LoadAllPerformances();
                }
            }

            ResetButtons();
        }

        private bool PerformChecksOnUserInput()
        {
            // block if any field is empty
            if (TempPerformance.Description == null ||
                DP01.SelectedDate == null ||
                EnteredStartTime == null ||
                DP02.SelectedDate == null ||
                EnteredEndTime == null ||
                CB01.SelectedItem == null)
            {
                MessageBox.Show("Please fill out all fields and ensure an event is selected.");
                return false;
            }

            // block if a non-parsable start time has been entered
            bool IsStartTimeUseable = DateTime.TryParse(EnteredStartTime, out DateTime useableStartTime);

            if (!IsStartTimeUseable)
            {
                MessageBox.Show("Please enter a useable start time, e.g. 14:02 or 2:02 PM.");
                return false;
            }

            // block if a non-parsable end time has been entered
            bool IsEndTimeUseable = DateTime.TryParse(EnteredEndTime, out DateTime useableEndTime);

            if (!IsEndTimeUseable)
            {
                MessageBox.Show("Please enter a useable end time, e.g. 14:02 or 2:02 PM.");
                return false;
            }

            // assemble StartTime
            TempPerformance.StartTime = new DateTime(
                DP01.SelectedDate.Value.Year,
                DP01.SelectedDate.Value.Month,
                DP01.SelectedDate.Value.Day,
                useableStartTime.Hour,
                useableStartTime.Minute,
                useableStartTime.Second
                );

            // assemble EndTime
            TempPerformance.EndTime = new DateTime(
                DP02.SelectedDate.Value.Year,
                DP02.SelectedDate.Value.Month,
                DP02.SelectedDate.Value.Day,
                useableEndTime.Hour,
                useableEndTime.Minute,
                useableEndTime.Second
                );

            // block if StartTime occurs after EndTime
            if (TempPerformance.StartTime >= TempPerformance.EndTime)
            {
                MessageBox.Show("Please ensure that the start time occurs before the end time.");
                return false;
            }

            // block if StartTime occurs before the start of the selected event 
            if (TempPerformance.StartTime < SelectedCavent.StartTime)
            {
                MessageBox.Show("The start time occurs before the start of the selected event. Please adjust the start time.");
                return false;
            }

            // block if EndTime occurs after the end of the selected event 
            if (TempPerformance.EndTime > SelectedCavent.EndTime)
            {
                MessageBox.Show("The end time occurs after the end of the selected event. Please adjust the end time.");
                return false;
            }

            // block if StartTime is earlier than the SQL database minimum 
            if (UserInputValidation.SQLDatabaseChecks.IsLowerEqualThanSQLDatabaseMinimum(TempPerformance.StartTime))
            {
                MessageBox.Show("Please enter a later start time.");
                return false;
            }


            // block if EndTime is earlier than the SQL database minimum 
            if (UserInputValidation.SQLDatabaseChecks.IsLowerEqualThanSQLDatabaseMinimum(TempPerformance.EndTime))
            {
                MessageBox.Show("Please enter a later start time.");
                return false;
            }


            // check for time conflicts (excluding the performance being edited, if applicable)
            // check that StartTime does not occur during an existing performance 
            Performance StartTimeConflict = _context.Performances
                .FirstOrDefault(Performance =>
                TempPerformance.StartTime >= Performance.StartTime &&
                TempPerformance.StartTime <= Performance.EndTime &&
                TempPerformance.Id != Performance.Id
                );

            if (StartTimeConflict != null)
            {
                MessageBox.Show("The start time conflicts with an existing performance. Please adjust the start time.");
                return false;
            }

            // check that EndTime does not occur during an existing performance 
            Performance EndTimeConflict = _context.Performances
                .FirstOrDefault(Performance =>
                TempPerformance.EndTime >= Performance.StartTime &&
                TempPerformance.EndTime <= Performance.EndTime &&
                TempPerformance.Id != Performance.Id
                );

            if (EndTimeConflict != null)
            {
                MessageBox.Show("The end time conflicts with an existing performance. Please adjust the end time.");
                return false;
            }

            // check that StartTime and EndTime do not occur before and after an existing performance, respectively 
            Performance EventOverlappedConflict = _context.Performances
                .FirstOrDefault(Performance =>
                TempPerformance.StartTime <= Performance.StartTime &&
                TempPerformance.EndTime >= Performance.EndTime &&
                TempPerformance.Id != Performance.Id
                );

            if (EventOverlappedConflict != null)
            {
                MessageBox.Show("The entered times encompass an existing performance. Please adjust the start and/or end times.");
                return false;
            }

            // if all checks have been passed
            return true;
        }

        private Performance TransferProperties(Performance toFill, Performance origin)
        {
            toFill.Id = origin.Id;
            toFill.Description = origin.Description;
            toFill.StartTime = origin.StartTime;
            toFill.EndTime = origin.EndTime;
            toFill.CaventId = origin.CaventId;

            //toFill.Cavent = origin.Cavent;

            return toFill;
        }
    }
}
