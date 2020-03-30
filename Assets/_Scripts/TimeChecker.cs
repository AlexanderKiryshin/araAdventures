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
        private static Dictionary<string, int> namesCount;

        static TimeChecker()
        {
            namesAndTime=new Dictionary<string, Time>();
            namesCount=new Dictionary<string, int>();
        }

        public static void Clear()
        {
            namesAndTime=new Dictionary<string, Time>();
            namesCount = new Dictionary<string, int>();
        }

        public static void BeginCount(string name)
        {
            if (namesCount.TryGetValue(name, out var result))
            {
                int newValue = result+1;
                namesCount.Remove(name);
                namesCount.Add(name, newValue);
            }
            else
            {
                namesCount.Add(name,1);
            }
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
            for (int i = 0; i < namesCount.Count; i++)
            {
                Debug.LogError(namesCount.Keys.ToList()[i] + " " + namesCount.Values.ToList()[i]);
            }
        }
    }
}
