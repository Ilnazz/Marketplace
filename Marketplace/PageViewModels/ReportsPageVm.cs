using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using System.Collections.ObjectModel;
using System;
using Marketplace.Database;
using System.Linq;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Marketplace.PageViewModels;

public partial class ReportsPageVm : PageVmBase                                                       
{
    private static readonly ObservableCollection<DateTimePoint> _dateTimePoints = new();

    public ISeries[] Series { get; set; } =
    {
        new ColumnSeries<DateTimePoint>
        {
            TooltipLabelFormatter = (chartPoint) =>
                $"{new DateTime((long) chartPoint.SecondaryValue):dd MMMM}: {chartPoint.PrimaryValue}",
            Values = _dateTimePoints
        }
    };

    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            Labeler = value => new DateTime((long) value).ToString("dd MMMM"),
            LabelsRotation = 80,

            // when using a date time type, let the library know your unit 
            UnitWidth = TimeSpan.FromDays(1).Ticks, 

            // if the difference between our points is in hours then we would:
            // UnitWidth = TimeSpan.FromHours(1).Ticks,

            // since all the months and years have a different number of days
            // we can use the average, it would not cause any visible error in the user interface
            // Months: TimeSpan.FromDays(30.4375).Ticks
            // Years: TimeSpan.FromDays(365.25).Ticks

            // The MinStep property forces the separator to be greater than 1 day.
            MinStep = TimeSpan.FromDays(1).Ticks
        }
    };

    public IEnumerable<ReportType> ReportTypes => Enum.GetValues(typeof(ReportType)).Cast<ReportType>();

    private ReportType _selectedReportType;
    public ReportType SelectedReportType
    {
        get => _selectedReportType;
        set
        {
            _selectedReportType = value;

            _dateTimePoints.Clear();

            int year = DateTime.Now.Year,
                month = DateTime.Now.Month,
                daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            int val = 0;
            for (int day = 1; day <= daysInMonth; day++)
            {
                if (value == ReportType.ByOrdersCount)
                    val = DatabaseContext.Entities.Orders.Local
                    .Where(o => o.DateTime.Year == year && o.DateTime.Month == month && o.DateTime.Day == day).Count();

                else if (value == ReportType.ByTotalCost)
                    val = (int)DatabaseContext.Entities.OrderProducts.Local
                    .Where(op => op.Order.DateTime.Year == year && op.Order.DateTime.Month == month && op.Order.DateTime.Day == day)
                    .Sum(op => op.Quantity * op.Cost);

                else
                    val = (int)DatabaseContext.Entities.OrderProducts.Local
                    .Where(op => op.Order.DateTime.Year == year && op.Order.DateTime.Month == month && op.Order.DateTime.Day == day)
                    .Sum(op => op.Quantity);

                _dateTimePoints.Add(new DateTimePoint(new DateTime(year, month, day), val));
            }
        }
    }

    public ReportsPageVm()
    {
        SelectedReportType = ReportType.ByOrdersCount;
    }
}

public enum ReportType
{
    ByOrdersCount,
    ByProductsCount,
    ByTotalCost
}