using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.Utils
{
    class FastAccess
    {
        public static string Style;

        public static void Onboarding(Xamarin.UITest.IApp app)
        {
            //On s'assure d'être sur la bonne page
            app.WaitForElement("LoginPage1");

            double swipePercentage = 0.99;
            //On coulisse de droite à gauche
            app.SwipeRightToLeft(swipePercentage);
            //On s'assure d'être sur la 2ème page d'OnBoarding
            app.WaitForElement("LoginPage2");

            app.Screenshot("LoginPage2");
            //On coulisse une nouvelle fois
            app.SwipeRightToLeft(swipePercentage);
            //On s'assure d'être sur la page de connexion
            app.WaitForElement("LoginPage3");

            app.Screenshot("LoginPage3");
        }

        public static void ForgetPassword(Xamarin.UITest.IApp app)
        {
            //Appel à la méthode précédente pour accéder à la page de connexion
            FastAccess.Onboarding(app);
            //Clic sur le bouton de Mot de passe oublié sur l'écran de cnnexion
            app.Tap("ForgetPasswordButton");
        }

        public static void Login(Xamarin.UITest.IApp app)
        {
            FastAccess.Onboarding(app);

            app.EnterText("EmailEntry", "testondijon@gmail.com");

            app.EnterText("PasswordEntry", "OnDijon2020");

            app.Tap("LoginButton");

            app.Screenshot("Dashboard");
        }

        public static void Profile(Xamarin.UITest.IApp app) 
        {
            FastAccess.Login(app);

            app.Tap("ProfileButton");

            app.Screenshot("Profile");
        }

        public static void UpdateProfile(Xamarin.UITest.IApp app)
        {
            FastAccess.Profile(app);

            app.Tap("UpdateProfile");

            app.Screenshot("UpdateProfile");
        }

        public static void Signalement(Xamarin.UITest.IApp app)
        {
            FastAccess.Login(app);

            app.Tap(c => c.Marked("Signalements"));

            app.Screenshot("Reporting");
        }

        public static void FollowReport(Xamarin.UITest.IApp app)
        {
            FastAccess.Signalement(app);

            app.Tap("FollowReport");

            app.Screenshot("FollowReport");
        }

        public static void ReportSignalement(Xamarin.UITest.IApp app)
        {
            FastAccess.Signalement(app);

            app.Tap("ReportEvent");

            app.Screenshot("Reporting2");
        }

        public static void TypeSignalement(Xamarin.UITest.IApp app)
        {
            FastAccess.ReportSignalement(app);

            app.Screenshot("ReportEvent");
        }

        public static void Localisation(Xamarin.UITest.IApp app)
        {
            FastAccess.TypeSignalement(app);

            app.Tap(c => c.Marked("Continuer"));

            app.WaitForElement("Type0");

            app.Tap("Type0");

            AppResult[] LabelResult = app.WaitForElement("Label0");

            Style = LabelResult[0].Text;

            app.Tap("Suivant");

            app.Screenshot("LocationEvent");
        }

        public static void Description(Xamarin.UITest.IApp app)
        {
            FastAccess.Localisation(app);

            AppResult[] Map = app.WaitForElement("Map");

            app.TapCoordinates(Map[0].Rect.CenterX, Map[0].Rect.CenterY);

            app.WaitForElement(query => query.Text("40 Avenue du Drapeau, 21000, Dijon"));

            app.Tap("Suivant");

            app.Screenshot("DescriptionEvent");
        }

        public static void Validation(Xamarin.UITest.IApp app)
        {
            FastAccess.Description(app);

            app.EnterText("DescriptionText", "Description Test");

            app.DismissKeyboard();

            app.Tap("Suivant");

            app.Screenshot("ValidationEvent");
        }

        public static void SignUp(Xamarin.UITest.IApp app)
        {
            FastAccess.Onboarding(app);

            app.Tap("SignUpButton");

            app.Screenshot("SignUp");

            app.EnterText("Name", "Test");

            app.EnterText("Firstname", "Test");

            app.EnterText("Email", "testnodijon@gmail.com");

            app.EnterText("Phone", "0606060606");

            app.DismissKeyboard();

            app.ScrollDownTo("PasswordConfirmation");

            app.Tap("GenderMale");

            app.EnterText("Password", "OnDijon2020");

            app.EnterText("PasswordConfirmation", "OnDijon2020");

            app.DismissKeyboard();

            app.ScrollDownTo("Validate");

            app.DismissKeyboard();

            app.Tap("Data");

            app.Tap("CGU");
        }
    }
}
