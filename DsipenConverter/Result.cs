using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DsipenConverter
{
	[XmlRoot(ElementName = "Patient")]
	public class Patient
	{

		[XmlElement(ElementName = "Ipp")]
		public string Ipp { get; set; }

		[XmlElement(ElementName = "Nom_usuel")]
		public string NomUsuel { get; set; }

		[XmlElement(ElementName = "Nom_naissance")]
		public string NomNaissance { get; set; }

		[XmlElement(ElementName = "Prénoms")]
		public string Prénoms { get; set; }

		[XmlElement(ElementName = "Date_naissance")]
		public string DateNaissance { get; set; }

		[XmlElement(ElementName = "Sexe")]
		public string Sexe { get; set; }
	}

	[XmlRoot(ElementName = "Séjour")]
	public class Séjour
	{

		[XmlElement(ElementName = "Id_séjour")]
		public string IdSéjour { get; set; }
	}

	[XmlRoot(ElementName = "Unité_hébergement")]
	public class UnitéHébergement
	{

		[XmlAttribute(AttributeName = "Phast-nomenclature")]
		public string PhastNomenclature { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Unité_resp_médicale")]
	public class UnitéRespMédicale
	{

		[XmlAttribute(AttributeName = "Phast-nomenclature")]
		public string PhastNomenclature { get; set; }

		[XmlText]
		public int Text { get; set; }
	}

	[XmlRoot(ElementName = "Rens_compl")]
	public class RensCompl
	{

		[XmlElement(ElementName = "Code_rens_compl")]
		public string CodeRensCompl { get; set; }

		[XmlElement(ElementName = "Dh_enreg_rens_compl")]
		public double DhEnregRensCompl { get; set; }

		[XmlElement(ElementName = "Valeur_rens_compl")]
		public string ValeurRensCompl { get; set; }

		[XmlElement(ElementName = "Dh_rens_compl")]
		public double DhRensCompl { get; set; }
	}

	[XmlRoot(ElementName = "Identification_prescripteur")]
	public class IdentificationPrescripteur
	{

		[XmlElement(ElementName = "Identifiant")]
		public string Identifiant { get; set; }

		[XmlElement(ElementName = "Nom_famille")]
		public string NomFamille { get; set; }

		[XmlElement(ElementName = "Prénoms")]
		public string Prénoms { get; set; }
	}

	[XmlRoot(ElementName = "Quantité_composant_prescrite")]
	public class QuantitéComposantPrescrite
	{

		[XmlElement(ElementName = "Nombre")]
		public double Nombre { get; set; }

		[XmlElement(ElementName = "Unité")]
		public string Unité { get; set; }
	}

	[XmlRoot(ElementName = "Composant_prescrit")]
	public class ComposantPrescrit
	{
		[XmlElement(ElementName = "Type_composant_1")]
		public int TypeComposant1 { get; set; }

		[XmlElement(ElementName = "Code_composant_1")]
		public string CodeComposant1 { get; set; }

		[XmlElement(ElementName = "Libellé_composant")]
		public string LibelléComposant { get; set; }

		[XmlElement(ElementName = "Quantité_composant_prescrite")]
		public QuantitéComposantPrescrite QuantitéComposantPrescrite { get; set; }

		[XmlElement(ElementName = "Véhicule")]
		public int Véhicule { get; set; }
	}

	[XmlRoot(ElementName = "Fréquence")]
	public class Fréquence
	{

		[XmlAttribute(AttributeName = "Phast-nomenclature")]
		public string PhastNomenclature { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Durée")]
	public class Durée
	{

		[XmlElement(ElementName = "Nombre")]
		public string Nombre { get; set; }

		[XmlElement(ElementName = "Unité")]
		public string Unité { get; set; }
	}

	[XmlRoot(ElementName = "Quantité")]
	public class Quantité
	{

		[XmlElement(ElementName = "Nombre")]
		public double Nombre { get; set; }

		[XmlElement(ElementName = "Unité")]
		public string Unité { get; set; }
	}

	[XmlRoot(ElementName = "Elément_posologie")]
	public class ElémentPosologie
	{
		[XmlElement(ElementName = "Type_événement_début")]
		public string TypeEvenementDebut { get; set; }

		[XmlElement(ElementName = "Fréquence")]
		public string Fréquence { get; set; }

		[XmlElement(ElementName = "Evénement_début")]
		public string EvénementDébut { get; set; }

		[XmlElement(ElementName = "Durée")]
		public Durée Durée { get; set; }

		[XmlElement(ElementName = "Quantité")]
		public Quantité Quantité { get; set; }
	}

	[XmlRoot(ElementName = "Elément_prescr_médic")]
	public class ElémentPrescrMédic
	{

		[XmlElement(ElementName = "Id_élément_prescr")]
		public int IdÉlémentPrescr { get; set; }

		[XmlElement(ElementName = "Cré_arr_mod_val")]
		public string CréArrModVal { get; set; }

		[XmlElement(ElementName = "Urgent")]
		public string Urgent { get; set; }

		[XmlElement(ElementName = "Fourniture")]
		public string Fourniture { get; set; }

		[XmlElement(ElementName = "Identification_prescripteur")]
		public IdentificationPrescripteur IdentificationPrescripteur { get; set; }

		[XmlElement(ElementName = "Voie_administration")]
		public int VoieAdministration { get; set; }

		[XmlElement(ElementName = "Dh_début")]
		public string DhDébut { get; set; }

		[XmlElement(ElementName = "Dh_fin")]
		public string DhFin { get; set; }

		[XmlElement(ElementName = "Composant_prescrit")]
		public ComposantPrescrit ComposantPrescrit { get; set; }

		[XmlElement(ElementName = "Elément_posologie")]
		public List<ElémentPosologie> ElémentPosologie { get; set; }
	}

	[XmlRoot(ElementName = "Prescription")]
	public class Prescription
	{

		[XmlElement(ElementName = "Mode_communication")]
		public string ModeCommunication { get; set; }

		[XmlElement(ElementName = "Dh_prescription")]
		public string DhPrescription { get; set; }

		[XmlElement(ElementName = "Unité_hébergement")]
		public UnitéHébergement UnitéHébergement { get; set; }

		[XmlElement(ElementName = "Unité_resp_médicale")]
		public UnitéRespMédicale UnitéRespMédicale { get; set; }

		[XmlElement(ElementName = "Rens_compl")]
		public List<RensCompl> RensCompl { get; set; }

		[XmlElement(ElementName = "Elément_prescr_médic")]
		public ElémentPrescrMédic ElémentPrescrMédic { get; set; }
	}

	[XmlRoot(ElementName = "M_Prescription_médicaments")]
	public class MPrescriptionMédicaments
	{

		[XmlElement(ElementName = "Patient")]
		public Patient Patient { get; set; }

		[XmlElement(ElementName = "Séjour")]
		public Séjour Séjour { get; set; }

		[XmlElement(ElementName = "Prescription")]
		public Prescription Prescription { get; set; }
	}

	[XmlRoot(ElementName = "Messages")]
	public class Messages
	{

		[XmlElement(ElementName = "M_Prescription_médicaments")]
		public List<MPrescriptionMédicaments> MPrescriptionMédicaments { get; set; }
	}
}
