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
        private List<User> InviteeList = new List<User>();

        public Schedule(String title, String description, DateTime date_start, DateTime date_end)
        {
            Title = title;
            Description = description;
            DateStart = date_start;
            DateEnd = date_end;
        }

        public void EditSchedule(String title, String description, DateTime date_start, DateTime date_end)
        {
            Title = title;
            Description = description;
            DateStart = date_start;
            DateEnd = date_end;
        }

        public Boolean CheckStartingDate(int year, int month, int day)
        {
            if ((DateStart.Year == year) && (DateStart.Month == month) && (DateStart.Day == day)){
                return true;
            }

            return false;
        }

        public void AddInvitee(User Invitee)
        {
            InviteeList.Add(Invitee);
        }

        public List<User> GetInviteeList()
        {
            return InviteeList;
        }

        public String GetTitle()
        {
            return Title;
        }

        public String GetDescription()
        {
            return Description;
        }

        public DateTime GetStartingDate()
        {
            return DateStart;
        }

        public DateTime GetEndingDate()
        {
            return DateEnd;
        }

    }
}
