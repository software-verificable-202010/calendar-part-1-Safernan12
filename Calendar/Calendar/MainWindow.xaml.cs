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

namespace Calendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int CurrentMonth;
        public int CurrentYear;
        public string[] MonthsNames = { "", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
        public string[] DaysNames = { "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" };
        public MainWindow()
        {
            InitializeComponent();
            CurrentMonth = DateTime.Now.Month;
            CurrentYear = DateTime.Now.Year;
            BtnNextMonth.Click += BtnNextMonth_Click;
            BtnLastMonth.Click += BtnLastMonth_Click;
            DrawCalendar(CurrentMonth, CurrentYear);
        }

        public void DrawCalendar(int month, int year)
        {
            MonthLabel.Content = MonthsNames[month];
            YearLabel.Content = year.ToString();
            DateTime FirstDayOfMonth = new DateTime(year, month, 1);
            int FirstDay = (int)FirstDayOfMonth.DayOfWeek;
            int DaysInMonth = DateTime.DaysInMonth(year, month);
            int TotalCollumns = 7;
            int TotalRows = 6;
            int DateNumber = 1;

            CalendarGrid.Children.Clear();

            AddDaysNames();

            AddWeekendBackgrounds();

            for (int FirstRowCollumn = FirstDay; FirstRowCollumn < TotalCollumns; FirstRowCollumn++)
            {
                Label label = new Label();
                label.Content = DateNumber;
                CalendarGrid.Children.Add(label);
                Grid.SetRow(label, 1);
                Grid.SetColumn(label, FirstRowCollumn);
                
                DateNumber++;
            }

            for (int row = 2; row < TotalRows; row++)
            {
                for (int collumn = 0; collumn < TotalCollumns; collumn++)
                {
                    if (DateNumber <= DaysInMonth)
                    {
                        Label label = new Label();
                        label.Content = DateNumber;
                        CalendarGrid.Children.Add(label);
                        Grid.SetRow(label, row);
                        Grid.SetColumn(label, collumn);
                        DateNumber++;
                    }
                }
            }

        }

        private void AddWeekendBackgrounds()
        {
            int TotalRows = 6;

            for (int row = 1; row < TotalRows; row++)
            {
                Rectangle SundayRectangle = new Rectangle()
                {
                    Fill = Brushes.Azure
                };

                CalendarGrid.Children.Add(SundayRectangle);
                Grid.SetRow(SundayRectangle, row);
                Grid.SetColumn(SundayRectangle, 0);

                Rectangle SaturdayRectangle = new Rectangle()
                {
                    Fill = Brushes.Azure
                };

                CalendarGrid.Children.Add(SaturdayRectangle);
                Grid.SetRow(SaturdayRectangle, row);
                Grid.SetColumn(SaturdayRectangle, 6);
            }

        }

        private void AddDaysNames()
        {
            int TotalCollumns = 7;

            for (int DayNameCollumn = 0; DayNameCollumn < TotalCollumns; DayNameCollumn++)
            {
                Label label = new Label()
                {
                    Content = DaysNames[DayNameCollumn]
                };
                CalendarGrid.Children.Add(label);
                Grid.SetRow(label, 0);
                Grid.SetColumn(label, DayNameCollumn);

            }
        }

        private void BtnNextMonth_Click(object sender, RoutedEventArgs e)
        {
            CurrentMonth = CurrentMonth + 1;
            if (CurrentMonth == 13)
            {
                CurrentMonth = 1;
                CurrentYear = CurrentYear + 1;
            }
            DrawCalendar(CurrentMonth, CurrentYear);
        }

        private void BtnLastMonth_Click(object sender, RoutedEventArgs e)
        {
            CurrentMonth = CurrentMonth - 1;
            if (CurrentMonth == 0)
            {
                CurrentMonth = 12;
                CurrentYear = CurrentYear - 1;
            }
            DrawCalendar(CurrentMonth, CurrentYear);
        }

    }
}
