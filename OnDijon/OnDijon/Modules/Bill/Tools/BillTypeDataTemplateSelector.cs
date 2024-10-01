using OnDijon.Modules.Bill.Entities.Models;
using System;
using Xamarin.Forms;

namespace OnDijon.Modules.Bill.Tools
{
    public class BillTypeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PaidBill { get; set; }
        public DataTemplate ToPayBill { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            BillModel bill = (BillModel)item;

            if (bill == null)
            {
                throw new NotSupportedException("item isn't a BillDto !");
            }

            switch (bill.State.ToLower())
            {
                case "facture à payer":
                    return ToPayBill;
                case "facture payée":
                    return PaidBill;
                default:
                    throw new NotSupportedException("No DataTemplate associated with this type");
            }
        }
    }
}
