using NUnit.Framework;
using System;
using System.Linq;
using Xamarin.UITest.Queries;

namespace OnDijon.UITest.Utils
{
    class TestClass
    {
        /// <summary>
        /// Methode permettant de vérifier le contenu d'une popup d'information
        /// </summary>
        /// <param name="Contenu"></param> Chaine de caractère représentant le contenu attendu de la popup d'info générée
        /// <param name="app"></param> Iapp app dans toutes les classes de test
        public static void TestPopupInfo(String Contenu, Xamarin.UITest.IApp app)
        {
            app.WaitForElement("PopupInfo");
            app.Screenshot("PopupInfo");
            AppResult[] PopupViewResults = app.Query("PopupMessage");
            Assert.AreEqual(PopupViewResults[0].Text, Contenu);
        }

        /// <summary>
        /// Methode permettant de vérifier le contenu d'une popup de réussite d'une action
        /// </summary>
        /// <param name="Contenu"></param> String du contenu attendu de la popup de générée
        /// <param name="app"></param> Iapp app dans toutes les classes de test
        public static void TestPopupSuccess(String Contenu, Xamarin.UITest.IApp app)
        {
            app.WaitForElement("PopupSuccess");
            app.Screenshot("PopupSuccess");
            //Récupération du message de la popup
            AppResult[] PopupViewResults = app.Query("PopupMessage");
            //Comparaison avec le contenu rentré en paramètre
            Assert.AreEqual(PopupViewResults[0].Text, Contenu);
        }

        /// <summary>
        /// Methode permettant de vérifier le contenu et les intéractions avec une popup d'erreur
        /// </summary>
        /// <param name="Contenu"></param> Chaine de caractère représentant le contenu attendu de la popup d'erreur générée
        /// <param name="app"></param> Iapp app dans toutes les classes de test
        /// <param name="Button"></param> AutomationID du bouton entrainant l'apparition de la popup
        /// <param name="View"></param> Vue sur laquelle on se situe avant de cliquer sur le bouton 
        public static void TestPopupError(String Contenu, Xamarin.UITest.IApp app, String Button, String View)
        {
            app.WaitForElement("PopupError");
            app.Screenshot("PopupError");
            AppResult[] PopupViewResults = app.Query("PopupMessage");
            Assert.AreEqual(PopupViewResults[0].Text, Contenu);
            app.Back();
            AppResult[] PopupViewResult1 = app.WaitForElement(View);
            Assert.IsTrue(PopupViewResult1.Any());
            app.Tap(Button);
            app.WaitForElement("PopupError");
            AppResult[] PopupViewResult2 = app.Query("PopupMessage");
            Assert.AreEqual(PopupViewResult2[0].Text, Contenu);
            app.TapCoordinates(100, 100);
            AppResult[] PopupViewResult3 = app.WaitForElement(View);
            Assert.IsTrue(PopupViewResult3.Any());
            app.Tap(Button);
            app.WaitForElement("PopupError");
            AppResult[] PopupViewResult4 = app.Query("PopupMessage");
            Assert.AreEqual(PopupViewResult4[0].Text, Contenu);
            //app.Tap("PopupCancelButton");
            app.Tap(query => query.Text("Retour"));
            AppResult[] PopupViewResult5 = app.WaitForElement(View);
            Assert.IsTrue(PopupViewResult5.Any());
        }

        /// <summary>
        /// Méthode permettant de vérifier qu'un bouton n'est pas cliquable
        /// </summary>
        /// <param name="app"></param> Iapp app dans toutes les classes de test
        /// <param name="Button"></param> AutomationID du bouton que l'on souhaite tester
        public static void TestNoValidation(Xamarin.UITest.IApp app, String Button)
        {
            app.ScrollDownTo(Button);
            AppResult[] SignUpViewResults = app.WaitForElement(Button);
            Assert.IsFalse(SignUpViewResults[0].Enabled);
        }

        public static void TestErrorMessage(Xamarin.UITest.IApp app, String Contenu)
        {
            AppResult[] ErrorMessage = app.WaitForElement(c => c.Marked(Contenu));
            Assert.IsTrue(ErrorMessage.Any()); ;
        }
    }
}
