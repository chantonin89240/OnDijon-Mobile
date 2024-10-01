using Xamarin.Forms;

namespace OnDijon.Common.Utils.Behaviors
{
    public class LetterValidator : Behavior<Entry>
    {
        //protected override void OnAttachedTo(Entry entry)
        //{
        //    entry.Unfocused += OnEntryCompleted;
        //    base.OnAttachedTo(entry);
        //}
        //protected override void OnDetachingFrom(Entry entry)
        
        //{
        //    entry.Unfocused -= OnEntryCompleted;
        //    base.OnDetachingFrom(entry);
        //}

        //void OnEntryCompleted(object sender, EventArgs args)
        //{
        //       Regex reg = new Regex("[0-9._]");
        //    bool isValid = reg.IsMatch(App.Locator.JobOfferSpontaneous.FirstName);
        //    App.Locator.JobOfferSpontaneous.FirstNameValidation = isValid;

        //}
    }
}
