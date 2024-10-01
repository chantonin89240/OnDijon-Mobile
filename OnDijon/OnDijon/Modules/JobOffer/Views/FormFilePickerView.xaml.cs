using OnDijon.Common.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.JobOffer.Views
{
    [Obsolete("Non coforme visuellement à cause de la frame du formbaseview",false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormFilePickerView : FormBaseView
    {
        public static readonly BindableProperty FileContentProperty = BindableProperty.Create(nameof(FileContent), typeof(byte[]), typeof(FormFilePickerView), defaultBindingMode: BindingMode.TwoWay);
        public static readonly BindableProperty FileNameProperty = BindableProperty.Create(nameof(FileName), typeof(string), typeof(FormFilePickerView), propertyChanged: FileNamePropertyChanged);
        public static readonly BindableProperty CancelReinitializeProperty = BindableProperty.Create(nameof(CancelReinitialize), typeof(bool), typeof(FormFilePickerView));


        public byte[] FileContent
        {
            get { return (byte[])GetValue(FileContentProperty); }
            set { SetValue(FileContentProperty, value); }
        }

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        public bool CancelReinitialize
        {
            get { return (bool)GetValue(CancelReinitializeProperty); }
            set { SetValue(CancelReinitializeProperty, value); }
        }

        public FormFilePickerView()
        {
            InitializeComponent();
        }
        private static void FileNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormFilePickerView)bindable;
            view.SelectedFileName.Text = newValue?.ToString();
            if (newValue != null)
            {
                view.SelectedFileName.IsVisible = true;
            }
            else
            {
                view.SelectedFileName.IsVisible = true;
            }
        }

        private async void SelectFile(object sender, EventArgs e)
        {
            try
            {
                CancelReinitialize = true;
                var file = await FilePicker.PickAsync();
                if (file != null)
                {
                    //Change to PDF
                    if (file.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        FileContent = System.IO.File.ReadAllBytes(file.FullPath);
                        FileName = file.FileName;
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}