using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Time = Assets._Scripts.Model.Time;

namespace Assets._Scripts
{
    public class TimeChecker
    {
        private static Dictionary<string, Time> namesAndTime;

        static TimeChecker()
        {
            namesAndTime=new Dictionary<string, Time>();
        }

        public static void Clear()
        {
            namesAndTime=new Dictionary<string, Time>();
        }
        public static void BeginMeasurement(string name)
        {
            if (namesAndTime.TryGetValue(name, out var result))
            {
                Time time=new Time(result.totalTime, System.DateTime.Now);
                namesAndTime.Remove(name);
                namesAndTime.Add(name,time);
            }
            else
            {
                namesAndTime.Add(name,new Time(new TimeSpan(), System.DateTime.Now));
            }
            
        }
        public static void EndMeasurement(string name)
        {
            if (namesAndTime.TryGetValue(name, out var result))
            {

                TimeSpan span = System.DateTime.Now - result.lastSession;
                result.totalTime=result.totalTime.Add(span);

                /*DateTime newDate = new DateTime(result.totalTime.Year+ System.DateTime.Now.Year - result.lastSession.Year, 
                    result.totalTime.Month + System.DateTime.Now.Month - result.lastSession.Month, 
                    result.totalTime.Day + System.DateTime.Now.Day - result.lastSession.Day,
                    result.totalTime.Hour + System.DateTime.Now.Hour - result.lastSession.Hour, result.totalTime.Minute
                    + System.DateTime.Now.Minute - result.lastSession.Minute,
                    result.totalTime.Millisecond + System.DateTime.Now.Millisecond - result.lastSession.Millisecond);*/
                Time time = new Time(result.totalTime, System.DateTime.Now);
                // Debug.LogError(name +" "+(System.DateTime.Now - result));
              //  namesAndTime.Remove(name);
            }
            else
            {
                Debug.LogError(name + " not found");
            }          
        }

        public static void PrintResult()
        {
            for (int i = 0; i < namesAndTime.Count; i++)
            {
                Debug.LogError(namesAndTime.Keys.ToList()[i]+" "+namesAndTime.Values.ToList()[i].totalTime);
            }
        }
    }
}
