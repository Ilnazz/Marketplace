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

    private DateTime _startDate;
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged();
            Generate();
        }
    }

    private DateTime _endDate;
    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged();
            Generate();
        }
    }

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
            UnitWidth = TimeSpan.FromDays(1).Ticks, 
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
            OnPropertyChanged();
            Generate();
        }
    }

    public ReportsPageVm()
    {
        _startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        _endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        SelectedReportType = ReportType.ByOrdersCount;
    }

    private void Generate()
    {
        _dateTimePoints.Clear();

        int val = 0;
        for (var start = StartDate; start < EndDate; start = start.AddDays(1))
        {
            int year = start.Year,
                month = start.Month,
                day = start.Day;

            if (_selectedReportType == ReportType.ByOrdersCount)
                val = DatabaseContext.Entities.Orders.Local
                .Where(o => o.DateTime.Year == year && o.DateTime.Month == month && o.DateTime.Day == day).Count();

            else if (_selectedReportType == ReportType.ByTotalCost)
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

public enum ReportType
{
    ByOrdersCount,
    ByProductsCount,
    ByTotalCost
}