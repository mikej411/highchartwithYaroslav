using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Definitions for the datasource of a Chart element
    /// </summary>
    public class RandomChartRootObject
    {
        public static string xAxis { get; set; }
        public static string yAxis { get; set; }
        public static string valueMemberPath { get; set; }
        public static bool isTransitionInEnabled { get; set; }
        public static bool isHighlightingEnabled { get; set; }
        public static bool showTooltip { get; set; }
        public static string tooltipTemplate { get; set; }
        public static string name { get; set; }
        public static string title { get; set; }
        // public List<RandomChartDataSource> dataSource { get; set; }
        public static string type { get; set; }
    }

    /// <summary>
    /// Definitions for the datasource of a Chart element
    /// </summary>
    public class RandomChartDataSource
    {
        public static string category { get; set; }
        public static int count { get; set; }
    }

    /// <summary>
    /// Definitions for the datasource of a Chart element
    /// </summary>
    public class CountryChartRootObject
    {
        public int Year { get; set; }
        public double Canada { get; set; }
        public double SaudiArabia { get; set; }
        public double Russia { get; set; }
        public double UnitedStates { get; set; }
        public double China { get; set; }
        //public string Total { get; set; }
    }

    /// <summary>
    /// Definitions for handling returned API data and for local deserialization.
    /// </summary>
    public class Person
    {
        public static string Name { get; set; }
        public static int Age { get; set; }
        public DateTime BirthDate { get; set; }
    }
}


