using System.Linq;
using OnDijon.Modules.Dashboard.Entities.Dto.Card;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardAlertView : CardViewBase
    {
        public CardAlertView()
        {
            InitializeComponent();
        }

        protected override void OnCardPropertyChanged(CardDto newCardDto)
        {
            base.OnCardPropertyChanged(newCardDto);

            if (newCardDto?.Tags?.Any() ?? false)
            {
                TagsContainer.Children.Clear();
                //string tags = "";
                foreach (var tag in newCardDto.Tags)
                {
                    TagsContainer.Children.Add(new BadgeView { BackgroundColor = Color.FromHex(tag.Color), Text = tag.Title });
                    //tags += tag.Title + ", ";
                }
                //tags = tags.Substring(0, tags.Length - 2);
                //TagsContainer.Children.Add(new Label { Text = tags });
            }
        }

    }
}