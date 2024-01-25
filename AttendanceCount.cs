using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceVisualizer
{
    public class AttendanceCount
    {
        private readonly List<string> _dates;
        private readonly List<int> _timeIns;
        private readonly List<int> _timeOuts;

        public AttendanceCount()
        {
            _dates = new List<string>();
            _timeIns = new List<int>();
            _timeOuts = new List<int>();
        }

        public void AddDate(string date)
        {
            _dates.Add(date);
        }

        public void AddTimeIn(int timeIn)
        {
            _timeIns.Add(timeIn);
        }

        public void AddTimeOut(int timeOut)
        {
            _timeOuts.Add(timeOut);
        }

        public string GetDate(int index)
        {
            if (index < 0 || index >= _dates.Count)
                throw new IndexOutOfRangeException("Index is out of range.");

            return _dates[index];
        }

        public int GetTimeIn(int index)
        {
            if (index < 0 || index >= _timeIns.Count)
                throw new IndexOutOfRangeException("Index is out of range.");

            return _timeIns[index];
        }

        public int GetTimeOut(int index)
        {
            if (index < 0 || index >= _timeOuts.Count)
                throw new IndexOutOfRangeException("Index is out of range.");

            return _timeOuts[index];
        }

        public List<int> CalculateTimeDifferences()
        {
            if (_timeIns.Count != _timeOuts.Count)
                throw new InvalidOperationException("TimeIns and TimeOuts lists should have the same length.");

            List<int> timeDifferences = new List<int>();

            for (int i = 0; i < _timeIns.Count; i++)
            {
                int timeDifference = _timeOuts[i] - _timeIns[i];
                timeDifferences.Add(timeDifference);
            }

            return timeDifferences;
        }
    }


}

