using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Model
{
    public class Time
    {
        public TimeSpan totalTime;
        public DateTime lastSession;

        public Time(TimeSpan totalTime, DateTime lastSession)
        {
            this.totalTime = totalTime;
            this.lastSession = lastSession;
        }
    }
}
