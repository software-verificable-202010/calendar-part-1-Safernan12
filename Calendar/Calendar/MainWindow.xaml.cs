using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace Calendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int CurrentMonth;
        public int CurrentYear;
        public int FocusedDay = 1;
        public string[] MonthsNames = { "", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
        public string[] DaysNames = { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo" };
        public string[] HoursNames = { "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "24:00", };
        public string CurrentMode = "MONTH";
        private List<Schedule> ScheduleList = new List<Schedule>();
        public int DaysInMonth;

        public MainWindow()
        {
            InitializeComponent();
            CurrentMonth = DateTime.Now.Month;
            CurrentYear = DateTime.Now.Year;
            BtnNextValue.Click += BtnNextValue_Click;
            BtnLastValue.Click += BtnLastValue_Click;
            DrawMonthCalendar(CurrentMonth, CurrentYear);
        }

        public void BuildRows(string type)
        {
            CalendarGrid.RowDefinitions.Clear();
            int FirstRow = 1;
            int TotalRows = 0;
            if (type == "MONTH")
            {
                TotalRows = 7;

            }
            else if(type == "WEEK")
            {
                TotalRows = 24;
            }

            for (int Row = FirstRow; Row < TotalRows; Row++)
            {
                RowDefinition gridRow = new RowDefinition();
                CalendarGrid.RowDefinitions.Add(gridRow);
            }
        }

        public void NameDaysLabels()
        {
            DayNamesGrid.Children.Clear();

            int TotalColumns = 7;
            int FirstColumn = 0;
            int DateNumber = 0;
            int LastDayofLastMonth = 0;
            int SubstractionValue = -1;
            int December = 12;
            if (CurrentMode == "WEEK")
            {
                DateNumber = GetFirstDayOfWeek(CurrentYear, CurrentMonth);
                if (DateNumber > FocusedDay)
                {
                    int LastMonth = CurrentMonth + SubstractionValue;
                    if (LastMonth == 0)
                    {
                        LastDayofLastMonth = DateTime.DaysInMonth(CurrentYear+SubstractionValue, December);
                    }
                    LastDayofLastMonth = DateTime.DaysInMonth(CurrentYear, LastMonth);
                }
            }
            for (int Column = FirstColumn; Column < TotalColumns; Column++)
            {
                Label label = new Label();
                if (CurrentMode == "MONTH")
                {
                    label.Content = DaysNames[Column];
                }
                else if(CurrentMode == "WEEK")
                {
                    label.Content = CreateWeekLabel(DateNumber, DaysNames[Column]);
                    if(DateNumber == LastDayofLastMonth)
                    {
                        DateNumber = 0;
                    }
                    DateNumber++;
                }
                DayNamesGrid.Children.Add(label);
                Grid.SetRow(label, 0);
                Grid.SetColumn(label, Column);
            }
        }

        public string CreateWeekLabel(int DayNumber, string DayName)
        {
            if (DayNumber > DaysInMonth)
            {
                return DayName;
            }
            else
            {
                return DayName + " " + DayNumber.ToString();
            }

        }

        public void DrawMonthCalendar(int month, int year)
        {
            TimeTableGrid.Visibility = Visibility.Collapsed;
            MonthLabel.Content = MonthsNames[month];
            YearLabel.Content = year.ToString();
            BtnLastValue.Content = "Last Month";
            BtnNextValue.Content = "Next Month";
            NameDaysLabels();
            int FirstDay = GetFirstDayValue(year, month);
            DaysInMonth = DateTime.DaysInMonth(year, month);
            int TotalCollumns = 7;
            int TotalRows = 7;
            int DateNumber = 1;

            CalendarGrid.Children.Clear();

            BuildRows("MONTH");

            AddWeekendBackgrounds();

            for (int FirstRowCollumn = FirstDay; FirstRowCollumn < TotalCollumns; FirstRowCollumn++)
            {

                Grid daygrid = new Grid();
                daygrid.RowDefinitions.Add(new RowDefinition());
                daygrid.RowDefinitions.Add(new RowDefinition());

                CalendarGrid.Children.Add(daygrid);
                Grid.SetRow(daygrid, 0);
                Grid.SetColumn(daygrid, FirstRowCollumn);

                Label label = new Label
                {
                    Content = DateNumber
                };
                label.MouseDoubleClick += (s, e) => BtnFocusWeek(s, e);

                daygrid.Children.Add(label);

                if (ScheduleList.Exists(x => x.checkStartingDate(CurrentYear, CurrentMonth, DateNumber))){
                    Schedule schedule = ScheduleList.Find(x => x.checkStartingDate(CurrentYear, CurrentMonth, DateNumber));
                    Border ScheduleBorder = new Border()
                    {
                        Background = Brushes.Blue,
                        Child = new TextBlock() { Text = schedule.getTitle(),
                                                  Foreground = Brushes.White}
                    };
                    daygrid.Children.Add(ScheduleBorder);
                    Grid.SetRow(ScheduleBorder, 1);
                }
                
                DateNumber++;
            }

            for (int row = 1; row < TotalRows; row++)
            {
                for (int collumn = 0; collumn < TotalCollumns; collumn++)
                {
                    if (DateNumber <= DaysInMonth)
                    {

                        Grid daygrid = new Grid();
                        daygrid.RowDefinitions.Add(new RowDefinition());
                        daygrid.RowDefinitions.Add(new RowDefinition());

                        CalendarGrid.Children.Add(daygrid);
                        Grid.SetRow(daygrid, row);
                        Grid.SetColumn(daygrid, collumn);

                        Label label = new Label();
                        label.Content = DateNumber;
                        label.MouseDoubleClick += (s, e) => BtnFocusWeek(s, e);
                        daygrid.Children.Add(label);

                        if (ScheduleList.Exists(x => x.checkStartingDate(CurrentYear, CurrentMonth, DateNumber)))
                        {
                            Schedule schedule = ScheduleList.Find(x => x.checkStartingDate(CurrentYear, CurrentMonth, DateNumber));
                            Border ScheduleBorder = new Border()
                            {
                                Background = Brushes.Blue,
                                Child = new TextBlock()
                                {
                                    Text = schedule.getTitle(),
                                    Foreground = Brushes.White
                                }
                            };
                            daygrid.Children.Add(ScheduleBorder);
                            Grid.SetRow(ScheduleBorder, 1);
                        }

                        DateNumber++;
                    }
                }
            }

        }

        private void DrawWeekCalendar(int month, int year)
        {
            TimeTableGrid.Visibility = Visibility.Visible;
            CalendarGrid.Children.Clear();
            BtnLastValue.Content = "Last Week";
            BtnNextValue.Content = "Next Week";
            BuildRows("WEEK");
            MonthLabel.Content = MonthsNames[month];
            YearLabel.Content = year.ToString();
            DaysInMonth = DateTime.DaysInMonth(year, month);
            NameDaysLabels();
            int TotalCollumns = 7;
            int FirstColumn = 0;
            int LastMonth = 12;

            int SubstractionValue = 1;
            int DateNumber = GetFirstDayOfWeek(year, month);
            int LastDayofLastMonth = 0;
            int December = 12;
            if (DateNumber > FocusedDay)
            {
                LastMonth = CurrentMonth - SubstractionValue;
                if (LastMonth == 0)
                {
                    LastMonth = December;
                    LastDayofLastMonth = DateTime.DaysInMonth(CurrentYear - SubstractionValue, December);
                }
                LastDayofLastMonth = DateTime.DaysInMonth(CurrentYear, LastMonth);
            }

            for (int column = FirstColumn; column < TotalCollumns; column++)
            {
                if (LastDayofLastMonth == 0)
                {
                    if (ScheduleList.Exists(x => x.checkStartingDate(CurrentYear, CurrentMonth, DateNumber)))
                    {
                        PaintWeekSchedule(CurrentYear, CurrentMonth, DateNumber, column);
                    }
                }
                else
                {
                    if (ScheduleList.Exists(x => x.checkStartingDate(CurrentYear, CurrentMonth, DateNumber)))
                    {
                        PaintWeekSchedule(CurrentYear+SubstractionValue, LastMonth, DateNumber, column);
                    }
                    if (LastDayofLastMonth == DateNumber)
                    {
                        DateNumber = 0;
                    }
                }
                DateNumber++;
            }


        }

        private void PaintWeekSchedule(int year, int month, int day, int column)
        {
            Schedule schedule = ScheduleList.Find(x => x.checkStartingDate(CurrentYear, CurrentMonth, day));
            int StartingHour = schedule.getStartingDate().Hour;
            int EndingHour = schedule.getEndingDate().Hour;
            if (EndingHour == 0)
            {
                EndingHour = 24;
            }

            Border ScheduleBorder = new Border()
            {
                Background = Brushes.Blue,
                Child = new TextBlock()
                {
                    Text = schedule.getTitle(),
                    Foreground = Brushes.White
                }
            };

            CalendarGrid.Children.Add(ScheduleBorder);
            Grid.SetRow(ScheduleBorder, StartingHour);
            Grid.SetColumn(ScheduleBorder, column);

            StartingHour++;

            for (int row = StartingHour; row <= EndingHour; row++)
            {
                Rectangle ScheduleRectangle = new Rectangle()
                {
                    Fill = Brushes.Blue
                };

                CalendarGrid.Children.Add(ScheduleRectangle);
                Grid.SetRow(ScheduleRectangle, row);
                Grid.SetColumn(ScheduleRectangle, column);
            }
        }


        private int GetFirstDayValue(int year, int month)
        {
            int FirstDayValue = 1;
            int DateValueCorrector = -1;
            int SundayValue = -1; 
            DateTime FirstDayOfMonth = new DateTime(year, month, FirstDayValue);
            int FirstDay = (int)FirstDayOfMonth.DayOfWeek;
            FirstDay += DateValueCorrector;
            if (FirstDay == SundayValue)
            {
                FirstDay = 6;
            }


            return FirstDay;
        }

        private int GetFirstDayOfWeek(int year, int month)
        {
            DateTime FocusedDatetime = new DateTime(year, month, FocusedDay);
            DayOfWeek firstDay = DayOfWeek.Monday;
            DateTime firstDayInWeek = FocusedDatetime.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek.Day;
        }

        private void AddWeekendBackgrounds()
        {
            int TotalRows = 6;
            int FirstRow = 0;
            int SaturdayColumn = 5;
            int SundayColumn = 6;

            for (int row = FirstRow; row < TotalRows; row++)
            {
                Rectangle SaturdayRectangle = new Rectangle()
                {
                    Fill = Brushes.Azure
                };

                CalendarGrid.Children.Add(SaturdayRectangle);
                Grid.SetRow(SaturdayRectangle, row);
                Grid.SetColumn(SaturdayRectangle, SaturdayColumn);

                Rectangle SundayRectangle = new Rectangle()
                {
                    Fill = Brushes.Azure
                };

                CalendarGrid.Children.Add(SundayRectangle);
                Grid.SetRow(SundayRectangle, row);
                Grid.SetColumn(SundayRectangle, SundayColumn);
            }

        }

        private void BtnNextValue_Click(object sender, RoutedEventArgs e)
        {
            int DateAdditionValue = 1;
            int WeekValue = 7;
            if (CurrentMode == "MONTH")
            {
                CurrentMonth = CurrentMonth + DateAdditionValue;
                if (CurrentMonth == 13)
                {
                    CurrentMonth = 1;
                    CurrentYear = CurrentYear + DateAdditionValue;
                }
                DrawMonthCalendar(CurrentMonth, CurrentYear);
            }
            else if (CurrentMode == "WEEK")
            {
                FocusedDay = FocusedDay + WeekValue;
                if (FocusedDay >= DaysInMonth)
                {
                    CurrentMonth = CurrentMonth + DateAdditionValue;
                    if (CurrentMonth == 13)
                    {
                        CurrentMonth = 1;
                        CurrentYear = CurrentYear + DateAdditionValue;
                    }
                    FocusedDay = CalculateFocusedDay(FocusedDay, CurrentMonth);
                }
                DrawWeekCalendar(CurrentMonth, CurrentYear);
            }
        }

        private void BtnLastValue_Click(object sender, RoutedEventArgs e)
        {
            int DateSubstractionValue = -1;
            int WeekValue = -7;
            if (CurrentMode == "MONTH")
            {
                CurrentMonth = CurrentMonth + DateSubstractionValue;
                if (CurrentMonth == 0)
                {
                    CurrentMonth = 12;
                    CurrentYear = CurrentYear + DateSubstractionValue;
                }
                DrawMonthCalendar(CurrentMonth, CurrentYear);
            }
            else if (CurrentMode == "WEEK")
            {
                FocusedDay = FocusedDay + WeekValue;
                if (FocusedDay <= 0)
                {
                    CurrentMonth = CurrentMonth + DateSubstractionValue;
                    if (CurrentMonth == 0)
                    {
                        CurrentMonth = 12;
                        CurrentYear = CurrentYear + DateSubstractionValue;
                    }
                    FocusedDay = CalculateFocusedDay(FocusedDay, CurrentMonth);
                }
                DrawWeekCalendar(CurrentMonth, CurrentYear);
            }
        }

        private int CalculateFocusedDay(int FocusedDay, int Month)
        {
            int DaysInNewMonth = DateTime.DaysInMonth(CurrentYear, Month);
            int NewFocusedDay = 0;
            if (FocusedDay <= 0)
            {
                NewFocusedDay = DaysInNewMonth + FocusedDay;
            }
            else
            {
                NewFocusedDay = FocusedDay - DaysInMonth;
            }

            return NewFocusedDay;
        }

        private void BtnFocusWeek(object sender, MouseButtonEventArgs e)
        {
            FocusedDay = (int)(sender as Label).Content;
            CurrentMode = "WEEK";
            DrawWeekCalendar(CurrentMonth, CurrentYear);
        }

        private void BtnFocusMonth(object sender, MouseButtonEventArgs e)
        {
            CurrentMode = "MONTH";
            DrawMonthCalendar(CurrentMonth, CurrentYear);
        }

        private void BtnNewSchedule_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Collapsed;
            ScheduleFormGrid.Visibility = Visibility.Visible;
        }

        private void BtnCreateNewSchedule_Click(object sender, RoutedEventArgs e)
        {
            String title = ScheduleTitleTextBox.Text;
            String description = ScheduleDescriptionTextBox.Text;
            String starting_time = StartingHourComboBox.Text;
            String ending_time = EndingHourComboBox.Text;
            if ((title == "") || (starting_time == "") || (ending_time == "") || !(DateStartInput.SelectedDate.HasValue))
            {
                MessageBox.Show("You have to fill all inputs", "Error");
            }
            else
            {
                Double starting_hour = Convert.ToDouble(starting_time.Substring(0, 2));
                Double ending_hour = Convert.ToDouble(ending_time.Substring(0, 2));
                DateTime date_start = (DateTime)DateStartInput.SelectedDate;
                DateTime date_end = (DateTime)DateStartInput.SelectedDate;
                date_start = date_start.AddHours(starting_hour);
                date_end = date_end.AddHours(ending_hour);
                ScheduleList.Add(new Schedule(title, description, date_start, date_end));
                MainGrid.Visibility = Visibility.Visible;
                ScheduleFormGrid.Visibility = Visibility.Collapsed;
                if (CurrentMode == "MONTH")
                {
                    DrawMonthCalendar(CurrentMonth, CurrentYear);
                }
                else if (CurrentMode == "WEEK")
                {
                    DrawWeekCalendar(CurrentMonth, CurrentYear);
                }

            }
        }

        private void CancelSchedule_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Visible;
            ScheduleFormGrid.Visibility = Visibility.Collapsed;
        }

        private void StartingHourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EndingHourComboBox.Items.Clear();
            int StartingComboBoxIndex = StartingHourComboBox.SelectedIndex;

            for (int ComboBoxIndex = StartingComboBoxIndex; ComboBoxIndex < HoursNames.Length; ComboBoxIndex++)
            {
                EndingHourComboBox.Items.Add(HoursNames[ComboBoxIndex]);
            }
        }
    }
}
