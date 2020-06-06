using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar
{
    class User
    {
        private String Username;
        private List<Schedule> ScheduleList = new List<Schedule>();

        public User(String name)
        {
            Username = name;
        }

        public void AddSchedule(Schedule schedule)
        {
            ScheduleList.Add(schedule);
        }

        public List<Schedule> GetSchedule()
        {
            return ScheduleList;
        }

        public String GetUsername()
        {
            return Username;
        }
    }
}
