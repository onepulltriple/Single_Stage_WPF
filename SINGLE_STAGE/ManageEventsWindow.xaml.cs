﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SINGLE_STAGE.Entities;
using SINGLE_STAGE.CRUD_logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace SINGLE_STAGE
{
    /// <summary>
    /// Interaction logic for ManageEventsWindow.xaml
    /// </summary>
    public partial class ManageEventsWindow : Window, INotifyPropertyChanged
    {
        #region Class Members

        readonly SingleStageContext _context;

        public event PropertyChangedEventHandler PropertyChanged;

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

        private Cavent _tempCavent;
        public Cavent TempCavent
        {
            get { return _tempCavent; }
            set
            {
                _tempCavent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TempCavent)));
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

        private List<Ticket> _listOfTickets;
        public List<Ticket> ListOfTickets
        {
            get { return _listOfTickets; }
            set
            {
                _listOfTickets = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListOfTickets)));
            }
        }

        #endregion

        public ManageEventsWindow()
        {
            InitializeComponent();
            DataContext = this;
            _context = new();

            LoadAllCavents();
            LoadAllPerformances();
            LoadAllTickets();
        }

        private void LoadAllCavents()
        {
            ListOfCavents = new(_context.Cavents
                .OrderBy(cavent => cavent.StartTime)
                .Include(cavent => cavent.Performances)
                .Include(cavent => cavent.Tickets)
                .ToArray()
                );

            ResetButtons();

            CloseUserInputFields();
        }

        private void LoadAllPerformances()
        {
            ListOfPerformances = new(_context.Performances
                .OrderBy(performance => performance.StartTime)
                .Include(performance => performance.Appearances)
                .ToArray()
                );
        }

        private void LoadAllTickets()
        {
            ListOfTickets = new(_context.Tickets
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
            UI12.IsEnabled = false;
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
            UI11.IsEnabled = true;
            UI12.IsEnabled = true;
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

            TempCavent = new();

            // clear user inputs fields which are not bound to the temp event
            DP01.SelectedDate = DateTime.Today;
            EnteredStartTime = null;
            DP02.SelectedDate = DateTime.Today;
            EnteredEndTime = null;
        }

        private void EDITButtonClicked(object sender, RoutedEventArgs e)
        {
            if (SelectedCavent == null)
            {
                MessageBox.Show("Please select an event.");
                return;
            }

            ButtonsInEditMode();
            OpenUserInputFields();

            TempCavent = TransferProperties(new Cavent(), SelectedCavent);

            // fill out user inputs fields which are not bound to the temp event
            DP01.SelectedDate = TempCavent.StartTime;
            EnteredStartTime = TempCavent.StartTime.ToString("HH:mm");
            DP02.SelectedDate = TempCavent.EndTime;
            EnteredEndTime = TempCavent.EndTime.ToString("HH:mm");
        }

        private void SAVEButtonClicked(object sender, RoutedEventArgs e)
        {
            bool ChecksWerePassed = PerformChecksOnUserInput();

            // when editing an existing event
            if (ChecksWerePassed && SelectedCavent != null)
            {
                SelectedCavent = TransferProperties(SelectedCavent, TempCavent);
                _context.Update(SelectedCavent);
                _context.SaveChanges();
                LoadAllCavents();
                return;
            }

            // when editing a new event
            if (ChecksWerePassed)
            {
                _context.Add(TempCavent);
                _context.SaveChanges();
                LoadAllCavents();
                return;
            }
        }

        private void DELEButtonClicked(object sender, RoutedEventArgs e)
        {
            if (SelectedCavent == null)
            {
                MessageBox.Show("Please select an event.");
                return;
            }

            MessageBoxResult answer01 = MessageBoxResult.No;
            MessageBoxResult answer02 = MessageBoxResult.No;

            answer01 = MessageBox.Show(
                "Are you sure you want to delete the selected event?", 
                "Confirm event deletion.", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (answer01 == MessageBoxResult.Yes)
            {
                // check if the event contains performances
                List<Performance> PerformancesScheduledForCavent = new(
                    SelectedCavent.Performances.ToArray()
                    );

                // check if the event contains tickets
                List<Ticket> TicketsBookedForCavent = new(
                    SelectedCavent.Tickets.ToArray()
                    );

                // if the event contains performances            - or -
                // if the event has tickets already booked
                if (!PerformancesScheduledForCavent.IsNullOrEmpty() ||
                    !TicketsBookedForCavent.IsNullOrEmpty())
                {
                    int countOfPerformances = PerformancesScheduledForCavent.Count();
                    int countOfTickets = TicketsBookedForCavent.Count();

                    string messageToUser = 
                        $"The selected event has\n" +
                        $"{countOfPerformances} performances scheduled and\n" +
                        $"{countOfTickets} tickets booked.\n" +
                        $"Deleting this event will delete all of its scheduled performances " +
                        $"and all of its booked tickets.\n" +
                        $"Are you sure you want to delete the selected event?";

                    // ask follow up question
                    answer02 = MessageBox.Show(messageToUser, 
                        "If you do this, you will get what you deserve.", 
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                }
                else
                {
                    // event is empty and can be deleted without follow up questions asked
                    answer02 = MessageBoxResult.Yes;
                }

                if (answer01 == MessageBoxResult.Yes &&
                    answer02 == MessageBoxResult.Yes)
                {
                    // proceed with event deletion (cascading delete in SQL is used)
                    _context.Remove(SelectedCavent);
                    _context.SaveChanges();
                    LoadAllCavents();
                }
            }

            ResetButtons();
        }

        private bool PerformChecksOnUserInput()
        {
            // block if any field is empty
            if (TempCavent.Name == null ||
                DP01.SelectedDate == null ||
                EnteredStartTime == null ||
                DP02.SelectedDate == null ||
                EnteredEndTime == null ||
                TempCavent.TicketPrice == null)
            {
                MessageBox.Show("Please fill out all fields and make sure the ticket price is a decimal number.");
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
            TempCavent.StartTime = new DateTime(
                DP01.SelectedDate.Value.Year,
                DP01.SelectedDate.Value.Month,
                DP01.SelectedDate.Value.Day,
                useableStartTime.Hour,
                useableStartTime.Minute,
                useableStartTime.Second
                );

            // assemble EndTime
            TempCavent.EndTime = new DateTime(
                DP02.SelectedDate.Value.Year,
                DP02.SelectedDate.Value.Month,
                DP02.SelectedDate.Value.Day,
                useableEndTime.Hour,
                useableEndTime.Minute,
                useableEndTime.Second
                );

            // block if StartTime occurs after EndTime
            if (TempCavent.StartTime >= TempCavent.EndTime)
            {
                MessageBox.Show("Please ensure that the start time occurs before the end time.");
                return false;
            }


            // block if StartTime is earlier than the SQL database minimum 
            if (UserInputValidation.SQLDatabaseChecks.IsLowerEqualThanSQLDatabaseMinimum(TempCavent.StartTime))
            {
                MessageBox.Show("Please enter a later start time.");
                return false;
            }


            // block if EndTime is earlier than the SQL database minimum 
            if (UserInputValidation.SQLDatabaseChecks.IsLowerEqualThanSQLDatabaseMinimum(TempCavent.EndTime))
            {
                MessageBox.Show("Please enter a later start time.");
                return false;
            }


            // check for time conflicts (excluding the event being edited, if applicable)
            // check that StartTime does not occur during an existing event 
            Cavent StartTimeConflict = _context.Cavents
                .FirstOrDefault(cavent =>
                TempCavent.StartTime >= cavent.StartTime &&
                TempCavent.StartTime <= cavent.EndTime &&
                TempCavent.Id != cavent.Id
                );

            if (StartTimeConflict != null)
            {
                MessageBox.Show("The start time conflicts with an existing event. Please adjust the start time.");
                return false;
            }

            // check that EndTime does not occur during an existing event 
            Cavent EndTimeConflict = _context.Cavents
                .FirstOrDefault(cavent =>
                TempCavent.EndTime >= cavent.StartTime &&
                TempCavent.EndTime <= cavent.EndTime &&
                TempCavent.Id != cavent.Id
                );

            if (EndTimeConflict != null)
            {
                MessageBox.Show("The end time conflicts with an existing event. Please adjust the end time.");
                return false;
            }

            // check that StartTime and EndTime do not occur before and after an existing event, respectively 
            Cavent EventOverlappedConflict = _context.Cavents
                .FirstOrDefault(cavent =>
                TempCavent.StartTime <= cavent.StartTime &&
                TempCavent.EndTime >= cavent.EndTime &&
                TempCavent.Id != cavent.Id
                );

            if (EventOverlappedConflict != null)
            {
                MessageBox.Show("The entered times encompass an existing event. Please adjust the start and/or end times.");
                return false;
            }

            // if all checks have been passed
            return true;
        }

        private Cavent TransferProperties(Cavent toFill, Cavent origin)
        {
            toFill.Id = origin.Id;
            toFill.Name = origin.Name;
            toFill.StartTime = origin.StartTime;
            toFill.EndTime = origin.EndTime;
            toFill.TicketPrice = origin.TicketPrice;
            toFill.SoldOut = origin.SoldOut;

            return toFill;
        }
    }
}
