using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BreadcrumbView : StackLayout
    {

        public static readonly BindableProperty CurrentStepProperty = BindableProperty.Create(nameof(CurrentStep), typeof(int), typeof(BreadcrumbView), propertyChanged: PropertyChanged);
        public static readonly BindableProperty StepCountProperty = BindableProperty.Create(nameof(StepCount), typeof(int), typeof(BreadcrumbView), propertyChanged: PropertyChanged);
        public static readonly BindableProperty StepTitleListProperty = BindableProperty.Create(nameof(StepTitleList), typeof(IList<string>), typeof(BreadcrumbView), propertyChanged: StepTitleListPropertyChanged);

        public int CurrentStep
        {
            get { return (int)GetValue(CurrentStepProperty); }
            set { SetValue(CurrentStepProperty, value); }
        }

        public int StepCount
        {
            get { return (int)GetValue(StepCountProperty); }
            set { SetValue(StepCountProperty, value); }
        }

        public IList<string> StepTitleList
        {
            get { return (IList<string>)GetValue(StepTitleListProperty); }
            set { SetValue(StepTitleListProperty, value); }
        }

        private int _internalCurrentStep = 0;



        public BreadcrumbView()
        {
            InitializeComponent();

            Initialize();
        }

        internal void Initialize()
        {
            if (CurrentStep == 0 || StepCount == 0)
                return;

            Grid grid = new Grid();
            grid.RowSpacing = 0;
            grid.ColumnSpacing = 5.5;
            grid.HorizontalOptions = LayoutOptions.FillAndExpand;

            SetInternalCurrentStepIndex();


            int nbColumn = StepCount + (StepCount - 1);

            //Row
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35, GridUnitType.Absolute) });

            //Columns
            for (int i = 0; i < nbColumn; i++)
            {
                if (i % 2 == 0)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = getGridLength(i) });
                }
                else
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                }
            }

            // Add title 
            AddTitleToGrid(grid, nbColumn);


            // Add Steps
            AddStepToGrid(grid, nbColumn);

            StackLayout.Children.Clear();
            StackLayout.Children.Add(grid);

        }


        private void AddTitleToGrid(Grid grid, int nbColumn)
        {
            if (StepTitleList != null && StepTitleList.Count > 0 && CurrentStep <= StepTitleList.Count)
            {
                int currentRow = 0;
                // Title 
                Label label = new Label() { Text = StepTitleList[CurrentStep - 1], Style = (Style)Resources["TitleLabel"] };

                // First
                if (_internalCurrentStep == 0)
                {
                    label.HorizontalOptions = LayoutOptions.Start;
                    grid.Children.Add(label, 0, currentRow);
                    Grid.SetColumnSpan(label, nbColumn);
                }
                // End
                else if (_internalCurrentStep == nbColumn - 1)
                {
                    label.HorizontalOptions = LayoutOptions.End;
                    grid.Children.Add(label, 0, currentRow);
                    Grid.SetColumnSpan(label, nbColumn);
                }
                // Between
                else
                {
                    int start = _internalCurrentStep - 1;
                    int sizeSpan = 3;
                    grid.Children.Add(label, start, currentRow);
                    Grid.SetColumnSpan(label, sizeSpan);
                }
            }
        }

        private void AddStepToGrid(Grid grid, int nbColumn)
        {
            int currentRow = 1;
            //Steps
            for (int i = 0; i < nbColumn; i++)
            {
                if (i > 0 && i % 2 == 1)
                {
                    Frame separator = new Frame { Style = (Style)Resources["BreadcrumbSeparator"] };
                    separator.BackgroundColor = (Color)App.Current.Resources[i <= _internalCurrentStep ? "GreenBreadcrumb" : "GrayBreadcrumb"];
                    grid.Children.Add(separator, i, currentRow);
                }
                else
                {
                    if (_internalCurrentStep == i)
                    {
                        grid.Children.Add(new BreadcrumbCurrentStepView(), i, currentRow);
                    }
                    else if (_internalCurrentStep > i)
                    {
                        grid.Children.Add(new BreadcrumbValidatedStepView(), i, currentRow);
                    }
                    else
                    {
                        Frame nextStep = new Frame { Style = (Style)Resources["BreadcrumbDisableStep"] };
                        grid.Children.Add(nextStep, i, currentRow);
                    }
                }
            }
        }

        private void SetInternalCurrentStepIndex()
        {
            if (CurrentStep == 1)
            {
                _internalCurrentStep = 0;
            }
            else if (CurrentStep == 2)
            {
                _internalCurrentStep = CurrentStep;
            }
            else if (CurrentStep == 3)
            {
                _internalCurrentStep = CurrentStep + 1;
            }
            else
            {
                _internalCurrentStep = CurrentStep + (int)Math.Ceiling(CurrentStep / 2.0);
            }

        }

        private GridLength getGridLength(int currentColumn)
        {
            //new GridLength(1, GridUnitType.Star)
            if (_internalCurrentStep == currentColumn || _internalCurrentStep > currentColumn)
            {
                return new GridLength(25, GridUnitType.Absolute);
            }
            else
            {
                return new GridLength(18, GridUnitType.Absolute);
            }

        }


        private static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (BreadcrumbView)bindable;
            view.Initialize();
        }

        public static void StepTitleListPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (BreadcrumbView)bindable;
            view.StepTitleList = (IList<string>)newValue;
            view.Initialize();
        }

    }
}