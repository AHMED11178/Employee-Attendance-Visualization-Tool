using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceVisualizer
{
    public class AttendanceCollection
    {
        private List<AttendanceCount> _attendances;

        public AttendanceCollection()
        {
            _attendances = new List<AttendanceCount>();
        }

        public void AddAttendance(AttendanceCount attendance)
        {
            _attendances.Add(attendance);
        }

        public void RemoveAttendance(AttendanceCount attendance)
        {
            _attendances.Remove(attendance);
        }

        public List<AttendanceCount> GetAttendances()
        {
            return _attendances;
        }
    }

}
