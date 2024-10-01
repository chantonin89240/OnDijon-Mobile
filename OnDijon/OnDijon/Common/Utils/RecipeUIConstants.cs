using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OnDijon.Common.Utils
{
    public class RecipeUIConstants
    {
        public static ReadOnlyCollection<string> StepTitleReport = new ReadOnlyCollection<string>
            (new List<string> {
                "Type de signalement",
                "Lieu du signalement",
                "Détails du signalement",
                "Récapitulatif"
            });

        public static ReadOnlyCollection<string> stepTitleBooking = new ReadOnlyCollection<string>(new List<string>() { 
            "Titre d'identité demandé",
            "Formulaire", 
            "Votre rendez-vous" });

        public static ReadOnlyCollection<string> stepTitleJobOffer = new ReadOnlyCollection<string>(new List<string>()
        {
            "Choix du lieu de candidature",
            "Type d'emploi ciblé",
            "Formulaire"
        });

        public static string GenderMale = "Monsieur";
        public static string GenderFemale = "Madame";
        public static string[] GenderChoices = new[] { GenderFemale, GenderMale };

        public static string IdentityCard = "Carte d'identité";
        public static string Passeport = "Passeport";
        public static string[] IdentityDocumentChoices = new[] { IdentityCard, Passeport };

        public static string Dijon = "Dijon";
        public static string Quetigny = "Quetigny";
        public static string[] CityChoices = new[] { Dijon, Quetigny };


        public static string AvatarNeutralSource = "OnDijon.Assets.avatars.avatar-neutral.svg";

        public static class BoyAvatarSourceList
        {
            public const string Boy1 = "OnDijon.Assets.avatars.avatar-boy-1.svg";
            public const string Boy2 = "OnDijon.Assets.avatars.avatar-boy-2.svg";
            public const string Boy3 = "OnDijon.Assets.avatars.avatar-boy-3.svg";
            public const string Boy4 = "OnDijon.Assets.avatars.avatar-boy-4.svg";
            public const string Boy5 = "OnDijon.Assets.avatars.avatar-boy-5.svg";
            public const string Boy6 = "OnDijon.Assets.avatars.avatar-boy-6.svg";
            public const string Boy7 = "OnDijon.Assets.avatars.avatar-boy-7.svg";
            public const string Boy8 = "OnDijon.Assets.avatars.avatar-boy-8.svg";
            public const string Boy9 = "OnDijon.Assets.avatars.avatar-boy-9.svg";
            public const string Boy10 = "OnDijon.Assets.avatars.avatar-boy-10.svg";

            public static string[] All = new[] { Boy1, Boy2, Boy3, Boy4, Boy5, Boy6, Boy7, Boy8, Boy9, Boy10 };
        }

        public static class GirlAvatarSourceList
        {
            public const string Girl1 = "OnDijon.Assets.avatars.avatar-girl-1.svg";
            public const string Girl2 = "OnDijon.Assets.avatars.avatar-girl-2.svg";
            public const string Girl3 = "OnDijon.Assets.avatars.avatar-girl-3.svg";
            public const string Girl4 = "OnDijon.Assets.avatars.avatar-girl-4.svg";
            public const string Girl5 = "OnDijon.Assets.avatars.avatar-girl-5.svg";
            public const string Girl6 = "OnDijon.Assets.avatars.avatar-girl-6.svg";
            public const string Girl7 = "OnDijon.Assets.avatars.avatar-girl-7.svg";
            public const string Girl8 = "OnDijon.Assets.avatars.avatar-girl-8.svg";
            public const string Girl9 = "OnDijon.Assets.avatars.avatar-girl-9.svg";
            public const string Girl10 = "OnDijon.Assets.avatars.avatar-girl-10.svg";

            public static string[] All = new[] { Girl1, Girl2, Girl3, Girl4, Girl5, Girl6, Girl7, Girl8, Girl9, Girl10 };

        }

        public static class KidBoyAvatarSourceList
        {
            public const string Boy1 = "OnDijon.Assets.avatars.kids.avatar-boy-1.svg";
            public const string Boy2 = "OnDijon.Assets.avatars.kids.avatar-boy-2.svg";
            public const string Boy3 = "OnDijon.Assets.avatars.kids.avatar-boy-3.svg";
            public const string Boy4 = "OnDijon.Assets.avatars.kids.avatar-boy-4.svg";
            public const string Boy5 = "OnDijon.Assets.avatars.kids.avatar-boy-5.svg";
            public const string Boy6 = "OnDijon.Assets.avatars.kids.avatar-boy-6.svg";
            public const string Boy7 = "OnDijon.Assets.avatars.kids.avatar-boy-7.svg";
            public const string Boy8 = "OnDijon.Assets.avatars.kids.avatar-boy-8.svg";

            public static string[] All = new[] { Boy1, Boy2, Boy3, Boy4, Boy5, Boy6, Boy7, Boy8};
        }

        public static class KidGirlAvatarSourceList
        {
            public const string Girl1 = "OnDijon.Assets.avatars.kids.avatar-girl-1.svg";
            public const string Girl2 = "OnDijon.Assets.avatars.kids.avatar-girl-2.svg";
            public const string Girl3 = "OnDijon.Assets.avatars.kids.avatar-girl-3.svg";
            public const string Girl4 = "OnDijon.Assets.avatars.kids.avatar-girl-4.svg";
            public const string Girl5 = "OnDijon.Assets.avatars.kids.avatar-girl-5.svg";
            public const string Girl6 = "OnDijon.Assets.avatars.kids.avatar-girl-6.svg";
            public const string Girl7 = "OnDijon.Assets.avatars.kids.avatar-girl-7.svg";
            public const string Girl8 = "OnDijon.Assets.avatars.kids.avatar-girl-8.svg";
            public const string Girl9 = "OnDijon.Assets.avatars.kids.avatar-girl-9.svg";

            public static string[] All = new[] { Girl1, Girl2, Girl3, Girl4, Girl5, Girl6, Girl7, Girl8, Girl9};

        }

        public static string GetAvatar(bool isGirl, int age)
        {
            if (isGirl)
            {
                if (age > 16)
                {
                    return GirlAvatarSourceList.All[new Random().Next(0, GirlAvatarSourceList.All.Length - 1)];
                }
                else
                {
                    return KidGirlAvatarSourceList.All[new Random().Next(0, KidGirlAvatarSourceList.All.Length - 1)];
                }
            }
            else
            {
                if (age > 16)
                {
                    return BoyAvatarSourceList.All[new Random().Next(0, BoyAvatarSourceList.All.Length - 1)];
                }
                else
                {
                    return KidBoyAvatarSourceList.All[new Random().Next(0, KidBoyAvatarSourceList.All.Length - 1)];
                }
            }
        }
    }
}
