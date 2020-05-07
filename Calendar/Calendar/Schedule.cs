using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar
{
    class Schedule
    {
        String Title;
        String Description;
        DateTime DateStart;
        DateTime DateEnd;

        public Schedule(String title, String description, DateTime date_start, DateTime date_end)
        {
            Title = title;
            Description = description;
            DateStart = date_start;
            DateEnd = date_end;
        }

        public Boolean checkStartingDate(int year, int month, int day)
        {
            if ((DateStart.Year == year) && (DateStart.Month == month) && (DateStart.Day == day)){
                return true;
            }

            return false;
        }

        public String getTitle()
        {
            return Title;
        }

        public String getDescription()
        {
            return Description;
        }

        public DateTime getStartingDate()
        {
            return DateStart;
        }

        public DateTime getEndingDate()
        {
            return DateEnd;
        }

    }
}
