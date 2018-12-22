﻿#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region

using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Brushes;
using LiveCharts.Core.Drawing.Shapes;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// An <see cref="ISeries"/> that has <see cref="Stroke"/> and <see cref="Fill"/> properties.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to plot.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate required by the series.</typeparam>
    /// <typeparam name="TPointShape">The type of the point shape in the UI.</typeparam>
    /// /// <typeparam name="TBrush">The type of the brush.</typeparam>
    /// <seealso cref="Series{TModel, TCoordinate, TPointShape}" />
    /// <seealso cref="IStrokeSeries" />
    public abstract class StrokeSeries<TModel, TCoordinate, TPointShape, TBrush>
        : Series<TModel, TCoordinate, TPointShape>, IStrokeSeries
        where TCoordinate : ICoordinate
        where TPointShape : class, IShape
        where TBrush : IBrush
    {
        private TBrush _stroke;
        private float _strokeThickness;
        private TBrush _fill;
        private IEnumerable<double> _strokeDashArray;

        /// <inheritdoc />
        protected StrokeSeries()
        {
            Charting.BuildFromTheme<IStrokeSeries>(this);
        }

        /// <inheritdoc />
        public TBrush Stroke
        {
            get => _stroke;
            set
            {
                _stroke = value;
                OnPropertyChanged();
            }
        }

        IBrush IStrokeSeries.Stroke { get => _stroke; set => Stroke = (TBrush)value; }

        /// <inheritdoc />
        public float StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public IEnumerable<double> StrokeDashArray
        {
            get => _strokeDashArray;
            set
            {
                _strokeDashArray = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public TBrush Fill
        {
            get => _fill;
            set
            {
                _fill = value;
                OnPropertyChanged();
            }
        }

        IBrush IStrokeSeries.Fill { get => _fill; set => Fill = (TBrush)value; }

        /// <inheritdoc />
        public override SeriesStyle Style
        {
            get
            {
                return new SeriesStyle
                {
                    Fill = Fill,
                    Stroke = Stroke,
                    StrokeThickness = StrokeThickness,
                    StrokeDashArray = StrokeDashArray?.Select(x => (float) x)
                };
            }
        }

        /// <inheritdoc />
        protected override void SetDefaultColors(ChartModel chart)
        {
            if (!(Stroke == null || Fill == null))
            {
                return;
            }

            var nextColor = chart.GetNextColor();

            if (Stroke == null)
            {
                Stroke = (TBrush) Charting.Settings.UiProvider.GetNewSolidColorBrush(
                    nextColor.A, nextColor.R, nextColor.G, nextColor.B);
            }

            if (Fill == null)
            {
                Fill = (TBrush) Charting.Settings.UiProvider.GetNewSolidColorBrush(
                    (byte)(DefaultFillOpacity * 255), nextColor.R, nextColor.G, nextColor.B);
            }
        }
    }
}