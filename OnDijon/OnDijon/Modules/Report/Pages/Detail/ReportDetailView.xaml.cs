using System;
using System.Collections.Generic;
using System.Linq;
using OnDijon.Modules.Report.Entities.Dto;
using OnDijon.Modules.Report.ViewModels;
using OnDijon.Common.Views;
using Xamarin.Forms;

namespace OnDijon.Modules.Report.Pages.Detail
{
    public partial class ReportDetailView : BaseView
    {
	    private ReportDetailViewModel _reportDetailVM => BindingContext as ReportDetailViewModel;

        public ReportDetailView()
        {
            InitializeComponent();
        }



        // private void PeportDetailVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        // {
        //     switch (e.PropertyName)
        //     {
        //         case nameof(ReportDetailViewModel.Report):
        //             InitializeHistory(_reportDetailVM.Report);
        //             break;
        //         default:
        //             break;
        //     }
        // }

        //  Refacto remplacé par une Bindable Layout
        // private void InitializeHistory(ReportDto report)
        // {
        //     
        //     
        //     if (!report.HistoryList?.Any() ?? false)
        //     {
        //         HistoryFrame.IsVisible = false;
        //     }
        //     else
        //     {
        //         Grid grid = new Grid();
        //         grid.ColumnSpacing = 15;
        //
        //         // Columns
        //         grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Auto) });
        //         grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //
        //         // Rows
        //         for (int i = 0; i < report.HistoryList.Count; i++)
        //         {
        //             grid.RowDefinitions.Add(new RowDefinition());
        //         }
        //
        //         for (int i = 0; i < report.HistoryList.Count; i++)
        //         {
        //             var currentElement = report.HistoryList[i];
        //             var breadcrumb = new ReportDetailBreadcrumbView { Date = currentElement.Date, IsLastElement = i == report.HistoryList.Count - 1 };
        //
        //
        //             var badge = new BadgeView { BackgroundColor = currentElement.StatusColor, Text = currentElement.Status, HorizontalOptions = LayoutOptions.Start };
        //             // TODO: A décommenter quand le destinataire sera renvoyé dans l'historique 
        //             //var deLabel = new Label { Text = $"De : {currentElement.De}", FontSize = 14, Margin = new Thickness(0, 8, 0, 0) };
        //             var descriptionLabel = new Label { Text = currentElement.Description, Style = (Style)Resources["DescriptionLabel"] };
        //
        //
        //             var stackLayout = new StackLayout
        //             {
        //                 Orientation = StackOrientation.Vertical,
        //                 HorizontalOptions = LayoutOptions.FillAndExpand,
        //                 Spacing = 0,
        //                 Padding = new Thickness(0, 0, 0, 30)
        //             };
        //
        //             stackLayout.Children.Add(badge);
        //             // TODO: A décommenter quand le destinataire sera renvoyé dans l'historique 
        //             //stackLayout.Children.Add(deLabel);
        //             stackLayout.Children.Add(descriptionLabel);
        //
        //             grid.Children.Add(breadcrumb, 0, i);
        //             grid.Children.Add(stackLayout, 1, i);
        //
        //         }
        //
        //         HistoryContainer.Children.Add(grid);
        //     }
        // }
    }
}
