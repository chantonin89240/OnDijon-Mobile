using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomCollectionView : CollectionView
    {
        public static readonly BindableProperty RowHeightProperty = BindableProperty.Create(nameof(RowHeight), typeof(double), typeof(CustomCollectionView), propertyChanged: RowHeightPropertyChanged);
        public static readonly BindableProperty BottomMargingProperty = BindableProperty.Create(nameof(BottomMarging), typeof(double), typeof(CustomCollectionView));

        public double RowHeight
        {
            get { return (double)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        public double BottomMarging
        {
            get { return (double)GetValue(BottomMargingProperty); }
            set { SetValue(BottomMargingProperty, value); }
        }
        public CustomCollectionView()
        {
            InitializeComponent();
            StackLayoutList = new List<StackLayout>();
        }

        private List<StackLayout> StackLayoutList;

        

        private void UpdateHeight()
        {
            var futureHeight = RowHeight * StackLayoutList.Count + BottomMarging;
            if(futureHeight > 0)
            {
                HeightRequest = futureHeight;
            }
            else
            {
                HeightRequest = RowHeight;
            }
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            StackLayoutList.Add((StackLayout) child);
            UpdateHeight();
        }

        protected override void OnChildRemoved(Element child, int oldLogicalIndex)
        {
            base.OnChildRemoved(child, oldLogicalIndex);
            StackLayoutList.Remove((StackLayout)child);
            UpdateHeight();
        }


        private static void RowHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

            var view = (CustomCollectionView)bindable;
            view.UpdateHeight();
        }
    }
}