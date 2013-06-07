using RectanglePlacer.Biz;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace TimelineControl.Model
{
    public class TimelineGenerator
    {
        private double _borderHeight;

        private double _minPos;
        private double _maxPos;
        private double _scaleWidth = 160;

        private ICollection<TimelineAxis> AxisDataCollection;
        private TimeRangeCollection TimeRangeCollection;
        private DateTimeAndPosConverter _timePosConverter;

        public TimelineGenerator(ICollection<TimelineAxis> axis, TimeRangeCollection allies, double scaleWidth, double minPos, double maxPos)
        {
            AxisDataCollection = axis;
            TimeRangeCollection = allies;

            _minPos = minPos;
            _maxPos = maxPos;
            _scaleWidth = scaleWidth;

            _timePosConverter = new DateTimeAndPosConverter(minPos, maxPos, new TimeRange()
            {
                StartDateTime = TimeRangeCollection.First().StartDateTime,
                EndDateTime = TimeRangeCollection.Last().EndDateTime
            });
        }

        private Border VacantBorder(double width)
        {
            return new Border()
            {
                Width = width,
                Height = _borderHeight,
            };
        }

        private Border GenerateBorder(Canvas canvas, TimeBorderViewModel model, double x, double y, double width, bool isUnbound)
        {
            var border = VacantBorder(width);
            border.DataContext = model;
            if (isUnbound)
            {
                border.Style = canvas.FindResource("UnboundBorder") as Style;
            }

            Canvas.SetTop(border, y);
            Canvas.SetLeft(border, x);
            canvas.Children.Add(border);

            return border;
        }

        private void MoveToTimePosition(ref double x, ref double y, TimeRange range)
        {
            y = _timePosConverter.ConvertToPos(range.StartDateTime);
            _borderHeight = _timePosConverter.ConvertToPos(range.EndDateTime) - y;
        }

        private void MoveToNextObject(ref double x, ref double y, double width)
        {
            x += width;
        }

        public void GenerateBorders(Canvas canvas)
        {
            double currentTop = 0;
            double currentLeft = 0;

            foreach (var obj in AxisDataCollection)
            {
                if (obj.IsDisplayed == false)
                {
                    continue;
                }
                foreach (var time in TimeRangeCollection)
                {
                    var tbModel = new TimeBorderViewModel()
                    {
                        StartDateTime = time.StartDateTime,
                        EndDateTime = time.EndDateTime,
                        SourceObject = obj
                    };

                    tbModel.MyBrush = obj.DrawBrush;
                    MoveToTimePosition(ref currentLeft, ref currentTop, time);

                    GenerateBorder(canvas, tbModel, currentLeft, currentTop, obj.Width, obj.IsUnbound);                    
                }

                MoveToNextObject(ref currentLeft, ref currentTop, obj.Width);
            }
        }

        public string GetLargestText(TimeRange range)
        {
            switch (TimeRangeCollection.Kind)
            {
                case TimeRangeDivideKind.Sec30:
                    return range.StartDateTime.ToString("HH時");
                case TimeRangeDivideKind.Min1:
                case TimeRangeDivideKind.Min2:
                case TimeRangeDivideKind.Min5:
                case TimeRangeDivideKind.Min15:
                case TimeRangeDivideKind.Min30:
                    return range.StartDateTime.ToString("dd日(ddd)");
                case TimeRangeDivideKind.Hour1:
                case TimeRangeDivideKind.Hour2:
                case TimeRangeDivideKind.Hour4:
                case TimeRangeDivideKind.Hour8:
                    return range.StartDateTime.ToString("MM月");
                case TimeRangeDivideKind.Day1:
                case TimeRangeDivideKind.Day2:
                case TimeRangeDivideKind.MonthHalf:
                    return range.StartDateTime.ToString("yy年");
            }
            return "";
        }

        public string GetMiddleText(TimeRange range)
        {
            switch (TimeRangeCollection.Kind)
            {
                case TimeRangeDivideKind.Sec30:
                    return range.StartDateTime.ToString("mm分");
                case TimeRangeDivideKind.Min1:
                case TimeRangeDivideKind.Min2:
                case TimeRangeDivideKind.Min5:
                case TimeRangeDivideKind.Min15:
                case TimeRangeDivideKind.Min30:
                    return range.StartDateTime.ToString("HH時");
                case TimeRangeDivideKind.Hour1:
                case TimeRangeDivideKind.Hour2:
                case TimeRangeDivideKind.Hour4:
                case TimeRangeDivideKind.Hour8:
                    return range.StartDateTime.ToString("dd日(ddd)");
                case TimeRangeDivideKind.Day1:
                case TimeRangeDivideKind.Day2:
                case TimeRangeDivideKind.MonthHalf:
                    return range.StartDateTime.ToString("MM月");
            }
            return "";
        }


        public string GetSmallText(TimeRange range)
        {
            switch (TimeRangeCollection.Kind)
            {
                case TimeRangeDivideKind.Sec30:
                    return range.StartDateTime.ToString("ss秒");
                case TimeRangeDivideKind.Min1:
                case TimeRangeDivideKind.Min2:
                case TimeRangeDivideKind.Min5:
                case TimeRangeDivideKind.Min15:
                case TimeRangeDivideKind.Min30:
                    return range.StartDateTime.ToString("mm分");
                case TimeRangeDivideKind.Hour1:
                case TimeRangeDivideKind.Hour2:
                case TimeRangeDivideKind.Hour4:
                case TimeRangeDivideKind.Hour8:
                    return range.StartDateTime.ToString("HH時");
                case TimeRangeDivideKind.Day1:
                case TimeRangeDivideKind.Day2:
                case TimeRangeDivideKind.MonthHalf:
                    return range.StartDateTime.ToString("dd日(ddd)");
            }
            return "";
        }

        public TextBlock GetVacanteTextBlock()
        {
            return new TextBlock()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                TextAlignment = System.Windows.TextAlignment.Left,
            };
        }

        public TextBlock GetLargestTextBlock(TimeRange prevRange, TimeRange currentRange)
        {
            var largestText = GetLargestText(currentRange);
            if (prevRange == null || GetLargestText(prevRange) != largestText)
            {
                var block = GetVacanteTextBlock();
                block.FontSize = 16;
                block.Foreground = Brushes.White;
                block.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                block.Text = largestText;

                return block;
            }

            return GetVacanteTextBlock();
        }

        public TextBlock GetMiddleTextBlock(TimeRange prevRange, TimeRange currentRange)
        {
            var middleText = GetMiddleText(currentRange);
            if (prevRange == null || GetMiddleText(prevRange) != middleText)
            {
                var block = GetVacanteTextBlock();
                block.FontSize = 14;
                block.Foreground = Brushes.White;
                block.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                block.Text = middleText;

                return block;
            }

            return GetVacanteTextBlock();
        }

        public TextBlock GetSmallTextBlock(TimeRange prevRange, TimeRange currentRange)
        {
            var smallText = GetSmallText(currentRange);
            if (prevRange == null || GetSmallText(prevRange) != smallText)
            {
                var block = GetVacanteTextBlock();
                block.FontSize = 12;
                block.Foreground = Brushes.White;
                block.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                block.Text = smallText;

                return block;
            }

            return GetVacanteTextBlock();
        }

        public UIElement GenerateScaleControl(TimeRange prevRange, TimeRange currentRange)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.HorizontalAlignment = HorizontalAlignment.Right;
            panel.Children.Add(GetLargestTextBlock(prevRange, currentRange));
            panel.Children.Add(GetMiddleTextBlock(prevRange, currentRange));
            panel.Children.Add(GetSmallTextBlock(prevRange, currentRange));

            return panel;
        }

        public void GenerateScale(Canvas canvas)
        {
            double currentTop = 0;
            double currentLeft = 0;

            TimeRange prev = null;
            foreach (var time in TimeRangeCollection)
            {
                var tbModel = new TimeBorderViewModel()
                {
                    StartDateTime = time.StartDateTime,
                    EndDateTime = time.EndDateTime,
                    SourceObject = null
                };
                MoveToTimePosition(ref currentLeft, ref currentTop, time);
                var border = GenerateBorder(canvas, tbModel, currentLeft, currentTop, _scaleWidth, false);

                border.Child = GenerateScaleControl(prev, time);

                prev = time;
            }
        }

        #region event系

        public TimelineAxis GetAxis(int id)
        {
            return AxisDataCollection.Where(item => item.Id == id).First();
        }

        public double GetLeft(int id)
        {
            double currentLeft = 0.0;
            foreach (var axis in AxisDataCollection)
            {
                if (axis.IsDisplayed == false)
                {
                    continue;
                }
                if (axis.Id == id)
                {
                    return currentLeft;
                }

                currentLeft += axis.Width;
            }

            return 0.0;
        }

        public double GetTop(DateTime time)
        {
            return Math.Max(_timePosConverter.ConvertToPos(time), _minPos);
        }

        public double GetHeight(DateTime startTime, DateTime endTime)
        {
            var height = Math.Min(_timePosConverter.ConvertToPos(endTime) - GetTop(startTime), _maxPos);
            return Math.Max(height, 0.0);
        }

        public String GetOverView(EventBorderViewModel evt)
        {
            StringBuilder build = new StringBuilder();
            build.AppendLine("イベント名：" + evt.Title);
            build.AppendLine("開始日時:" + evt.Parent.StartDateTime.ToString("yyyy/MM/dd(ddd) HH:mm:ss"));
            build.AppendLine("終了日時:" + evt.Parent.EndDateTime.ToString("yyyy/MM/dd(ddd) HH:mm:ss"));
            build.AppendLine("【参加者】");

            foreach (var id in evt.Parent.Participants)
            {
                var axis = GetAxis(id);
                if (!axis.IsUnbound)
                {
                    build.AppendLine(axis.HeaderName);
                }
            }

            return build.ToString();
        }

        public Border GenerateGeneralEventBorder(Canvas canvas, Rect rect, EventBorderViewModel evt)
        {
            var brd = VacantBorder(1);
            brd.CornerRadius = new CornerRadius(2, 2, 2, 2);
            brd.DataContext = evt;
            brd.ToolTip = GetOverView(evt);

            brd.Width = rect.Width;
            brd.Height = rect.Height;

            Canvas.SetTop(brd, rect.Top);
            Canvas.SetLeft(brd, rect.Left);

            return brd;
        }

        public Border GenerateShadowEventBorder(Canvas canvas, Rect rect, EventBorderViewModel evt)
        {
            var brd = GenerateGeneralEventBorder(canvas, rect, evt);

            brd.Effect = new DropShadowEffect
            {
                Color = new Color { A = 255, R = 219, G = 219, B = 219 },
                Direction = 295,
                ShadowDepth = 5,
                Opacity = 5
            };

            return brd;
        }

        public Border AddEventBlock(Canvas canvas, Rect rect, EventBorderViewModel evt, bool isUnbound)
        {
            canvas.Children.Add(GenerateShadowEventBorder(canvas, rect, evt));

            var brd = GenerateGeneralEventBorder(canvas, rect, evt);

            canvas.Children.Add(brd);
            var grid = new Grid();

            brd.Child = grid;

            var blck = GetVacanteTextBlock();

            blck.VerticalAlignment = VerticalAlignment.Top;
            blck.HorizontalAlignment = HorizontalAlignment.Center;
            blck.Text = evt.Title;
            grid.Children.Add(blck);

            if (isUnbound)
            {
                brd.Style = canvas.FindResource("UnboundEventItemBorder") as Style;
            }
            else
            {
                TextBlock deletableTextBlock = new TextBlock();
                deletableTextBlock.Style = canvas.FindResource("DeletableTextBlock") as Style;
                brd.Style = canvas.FindResource("EventItemBorder") as Style;
                grid.Children.Add(deletableTextBlock);
            }
            
            return brd;
        }

        public void GenerateEventBorders(Canvas canvas, List<PlacableRect> placableList, double minLeft, double width, bool isUnbounded)
        {
            // ソートしておく
            placableList.Sort((x, y) =>
                {
                    if (x.rect.Y == y.rect.Y)
                    {
                        return (int)(y.rect.Height - x.rect.Height);
                    }
                    return (int)(x.rect.Y - y.rect.Y);
                });

            // EventBorderのプラスマイナスはMargin分
            RectPlacer rectPlacer = new RectPlacer(minLeft + 2, minLeft + width - 10);

            rectPlacer.Place(placableList);

            foreach (var place in placableList)
            {
                AddEventBlock(canvas, place.rect, place.source as EventBorderViewModel, isUnbounded);
            }
        }

        public void GenerateEvents(Canvas canvas, EventModelManager eventManager)
        {
            var dictionary = eventManager.GetEventModel(TimeRangeCollection.First().StartDateTime, TimeRangeCollection.Last().EndDateTime);

            foreach (var axis in AxisDataCollection)
            {
                if (axis.IsDisplayed == false)
                {
                    continue;
                }

                if (!dictionary.ContainsKey(axis.Id))
                {
                    continue;
                }

                var placableRectList = new List<PlacableRect>();

                foreach (var evModl in dictionary[axis.Id])
                {
                    var top = GetTop(evModl.StartDateTime);
                    var height = GetHeight(evModl.StartDateTime, evModl.EndDateTime.AddSeconds(-1));

                    var viewModel = new EventBorderViewModel() { Parent = evModl, MyBrush = axis.DrawBrush, Title = evModl.Title };
                    placableRectList.Add(new PlacableRect(){rect = new Rect(0, top, 0, height), source = viewModel});

                }
                GenerateEventBorders(canvas, placableRectList, GetLeft(axis.Id), axis.Width, axis.IsUnbound);
            }
        }

        #endregion
    }
}
