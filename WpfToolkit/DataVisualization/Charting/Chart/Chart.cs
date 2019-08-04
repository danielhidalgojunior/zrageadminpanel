﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Controls.DataVisualization.Charting.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace System.Windows.Controls.DataVisualization.Charting
{
    /// <summary>
    /// Represents a control that displays a Chart.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [TemplatePart(Name = Chart.ChartAreaName, Type = typeof(EdgePanel))]
    [TemplatePart(Name = Chart.LegendName, Type = typeof(Legend))]
    [TemplatePart(Name = Chart.SelectionAreaName, Type = typeof(Canvas))]
    [TemplatePart(Name = Chart.PlotAreaName, Type = typeof(Grid))]
    [StyleTypedProperty(Property = "TitleStyle", StyleTargetType = typeof(Title))]
    [StyleTypedProperty(Property = "LegendStyle", StyleTargetType = typeof(Legend))]
    [StyleTypedProperty(Property = "ChartAreaStyle", StyleTargetType = typeof(EdgePanel))]
    [StyleTypedProperty(Property = "PlotAreaStyle", StyleTargetType = typeof(Grid))]
    [ContentProperty("Series")]
    public partial class Chart : Control, ISeriesHost
    {
        /// <summary>
        /// Specifies the name of the ChartArea TemplatePart.
        /// </summary>
        private const string ChartAreaName = "ChartArea";

        /// <summary>
        /// Specifies the name of the ChartArea TemplatePart.
        /// </summary>
        private const string SelectionAreaName = "SelectionArea";

        private const string PlotAreaName = "PlotArea";

        /// <summary>
        /// Specifies the name of the legend TemplatePart.
        /// </summary>
        private const string LegendName = "Legend";

        /// <summary>
        /// Specifies the name of the legend TemplatePart.
        /// </summary>
        private const string CrosshairContainerName = "PART_CrosshairContainer";
        

        /// <summary>
        /// Gets or sets the chart area children collection.
        /// </summary>
        private AggregatedObservableCollection<UIElement> ChartAreaChildren { get; set; }

        /// <summary>
        /// An adapter that synchronizes changes to the ChartAreaChildren
        /// property to the ChartArea panel's children collection.
        /// </summary>
        private ObservableCollectionListAdapter<UIElement> _chartAreaChildrenListAdapter = new ObservableCollectionListAdapter<UIElement>();

        /// <summary>
        /// Gets or sets a collection of Axes in the Chart.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Setter is public to work around a limitation with the XAML editing tools.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value", Justification = "Setter is public to work around a limitation with the XAML editing tools.")]
        public Collection<IAxis> Axes
        {
            get
            {
                return _axes;
            }
            set
            {
                throw new NotSupportedException(Properties.Resources.Chart_Axes_SetterNotSupported);
            }
        }

        /// <summary>
        /// Stores the collection of Axes in the Chart.
        /// </summary>
        private Collection<IAxis> _axes;

        /// <summary>
        /// The collection of foreground elements.
        /// </summary>
        private ObservableCollection<UIElement> _foregroundElements = new NoResetObservableCollection<UIElement>();

        /// <summary>
        /// The collection of background elements.
        /// </summary>
        private ObservableCollection<UIElement> _backgroundElements = new NoResetObservableCollection<UIElement>();

        /// <summary>
        /// Gets the collection of foreground elements.
        /// </summary>
        ObservableCollection<UIElement> ISeriesHost.ForegroundElements { get { return ForegroundElements; } }

        /// <summary>
        /// Gets the collection of foreground elements.
        /// </summary>
        protected ObservableCollection<UIElement> ForegroundElements { get { return _foregroundElements; } }

        /// <summary>
        /// Gets the collection of background elements.
        /// </summary>
        ObservableCollection<UIElement> ISeriesHost.BackgroundElements { get { return BackgroundElements; } }

        /// <summary>
        /// Gets the collection of background elements.
        /// </summary>
        protected ObservableCollection<UIElement> BackgroundElements { get { return _backgroundElements; } }

        /// <summary>
        /// Axes arranged along the edges.
        /// </summary>
        private ObservableCollection<Axis> _edgeAxes = new NoResetObservableCollection<Axis>();

        /// <summary>
        /// Gets or sets the axes that are currently in the chart.
        /// </summary>
        private IList<IAxis> InternalActualAxes { get; set; }

        /// <summary>
        /// Gets the actual axes displayed in the chart.
        /// </summary>
        public ReadOnlyCollection<IAxis> ActualAxes { get; private set; }

        /// <summary>
        /// Gets or sets the reference to the template's ChartArea.
        /// </summary>
        private EdgePanel ChartArea { get; set; }

        /// <summary>
        /// Gets or sets the reference to the Chart's Legend.
        /// </summary>
        private Legend Legend { get; set; }

        private Canvas SelectionArea { get; set; }

        private Grid PlotArea { get; set; }

        private Grid CrosshairContainer { get; set; }
        private Grid Crosshair { get; set; }
        private Border LocationIndicator { get; set; }
        

        /// <summary>
        /// Gets or sets the collection of Series displayed by the Chart.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Setter is public to work around a limitation with the XAML editing tools.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value", Justification = "Setter is public to work around a limitation with the XAML editing tools.")]
        public Collection<ISeries> Series
        {
            get
            {
                return _series;
            }
            set
            {
                throw new NotSupportedException(Properties.Resources.Chart_Series_SetterNotSupported);
            }
        }

        /// <summary>
        /// Stores the collection of Series displayed by the Chart.
        /// </summary>
        private Collection<ISeries> _series;

        #region ItemsSource

        /// <summary>
        /// List of CLR-objects which represent series of the chart
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(Chart), new PropertyMetadata(null, (s, e) => ((Chart)s).InitSeries()));

        /// <summary>
        /// Template for an item, transforms a CLR-object to an ISeries object
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(Chart), new PropertyMetadata(null, (s, e) => ((Chart)s).InitSeries()));

        /// <summary>
        /// This property is necessary for stacked charts
        /// </summary>
        public DataTemplate ItemsHostTemplate
        {
            get { return (DataTemplate)GetValue(ItemsHostTemplateProperty); }
            set { SetValue(ItemsHostTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemsHostTemplateProperty =
            DependencyProperty.Register("ItemsHostTemplate", typeof(DataTemplate), typeof(Chart), new PropertyMetadata(null, (s, e) => ((Chart)s).InitSeries()));

        private void InitSeries()
        {
            this.Series.Clear();
            if (this.ItemsSource == null || this.ItemTemplate == null)
                return;

            //From items to series
            var series = from item in this.ItemsSource.OfType<object>()
                         let seriesItem = this.ItemTemplate.LoadContent() as ISeries
                         where seriesItem != null && seriesItem is FrameworkElement
                         let dummy = ((FrameworkElement)seriesItem).DataContext = item
                         select seriesItem;

            //Generic series and stacked series are different, that's why I use this if-else
            var hostSeries = this.ItemsHostTemplate != null ? this.ItemsHostTemplate.LoadContent() as DefinitionSeries : null;
            if (hostSeries != null)
            {
                this.Series.Add(hostSeries);
                series.OfType<SeriesDefinition>().ToList().ForEach(hostSeries.SeriesDefinitions.Add);
            }
            else series.ToList().ForEach(this.Series.Add);
        }

        #endregion

        #region public Style ChartAreaStyle
        /// <summary>
        /// Gets or sets the Style of the ISeriesHost's ChartArea.
        /// </summary>
        public Style ChartAreaStyle
        {
            get { return GetValue(ChartAreaStyleProperty) as Style; }
            set { SetValue(ChartAreaStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the ChartAreaStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty ChartAreaStyleProperty =
            DependencyProperty.Register(
                "ChartAreaStyle",
                typeof(Style),
                typeof(Chart),
                null);
        #endregion public Style ChartAreaStyle

        /// <summary>
        /// Gets the collection of legend items.
        /// </summary>
        public Collection<object> LegendItems { get; private set; }

        #region public Style LegendStyle
        /// <summary>
        /// Gets or sets the Style of the ISeriesHost's Legend.
        /// </summary>
        public Style LegendStyle
        {
            get { return GetValue(LegendStyleProperty) as Style; }
            set { SetValue(LegendStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the LegendStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendStyleProperty =
            DependencyProperty.Register(
                "LegendStyle",
                typeof(Style),
                typeof(Chart),
                null);
        #endregion public Style LegendStyle

        #region public object LegendTitle
        /// <summary>
        /// Gets or sets the Title content of the Legend.
        /// </summary>
        public object LegendTitle
        {
            get { return GetValue(LegendTitleProperty); }
            set { SetValue(LegendTitleProperty, value); }
        }

        /// <summary>
        /// Identifies the LegendTitle dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendTitleProperty =
            DependencyProperty.Register(
                "LegendTitle",
                typeof(object),
                typeof(Chart),
                null);
        #endregion public object LegendTitle

        #region public Style PlotAreaStyle
        /// <summary>
        /// Gets or sets the Style of the ISeriesHost's PlotArea.
        /// </summary>
        public Style PlotAreaStyle
        {
            get { return GetValue(PlotAreaStyleProperty) as Style; }
            set { SetValue(PlotAreaStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the PlotAreaStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty PlotAreaStyleProperty =
            DependencyProperty.Register(
                "PlotAreaStyle",
                typeof(Style),
                typeof(Chart),
                null);
        #endregion public Style PlotAreaStyle

        #region public Collection<ResourceDictionary> Palette
        /// <summary>
        /// Gets or sets a palette of ResourceDictionaries used by the children of the Chart.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Want to allow this to be set from XAML.")]
        public Collection<ResourceDictionary> Palette
        {
            get { return GetValue(PaletteProperty) as Collection<ResourceDictionary>; }
            set { SetValue(PaletteProperty, value); }
        }

        /// <summary>
        /// Identifies the Palette dependency property.
        /// </summary>
        public static readonly DependencyProperty PaletteProperty =
            DependencyProperty.Register(
                "Palette",
                typeof(Collection<ResourceDictionary>),
                typeof(Chart),
                new PropertyMetadata(OnPalettePropertyChanged));

        /// <summary>
        /// Called when the value of the Palette property is changed.
        /// </summary>
        /// <param name="d">Chart that contains the changed Palette.
        /// </param>
        /// <param name="e">Event arguments.</param>
        private static void OnPalettePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Chart source = (Chart) d;
            Collection<ResourceDictionary> newValue = (Collection<ResourceDictionary>)e.NewValue;
            source.OnPalettePropertyChanged(newValue);
        }

        /// <summary>
        /// Called when the value of the Palette property is changed.
        /// </summary>
        /// <param name="newValue">The new value for the Palette.</param>
        private void OnPalettePropertyChanged(Collection<ResourceDictionary> newValue)
        {
            ResourceDictionaryDispenser.ResourceDictionaries = newValue;
        }
        #endregion public Collection<ResourceDictionary> Palette

        public Visibility CrosshairVisibility
        {
            get { return (Visibility)GetValue(CrosshairVisibilityProperty); }
            set { SetValue(CrosshairVisibilityProperty, value); }
        }

        public static readonly DependencyProperty CrosshairVisibilityProperty =
            DependencyProperty.Register("CrosshairVisibility", typeof(Visibility), typeof(Chart), new PropertyMetadata(Visibility.Collapsed));

        
        /// <summary>
        /// Gets or sets an object that rotates through the palette.
        /// </summary>
        private ResourceDictionaryDispenser ResourceDictionaryDispenser { get; set; }

        /// <summary>
        /// Event that is invoked when the ResourceDictionaryDispenser's collection has changed.
        /// </summary>
        public event EventHandler ResourceDictionariesChanged;

        #region public object Title
        /// <summary>
        /// Gets or sets the title displayed for the Chart.
        /// </summary>
        public object Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Identifies the Title dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(object),
                typeof(Chart),
                null);
        #endregion

        #region public Style TitleStyle
        /// <summary>
        /// Gets or sets the Style of the ISeriesHost's Title.
        /// </summary>
        public Style TitleStyle
        {
            get { return GetValue(TitleStyleProperty) as Style; }
            set { SetValue(TitleStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the TitleStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleStyleProperty =
            DependencyProperty.Register(
                "TitleStyle",
                typeof(Style),
                typeof(Chart),
                null);
        #endregion public Style TitleStyle

#if !SILVERLIGHT
        /// <summary>
        /// Initializes the static members of the Chart class.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Dependency properties are initialized in-line.")]
        static Chart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Chart), new FrameworkPropertyMetadata(typeof(Chart)));
        }

#endif
        /// <summary>
        /// Initializes a new instance of the Chart class.
        /// </summary>
        public Chart()
        {
#if SILVERLIGHT
            DefaultStyleKey = typeof(Chart);
#endif
            // Create the backing collection for Series
            UniqueObservableCollection<ISeries> series = new UniqueObservableCollection<ISeries>();
            series.CollectionChanged += new NotifyCollectionChangedEventHandler(SeriesCollectionChanged);
            _series = series;

            // Create the backing collection for Axes
            UniqueObservableCollection<IAxis> axes = new UniqueObservableCollection<IAxis>();
            _axes = axes;

            ObservableCollection<IAxis> actualAxes = new SeriesHostAxesCollection(this, axes);
            actualAxes.CollectionChanged += ActualAxesCollectionChanged;
            this.InternalActualAxes = actualAxes;
            this.ActualAxes = new ReadOnlyCollection<IAxis>(InternalActualAxes);

            // Create collection for LegendItems
            LegendItems = new AggregatedObservableCollection<object>();

            ChartAreaChildren = new AggregatedObservableCollection<UIElement>();
            ChartAreaChildren.ChildCollections.Add(_edgeAxes);
            ChartAreaChildren.ChildCollections.Add(_backgroundElements);
            ChartAreaChildren.ChildCollections.Add(Series);
            ChartAreaChildren.ChildCollections.Add(_foregroundElements);

            _chartAreaChildrenListAdapter.Collection = ChartAreaChildren;

            // Create a dispenser
            ResourceDictionaryDispenser = new ResourceDictionaryDispenser();
            ResourceDictionaryDispenser.ResourceDictionariesChanged += delegate
            {
                OnResourceDictionariesChanged(EventArgs.Empty);
            };
        }

        /// <summary>
        /// Invokes the ResourceDictionariesChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        private void OnResourceDictionariesChanged(EventArgs e)
        {
            // Forward event on to listeners
            EventHandler handler = ResourceDictionariesChanged;
            if (null != handler)
            {
                handler.Invoke(this, e);
            }
        }

        /// <summary>
        /// Determines the location of an axis based on the existing axes in
        /// the chart.
        /// </summary>
        /// <param name="axis">The axis to determine the location of.</param>
        /// <returns>The location of the axis.</returns>
        private AxisLocation GetAutoAxisLocation(Axis axis)
        {
            if (axis.Orientation == AxisOrientation.X)
            {
                int numberOfTopAxes = InternalActualAxes.OfType<Axis>().Where(currentAxis => currentAxis.Location == AxisLocation.Top).Count();
                int numberOfBottomAxes = InternalActualAxes.OfType<Axis>().Where(currentAxis => currentAxis.Location == AxisLocation.Bottom).Count();
                return (numberOfBottomAxes > numberOfTopAxes) ? AxisLocation.Top : AxisLocation.Bottom;
            }
            else if (axis.Orientation == AxisOrientation.Y)
            {
                int numberOfLeftAxes = InternalActualAxes.OfType<Axis>().Where(currentAxis => currentAxis.Location == AxisLocation.Left).Count();
                int numberOfRightAxes = InternalActualAxes.OfType<Axis>().Where(currentAxis => currentAxis.Location == AxisLocation.Right).Count();
                return (numberOfLeftAxes > numberOfRightAxes) ? AxisLocation.Right : AxisLocation.Left;
            }
            else
            {
                return AxisLocation.Auto;
            }
        }

        /// <summary>
        /// Adds an axis to the ISeriesHost area.
        /// </summary>
        /// <param name="axis">The axis to add to the ISeriesHost area.</param>
        private void AddAxisToChartArea(Axis axis)
        {
            IRequireSeriesHost requiresSeriesHost = axis as IRequireSeriesHost;
            if (requiresSeriesHost != null)
            {
                requiresSeriesHost.SeriesHost = this;
            }

            if (axis.Location == AxisLocation.Auto)
            {
                axis.Location = GetAutoAxisLocation(axis);
            }

            SetEdge(axis);

            axis.LocationChanged += AxisLocationChanged;
            axis.OrientationChanged += AxisOrientationChanged;

            if (axis.Location != AxisLocation.Auto)
            {
                _edgeAxes.Add(axis);
            }
        }

        /// <summary>
        /// Rebuilds the chart area if an axis orientation is changed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Information about the event.</param>
        private void AxisOrientationChanged(object sender, RoutedPropertyChangedEventArgs<AxisOrientation> args)
        {
            Axis axis = (Axis)sender;

            axis.Location = GetAutoAxisLocation(axis);
        }

        /// <summary>
        /// Sets the Edge property of an axis based on its location and
        /// orientation.
        /// </summary>
        /// <param name="axis">The axis to set the edge property of.</param>
        private static void SetEdge(Axis axis)
        {
            switch (axis.Location)
            {
                case AxisLocation.Bottom:
                    EdgePanel.SetEdge(axis, Edge.Bottom);
                    break;
                case AxisLocation.Top:
                    EdgePanel.SetEdge(axis, Edge.Top);
                    break;
                case AxisLocation.Left:
                    EdgePanel.SetEdge(axis, Edge.Left);
                    break;
                case AxisLocation.Right:
                    EdgePanel.SetEdge(axis, Edge.Right);
                    break;
            }
        }

        /// <summary>
        /// Rebuild the chart area if an axis location is changed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Information about the event.</param>
        private void AxisLocationChanged(object sender, RoutedPropertyChangedEventArgs<AxisLocation> args)
        {
            Axis axis = (Axis)sender;

            if (args.NewValue == AxisLocation.Auto)
            {
                throw new InvalidOperationException(Properties.Resources.Chart_AxisLocationChanged_CantBeChangedToAutoWhenHostedInsideOfASeriesHost);
            }

            SetEdge(axis);

            _edgeAxes.Remove(axis);
            _edgeAxes.Add(axis);
        }

        /// <summary>
        /// Adds a series to the plot area and injects chart services.
        /// </summary>
        /// <param name="series">The series to add to the plot area.</param>
        private void AddSeriesToPlotArea(ISeries series)
        {
            series.SeriesHost = this;

            AggregatedObservableCollection<object> chartLegendItems = this.LegendItems as AggregatedObservableCollection<object>;
            int indexOfSeries = this.Series.IndexOf(series);
            chartLegendItems.ChildCollections.Insert(indexOfSeries, series.LegendItems);
        }

        /// <summary>
        /// Builds the visual tree for the Chart control when a new template
        /// is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            // Call base implementation
            base.OnApplyTemplate();

            // Unhook events from former template parts
            if (null != ChartArea)
            {
                ChartArea.Children.Clear();
            }

            if (null != Legend)
            {
                Legend.ItemsSource = null;
            }

            // Access new template parts
            ChartArea = GetTemplateChild(ChartAreaName) as EdgePanel;

            Legend = GetTemplateChild(LegendName) as Legend;

            SelectionArea = GetTemplateChild(SelectionAreaName) as Canvas;

            PlotArea = GetTemplateChild(PlotAreaName) as Grid;

            CrosshairContainer = GetTemplateChild(CrosshairContainerName) as Grid;
            Crosshair = GetTemplateChild("PART_Crosshair") as Grid;
            LocationIndicator = GetTemplateChild("PART_LocationIndicator") as Border;


            if (ChartArea != null)
            {
                _chartAreaChildrenListAdapter.TargetList = ChartArea.Children;
                _chartAreaChildrenListAdapter.Populate();
            }

            if (Legend != null)
            {
                Legend.ItemsSource = this.LegendItems;
            }

            if (SelectionArea != null)
            {
                SelectionArea.MouseLeftButtonDown += SelectionArea_MouseLeftButtonDown;
            }

            if (CrosshairContainer != null)
            {
                CrosshairContainer.MouseEnter += CrosshairContainer_MouseEnter;
                CrosshairContainer.MouseLeave += CrosshairContainer_MouseLeave;
                CrosshairContainer.MouseMove += CrosshairContainer_MouseMove;
            }
        }

        private KeyValuePair<object, object> GetPlotAreaCoordinates(Point position)
        {
            if (this.ActualAxes.Count >= 2) // && Axes[0] is IRangeAxis && Axes[1] is IRangeAxis)
            {
                object yAxisHit = null;
                object xAxisHit = null;

                if (this.ActualAxes[0].Orientation == AxisOrientation.Y)
                {
                    if (ActualAxes[0] is IRangeAxis)
                    {
                        yAxisHit = ((IRangeAxis)this.ActualAxes[0]).GetValueAtPosition(new UnitValue(PlotArea.ActualHeight - position.Y, Unit.Pixels));

                    }
                    else if (ActualAxes[0] is ICategoryAxis)
                    {
                        yAxisHit = ((ICategoryAxis)this.ActualAxes[0]).GetCategoryAtPosition(new UnitValue(/*PlotArea.ActualHeight -*/ position.Y, Unit.Pixels));
                    }
                }
                if (this.ActualAxes[0].Orientation == AxisOrientation.X)
                {
                    if (ActualAxes[0] is IRangeAxis)
                    {
                        xAxisHit = ((IRangeAxis)this.ActualAxes[0]).GetValueAtPosition(new UnitValue(position.X, Unit.Pixels));

                    }
                    else if (ActualAxes[0] is ICategoryAxis)
                    {
                        xAxisHit = ((ICategoryAxis)this.ActualAxes[0]).GetCategoryAtPosition(new UnitValue(position.X, Unit.Pixels));
                    }
                }

                if (this.ActualAxes[1].Orientation == AxisOrientation.Y)
                {
                    if (ActualAxes[1] is IRangeAxis)
                    {
                        yAxisHit = ((IRangeAxis)this.ActualAxes[1]).GetValueAtPosition(new UnitValue(PlotArea.ActualHeight - position.Y, Unit.Pixels));

                    }
                    else if (ActualAxes[1] is ICategoryAxis)
                    {
                        yAxisHit = ((ICategoryAxis)this.ActualAxes[1]).GetCategoryAtPosition(new UnitValue(/*PlotArea.ActualHeight -*/ position.Y, Unit.Pixels));
                    }
                }
                if (this.ActualAxes[1].Orientation == AxisOrientation.X)
                {
                    if (ActualAxes[1] is IRangeAxis)
                    {
                        xAxisHit = ((IRangeAxis)this.ActualAxes[1]).GetValueAtPosition(new UnitValue(position.X, Unit.Pixels));

                    }
                    else if (ActualAxes[1] is ICategoryAxis)
                    {
                        xAxisHit = ((ICategoryAxis)this.ActualAxes[1]).GetCategoryAtPosition(new UnitValue(position.X, Unit.Pixels));
                    }
                }
                
                return new KeyValuePair<object, object>(xAxisHit, yAxisHit);
            }

            return new KeyValuePair<object, object>();
        }

        void CrosshairContainer_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(PlotArea);
            var crosshairLocation = GetPlotAreaCoordinates(mousePos);

            LocationIndicator.DataContext = crosshairLocation;
            Crosshair.DataContext = mousePos;    
        }

        void CrosshairContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            SetCrossHairVisibility(false);
        }

        private void CrosshairContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            SetCrossHairVisibility(true);
        }

        private void SetCrossHairVisibility(bool visible)
        {
            LocationIndicator.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            Crosshair.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            this.Cursor = visible ? Cursors.None : Cursors.Arrow;
        }

        private Rectangle SelectionRect;
        private Point SelectionStartPoint;
        private void SelectionArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                //if (currentOrderList != null && currentOrderList.Count <= 7)
                //    return;

                SelectionStartPoint = e.GetPosition((sender as Canvas));
                
                if (SelectionRect == null)
                {
                    SelectionRect = new Rectangle();
                    SelectionArea.Children.Add(SelectionRect);
                    Canvas.SetLeft(SelectionRect, SelectionStartPoint.X);
                    Canvas.SetTop(SelectionRect, 0);
                    SelectionRect.Height = PlotArea.ActualHeight;
                    SelectionRect.Opacity = .5;
                    SelectionRect.Fill = new SolidColorBrush(Colors.LightGray);
                    SelectionRect.Stroke = new SolidColorBrush(Colors.Gray);
                    SelectionRect.StrokeThickness = 2.0;
                }

            }
            else
            {
                //LineSeries ls = this.chart1.Series[0] as LineSeries;

                //if (OrdersStack.Count <= 1)
                //{
                //    ls.ItemsSource = orders;
                //    currentOrderList = null;
                //    while (OrdersStack.Count > 0)
                //        OrdersStack.Pop();
                //    return;
                //}
                //else
                //{
                //    OrdersStack.Pop();
                //    currentOrderList = OrdersStack.Pop();
                //    ls.ItemsSource = currentOrderList;
                //}
            }

        }


        /// <summary>
        /// Ensures that ISeriesHost is in a consistent state when axes collection is
        /// changed.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        private void ActualAxesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Axis axis in e.NewItems.OfType<Axis>())
                {
                    AddAxisToChartArea(axis);
                }
            }
            if (e.OldItems != null)
            {
                foreach (Axis axis in e.OldItems.OfType<Axis>())
                {
                    RemoveAxisFromChartArea(axis);
                }
            }
        }

        /// <summary>
        /// Removes an axis from the Chart area.
        /// </summary>
        /// <param name="axis">The axis to remove from the ISeriesHost area.</param>
        private void RemoveAxisFromChartArea(Axis axis)
        {
            axis.LocationChanged -= AxisLocationChanged;
            axis.OrientationChanged -= AxisOrientationChanged;
            IRequireSeriesHost requiresSeriesHost = axis as IRequireSeriesHost;
            if (requiresSeriesHost != null)
            {
                requiresSeriesHost.SeriesHost = null;
            }

            _edgeAxes.Remove(axis);
        }

        /// <summary>
        /// Removes a series from the plot area.
        /// </summary>
        /// <param name="series">The series to remove from the plot area.
        /// </param>
        private void RemoveSeriesFromPlotArea(ISeries series)
        {
            AggregatedObservableCollection<object> legendItemsList = LegendItems as AggregatedObservableCollection<object>;
            legendItemsList.ChildCollections.Remove(series.LegendItems);

            series.SeriesHost = null;
        }

        /// <summary>
        /// Called when the ObservableCollection.CollectionChanged property
        /// changes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void SeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Clear ISeriesHost property of old Series
            if (null != e.OldItems)
            {
                foreach (ISeries series in e.OldItems)
                {
                    ISeriesHost host = series as ISeriesHost;
                    if (host != null)
                    {
                        foreach (IRequireGlobalSeriesIndex tracksGlobalIndex in host.GetDescendentSeries().OfType<IRequireGlobalSeriesIndex>())
                        {
                            tracksGlobalIndex.GlobalSeriesIndexChanged(null);
                        }
                        host.Series.CollectionChanged -= new NotifyCollectionChangedEventHandler(ChildSeriesCollectionChanged);
                    }
                    IRequireGlobalSeriesIndex require = series as IRequireGlobalSeriesIndex;
                    if (require != null)
                    {
                        require.GlobalSeriesIndexChanged(null);
                    }

                    RemoveSeriesFromPlotArea(series);
                }
            }

            // Set ISeriesHost property of new Series
            if (null != e.NewItems)
            {
                foreach (ISeries series in e.NewItems)
                {
                    ISeriesHost host = series as ISeriesHost;
                    if (null != host)
                    {
                        host.Series.CollectionChanged += new NotifyCollectionChangedEventHandler(ChildSeriesCollectionChanged);
                    }
                    AddSeriesToPlotArea(series);
                }
            }

            if (e.Action != NotifyCollectionChangedAction.Replace)
            {
                OnGlobalSeriesIndexesInvalidated(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Handles changes to the collections of child ISeries implementing ISeriesHost.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        private void ChildSeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnGlobalSeriesIndexesInvalidated(this, new RoutedEventArgs());
        }

        /// <summary>
        /// Returns a rotating enumerator of ResourceDictionary objects that coordinates
        /// with the dispenser object to ensure that no two enumerators are on the same
        /// item. If the dispenser is reset or its collection is changed then the
        /// enumerators are also reset.
        /// </summary>
        /// <param name="predicate">A predicate that returns a value indicating
        /// whether to return an item.</param>
        /// <returns>An enumerator of ResourceDictionaries.</returns>
        public IEnumerator<ResourceDictionary> GetResourceDictionariesWhere(Func<ResourceDictionary, bool> predicate)
        {
            return ResourceDictionaryDispenser.GetResourceDictionariesWhere(predicate);
        }

        /// <summary>
        /// Updates the global indexes of all descendents that require a global
        /// index.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The event data.</param>
        private void OnGlobalSeriesIndexesInvalidated(object sender, RoutedEventArgs args)
        {
            UpdateGlobalIndexes();
        }

        /// <summary>
        /// Updates the global index property of all Series that track their
        /// global index.
        /// </summary>
        private void UpdateGlobalIndexes()
        {
            (this as ISeriesHost).GetDescendentSeries().OfType<IRequireGlobalSeriesIndex>().ForEachWithIndex(
                (seriesThatTracksGlobalIndex, index) =>
                {
                    seriesThatTracksGlobalIndex.GlobalSeriesIndexChanged(index);
                });
        }

        /// <summary>
        /// Gets or sets the Series host of the chart.
        /// </summary>
        /// <remarks>This will always return null.</remarks>
        ISeriesHost IRequireSeriesHost.SeriesHost
        {
            get { return SeriesHost; }
            set { SeriesHost = value; }
        }

        /// <summary>
        /// Gets or sets the Series host of the chart.
        /// </summary>
        /// <remarks>This will always return null.</remarks>
        protected ISeriesHost SeriesHost { get; set; }

        /// <summary>
        /// Gets the axes collection of the chart.
        /// </summary>
        ObservableCollection<IAxis> ISeriesHost.Axes
        {
            get { return InternalActualAxes as ObservableCollection<IAxis>; }
        }

        /// <summary>
        /// Gets the Series collection of the chart.
        /// </summary>
        ObservableCollection<ISeries> ISeriesHost.Series
        {
            get { return (ObservableCollection<ISeries>)Series; }
        }
    }
}
