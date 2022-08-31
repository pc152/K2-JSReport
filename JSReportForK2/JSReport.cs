using System;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using Attributes = SourceCode.SmartObjects.Services.ServiceSDK.Attributes;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System.IO;
using jsreport.Client;

namespace JSReportForK2
{
    [Attributes.ServiceObject("JSReport", "Get report from JSReport", "Permet de récupérer des raports PDF, Word, Excel... depuis JSReport")]
    public class JSReport
    {
        public JSReport()
        {

        }

        private string _nomModel        = "";
        private string _contenu         = "";
        private string _dataJSON        = "";
        private string _document        = "";
        private string _nomDocument     = "";

        /// <summary>
        /// Nom du modèle de raport qui doit être utilisé par JSReport et stocké dans JSReport
        /// </summary>
        [Attributes.Property("NomModel", SoType.Text, "NomModel", "Indiquer le nom du model à utiliser")]
        public string NomModel
        {
            get{ return _nomModel;}
            set{ _nomModel = value;}
        }
        
        /// <summary>
        /// Contenu du rapport au format HTML
        /// </summary>
        [Attributes.Property("Contenu", SoType.Memo, "Contenu", "Permet de générer un rapport en passant directement le contenu HTML")]
        public string Contenu
        {
            get { return _contenu; }
            set { _contenu = value; }
        }
        
        /// <summary>
        /// Donnée au format JSON attendu par le modèle de rapport
        /// </summary>
        [Attributes.Property("DataJSON", SoType.Text, "DataJSON", "Indiquer les données du rapport")]
        public string DataJSON
        {
            get { return _dataJSON; }
            set { _dataJSON = value; }
        }
        
        /// <summary>
        /// Fichier (PDF, Excel etc...) retour par JSReport
        /// </summary>
        [Attributes.Property("Document", SoType.File, "Document", "Le rapport retourné par JSReport")]
        public string Document
        {
            get { return _document; }
            set { _document = value; }
        }

        /// <summary>
        /// Nom à donner au document
        /// </summary>
        [Attributes.Property("FileNameDocument", SoType.Text, "FileNameDocument", "Nom du rapport généré")]
        public string FileNameDocument
        {
            get { return _nomDocument; }
            set { _nomDocument = value; }
        }



        private ServiceConfiguration _serviceConfiguration;
        public ServiceConfiguration ServiceConfiguration
        {
            get { return _serviceConfiguration; }
            set { _serviceConfiguration = value; }
        }
        private ServicePackage _servicePackage;
        public ServicePackage ServicePackage
        {
            get { return _servicePackage; }
            set { _servicePackage = value; }
        }


        [Attributes.Method("GetByName", MethodType.Read, "Récupérer le rapport par son nom", "Permet de récupérer le rapport généré",
                             new string[] {
                              "NomModel","FileNameDocument","DataJSON"
                             },
                             new string[] {
                              "NomModel","FileNameDocument","DataJSON"
                             },
                             new string[] {
                              "Document"
                             })]
        public JSReport  GetByName()
        {
            // Récupération des informations de connexion
            string uRLJSReport = ServiceConfiguration["URLJSReport"].ToString();
            string login = ServiceConfiguration["Login"].ToString();
            string password = ServiceConfiguration["Password"].ToString();
            
            // Cela est important car nous devons retourner cette propore classe
            // avec les propriétés de sorties remplies
            JSReport jsReport = new JSReport();

            try
            {
                // Objet provenant de l'import des classes JSReport
                // Nous créons l'objet permettant la communication avec notre instance de JSReport
                var rs = new ReportingService(uRLJSReport, login, password);

                // On passe le modèle et les datas
                var invoiceReport = rs.RenderByNameAsync(_nomModel, _dataJSON).Result;

                // On récupère le contenu renvoyé par JSReport
                Stream stream = invoiceReport.Content;

                // On parcour le contenu pour en créer un tableau de "byte"
                byte[] bytes;
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                // On génère un "string" sur une base64 contenant le fichier
                string base64 = Convert.ToBase64String(bytes);
                // On remplit la propriété Document avec le nom et le contenu du fichier
                // Se format est le seul permettant à K2 d'afficher le fichier
                jsReport.Document = "<file><name>" + FileNameDocument + "</name><content>" + base64 + "</content></file>";
                
                // On renvois donc l'objet avec les informations à K2
                return jsReport;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                jsReport.Contenu = errorMsg;
                return jsReport;
            }
        }
    }
}
