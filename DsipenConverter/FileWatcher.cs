using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DsipenConverter
{
    internal class FileWatcher
    {
        private static string outFile = string.Empty;
        private static string ProcessedFile = string.Empty;

        public static T LoadFromXmlWithDTD<T>(string filename, XmlSerializer serial = default, System.Xml.Schema.ValidationEventHandler validationCallBack = default)
        {
            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                IgnoreWhitespace = true,
            };
            settings.ValidationEventHandler += validationCallBack;
            serial = serial ?? new XmlSerializer(typeof(T));
            using (var reader = XmlReader.Create(filename, settings))
                return (T)serial.Deserialize(reader);
        }




        public static void OnCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                Thread.Sleep(1000);
                XmlDocument doc = new XmlDocument();
                doc.Load("config.xml");
                outFile = doc.DocumentElement.SelectSingleNode("/Config/OutputDirectory").InnerText;
                ProcessedFile = doc.DocumentElement.SelectSingleNode("/Config/ProcessedFileDirectory").InnerText;
                //Messages messages = DeserializeObject(e.FullPath);
                Messages messages = LoadFromXmlWithDTD<Messages>(e.FullPath);

                string result = string.Empty;
                int lineCount = 0;
                foreach (var prescription in messages.MPrescriptionMédicaments)
                {
                    lineCount++;
                    result += createLine(prescription);
                    if (lineCount< messages.MPrescriptionMédicaments.Count)
                    {
                        result +=endLine ;
                    }
                }
                string resFileName = DateTime.Now.Ticks.ToString() + e.Name;
                File.WriteAllText(Path.Combine(outFile,  Path.ChangeExtension(resFileName, ".txt")), result);
                // Move the file
                bool exists = System.IO.Directory.Exists(ProcessedFile);
                /*if (!exists)
                    System.IO.Directory.CreateDirectory("Processed");*/
                string fileReplacedPath = Path.Combine(ProcessedFile, resFileName);
                File.Move(e.FullPath, fileReplacedPath);
                Console.WriteLine(e.Name + " Processed at " + DateTime.Now);
            }
            catch (Exception ex)
            {
                bool exists = System.IO.Directory.Exists("logs");
                if (!exists)
                    System.IO.Directory.CreateDirectory("logs");
                File.AppendText(Path.Combine("logs", "\n" + DateTime.Now.ToShortDateString() + "-----" + "\n" + ex.Message));
                Console.WriteLine(ex.Message);

            }
        }

        static string tab = "	";
        static string blanc = " ";
        static string endLine = "\n";

        private static string createLine(MPrescriptionMédicaments prescription)
        {

            //foreach(ElémentPosologie element in prescription.Prescription.ElémentPrescrMédic.ElémentPosologie)
            //{
            //    element.Quantité.Nombre
            //}
            string typeEvent = "0";
            if (!string.IsNullOrEmpty(prescription.Prescription.ElémentPrescrMédic.ComposantPrescrit.TypeEvenementDebut))
            {
                typeEvent = getValue(prescription.Prescription.ElémentPrescrMédic.ComposantPrescrit.TypeEvenementDebut);
            }
            bool isBesoin = typeEvent.ToLower() == "sb";


            string resultLine =
                    // 1 N° établissement
                    $"{getValue(prescription.Prescription.UnitéHébergement.Text)}{tab}" +
                    // 2 Nom Etablissement
                    $"{getValue(prescription.Prescription.UnitéHébergement.Text)}{tab}" +
                    // 3 N° secteur 
                    $"{blanc}{tab}" +
                    // 4 Nom secteur
                    $"{getValue(prescription.Prescription.UnitéHébergement.Text)}{tab}" +
                    // 5 N° patient
                    $"{getValue(prescription.Patient.Ipp)}{tab}" +
                    // 6 Nom patient (*)
                    $"{getValue(prescription.Patient.Prénoms + " " + prescription.Patient.NomUsuel)}{tab}" +
                    // 7 Code médecin (blanc)
                    $"{"999"}{tab}" +
                    // 8 Nom médecin
                    $"{getValue(prescription.Prescription.ElémentPrescrMédic.IdentificationPrescripteur.Prénoms + " " + prescription.Prescription.ElémentPrescrMédic.IdentificationPrescripteur.NomFamille)}{tab}" +
                    // 9 Date début traitement (aaaammjj) 
                    $"{getValueAsStringDate(prescription.Prescription.ElémentPrescrMédic.DhDébut)}{tab}" +
                    // 10 Date fin traitement (date début traitement + 30 jours maxi) (aaaammjj)
                    $"{getValueAsStringDate(prescription.Prescription.ElémentPrescrMédic.DhFin)}{tab}" +
                    // 11 Code cip (*)
                    $"{getValue(prescription.Prescription.ElémentPrescrMédic.ComposantPrescrit.CodeComposant1)}{tab}" +
                    // 12 Nom médicament (*)
                    $"{getValue(prescription.Prescription.ElémentPrescrMédic.ComposantPrescrit.LibelléComposant)}{tab}" +
                    // 13 Hors pilulier 0=non 1=oui
                    $"{getValue(prescription.Prescription.ElémentPrescrMédic.Fourniture)}{tab}" +
                    // 14 Dispensé si besoin 0=non 1=oui (si 1, les 24 quantités sont à zéro et ‘dispensé jour X’ à ‘0’)
                    $"{(isBesoin?"1":"0")}{tab}" +
                    // 15 Code forme médicament (si inexistant, mettre un blanc) 
                    $"{blanc}{tab}" +
                    // 16 Nom forme médicament (*)
                    $"{getValue(prescription.Prescription.ElémentPrescrMédic.ComposantPrescrit.QuantitéComposantPrescrite.Unité)}{tab}" +
                    // 17 Dosage médicament (si inexistant, mettre un blanc)
                    $"{blanc}{tab}" +
                    // 18 Conditionnement boite médicament (si inexistant, mettre un blanc)
                    $"{blanc}{tab}";

            DateTime? from = getValueAsDate(prescription.Prescription.ElémentPrescrMédic.DhDébut);
            DateTime? to = getValueAsDate(prescription.Prescription.ElémentPrescrMédic.DhFin);
            if (from.HasValue && to.HasValue)
            {
                int iCOunt = 0;
                string posologie = getPoso(prescription.Prescription.ElémentPrescrMédic.ElémentPosologie, isBesoin);
                resultLine = resultLine + posologie + tab;

                // Compléter les 6 jours 
                // Dispensation // A vérifier
                foreach (DateTime day in EachDay(from.Value, to.Value))
                {
                    iCOunt++;
                    if (iCOunt <= 31)
                    {
                        resultLine = resultLine + (isBesoin? "0" :"1") + tab;
                    }
                }
                /// Le reste est dispensé 
                for (int i = iCOunt; i < 31; i++)
                {
                    resultLine = resultLine + "0" + tab;
                }
            }
            // 74 N° Sécurité sociale (blanc)
            resultLine = resultLine + $"{blanc}{tab}" +
            // 75 N° de prescription (si inexistant, mettre un blanc)
            $"{getValue(prescription.Prescription.DhPrescription)}{tab}" +
            // 76 Ald  ‘A‘ = Oui
            $"{blanc}{tab}" +
            // 77 N° de ligne precription (si inexistant, mettre un blanc)
            $"{blanc}{tab}" +
            // 78 Code Utilisateur (si inexistant, mettre un blanc)
            $"{blanc}{tab}" +
            // 79 Date transfert (jj/mm/aaaa)
            $"{getValueAsStringDateWithSlash(prescription.Prescription.DhPrescription)}{tab}" +
            // 80 Heure transfert (hh :mm)
            $"{getValueAsHourMinute(prescription.Prescription.DhPrescription)}{tab}" +
            // 81 Commentaire
            $"{blanc}{tab}" +
            // 82 N° de chambre
            $"{blanc}{tab}" +
            // 83 Optionnel : Date de naissance (jj/mm/aaaa)
            $"{getValueAsStringDateWithSlash(prescription.Patient.DateNaissance)}";
            return resultLine;

        }

        private static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        private static string getPoso(List<ElémentPosologie> elementPosologies, bool IsSelonBesoin)
        {
            bool is01 = false;
            bool is02 = false;
            bool is03 = false;
            bool is04 = false;
            bool is05 = false;
            bool is06 = false;
            bool is07 = false;
            bool is08 = false;
            bool is09 = false;
            bool is10 = false;
            bool is11 = false;
            bool is12 = false;
            bool is13 = false;
            bool is14 = false;
            bool is15 = false;
            bool is16 = false;
            bool is17 = false;
            bool is18 = false;
            bool is19 = false;
            bool is20 = false;
            bool is21 = false;
            bool is22 = false;
            bool is23 = false;
            bool is24 = false;
            if (!IsSelonBesoin)
            {
                foreach (var elementPosologie in elementPosologies)
                {
                    switch ((getValueAsTime(elementPosologie.EvénementDébut).Trim()))
                    {
                        case "01":
                            is01 = true;
                            break;
                        case "02":
                            is02 = true;
                            break;
                        case "03":
                            is03 = true;
                            break;
                        case "04":
                            is04 = true;
                            break;
                        case "05":
                            is05 = true;
                            break;
                        case "06":
                            is06 = true;
                            break;
                        case "07":
                            is07 = true;
                            break;
                        case "08":
                            is08 = true;
                            break;
                        case "09":
                            is09 = true;
                            break;
                        case "10":
                            is10 = true;
                            break;
                        case "11":
                            is11 = true;
                            break;
                        case "12":
                            is12 = true;
                            break;
                        case "13":
                            is13 = true;
                            break;
                        case "14":
                            is14 = true;
                            break;
                        case "15":
                            is15 = true;
                            break;
                        case "16":
                            is16 = true;
                            break;
                        case "17":
                            is17 = true;
                            break;
                        case "18":
                            is18 = true;
                            break;
                        case "19":
                            is19 = true;
                            break;
                        case "20":
                            is20 = true;
                            break;
                        case "21":
                            is21 = true;
                            break;
                        case "22":
                            is22 = true;
                            break;
                        case "23":
                            is23 = true;
                            break;
                        case "24":
                            is24 = true;
                            break;
                    }
                }
            }
                return
                getSinglePoso(is01) + tab +
                getSinglePoso(is02) + tab +
                getSinglePoso(is03) + tab +
                getSinglePoso(is04) + tab +
                getSinglePoso(is05) + tab +
                getSinglePoso(is06) + tab +
                getSinglePoso(is07) + tab +
                getSinglePoso(is08) + tab +
                getSinglePoso(is09) + tab +
                getSinglePoso(is10) + tab +
                getSinglePoso(is11) + tab +
                getSinglePoso(is12) + tab +
                getSinglePoso(is13) + tab +
                getSinglePoso(is14) + tab +
                getSinglePoso(is15) + tab +
                getSinglePoso(is16) + tab +
                getSinglePoso(is17) + tab +
                getSinglePoso(is18) + tab +
                getSinglePoso(is19) + tab +
                getSinglePoso(is20) + tab +
                getSinglePoso(is21) + tab +
                getSinglePoso(is22) + tab +
                getSinglePoso(is23) + tab +
                getSinglePoso(is24);
        }

        private static string getSinglePoso(bool v)
        {
            return (v ? "1" : "0");
        }

        private static string getValue(string value)
        {
            return value;
        }
        private static string getValueAsStringDate(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length >= 8)
            {
                value = value.Substring(0, 8);
            }
            return value;
        }

        private static DateTime? getValueAsDate(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length >= 8)
            {
                string day = value.Substring(6, 2);
                string month = value.Substring(4, 2);
                string year = value.Substring(0, 4);
                return new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day));   
            }
            return null;
        }


        private static string getValueAsStringDateWithSlash(string value)
        {
            DateTime? res = getValueAsDate(value);
            if(res.HasValue)
            {
                return res.Value.Day.ToString().PadLeft(2, '0') + "/" +
                    res.Value.Month.ToString().PadLeft(2, '0') + "/" +
                    res.Value.Year.ToString();
            }
            return string.Empty;
        }

        private static string getValueAsTime(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length >= 10)
            {
                value = value.Substring(8, 2);
            }
            return value;
        }

        private static string getValueAsHourMinute(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length >= 12)
            {
                value = value.Substring(8, 2)+" :"+ value.Substring(10, 2);
            }
            return value;
        }
        public static void OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.GetException());
        }
    }
}
