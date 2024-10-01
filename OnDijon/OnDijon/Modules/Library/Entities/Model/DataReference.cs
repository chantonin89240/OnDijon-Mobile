using System.Collections.Generic;
using OnDijon.Common.Utils;

namespace OnDijon.Modules.Library.Entities.Model
{
    public class DataReference
    {

        public class CodeTypeOfDocument
        {
            public const string LIVR = "LIVR";
            public const string FASC = "FASC";
            public const string DSON = "DSON";
            public const string DAVI = "DAVI";
        }

        public static readonly IDictionary<string, string> UrlIconTypeOfDocument = new Dictionary<string, string>() {
            { CodeTypeOfDocument.LIVR, Constants.ICON_BM_URL + "icon_livr.png" },
            { CodeTypeOfDocument.FASC, Constants.ICON_BM_URL + "icon_fasc.png" },
            { CodeTypeOfDocument.DSON , Constants.ICON_BM_URL + "icon_dson.png" },
            { CodeTypeOfDocument.DAVI, Constants.ICON_BM_URL + "icon_davi.png" },
        };

        //public class CodeTypeOfDocument
        //{
        //    public const string BD = "BAD";
        //    public const string Carte = "CAR";
        //    public const string K7 = "CAS";
        //    public const string CD = "CDA";
        //    public const string Cederom = "CDR";
        //    public const string Commande = "CMD";
        //    public const string CDDaisy = "DAI";
        //    public const string Diapositive = "DIA";
        //    public const string DVD = "DVD";
        //    public const string DVDrom = "DVR";
        //    public const string Image = "IMA";
        //    public const string Jeu = "JOU";
        //    public const string JeuVideo = "JVD";
        //    public const string Video = "KVI";
        //    public const string Liseuse = "LIS";
        //    public const string Livre = "LIV";
        //    public const string MethodeLangue = "MET";
        //    public const string Microforme = "MIC";
        //    public const string CDNumerique = "NCD";
        //    public const string LivreNumerique = "NLI";
        //    public const string VideoNumerique = "NVI";
        //    public const string Partition = "PAR";
        //    public const string Plan = "PLA";
        //    public const string Revue = "REV";
        //    public const string Tablette = "TAB";
        //    public const string DisqueVideo = "VDS";
        //    public const string LecteurVictor = "VIC";
        //    public const string Vinyl = "VIN";
        //}

        //public static readonly IDictionary<string, string> UrlIconTypeOfDocument = new Dictionary<string, string>() {
        //    { CodeTypeOfDocument.BD, "" },
        //    { CodeTypeOfDocument.Carte, "" },
        //    { CodeTypeOfDocument.K7 , "" },
        //    { CodeTypeOfDocument.CD, "" },
        //    { CodeTypeOfDocument.Cederom, "" },
        //    { CodeTypeOfDocument.Commande, "" },
        //    { CodeTypeOfDocument.CDDaisy, "" },
        //    { CodeTypeOfDocument.Diapositive, "" },
        //    { CodeTypeOfDocument.DVD , "" },
        //    { CodeTypeOfDocument.DVDrom, "" },
        //    { CodeTypeOfDocument.Image , "" },
        //    { CodeTypeOfDocument.Jeu, "" },
        //    { CodeTypeOfDocument.JeuVideo, "" },
        //    { CodeTypeOfDocument.Video, "" },
        //    { CodeTypeOfDocument.Liseuse, "" },
        //    { CodeTypeOfDocument.Livre , "https://eservices.dijon.fr/Style%20Library/dijon/web/bm/sprites/icon_livr.png" },
        //    { CodeTypeOfDocument.MethodeLangue, "" },
        //    { CodeTypeOfDocument.Microforme, "" },
        //    { CodeTypeOfDocument.CDNumerique, "" },
        //    { CodeTypeOfDocument.LivreNumerique, "" },
        //    { CodeTypeOfDocument.VideoNumerique, "" },
        //    { CodeTypeOfDocument.Partition, "" },
        //    { CodeTypeOfDocument.Plan, "" },
        //    { CodeTypeOfDocument.Revue, "" },
        //    { CodeTypeOfDocument.Tablette, "" },
        //    { CodeTypeOfDocument.DisqueVideo, "" },
        //    { CodeTypeOfDocument.LecteurVictor, "" },
        //    { CodeTypeOfDocument.Vinyl, "" },
        //};
    }
}
