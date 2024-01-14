using System;

namespace AbeckDev.DLRG.ExamRegistration.Functions.Models
{
	public class Exam
	{
		public int ExamId { get; set; }
		public Landesverband Landesverband { get; set; }
		public string ExamName { get; set; }
		public string EventLinkId { get; set; }

	}

	public enum Landesverband
	{
		Hessen,
		Sachsen,
		MecklenburgVorpommern,
		Bayern,
		Baden,
		Wuerttemberg,
		Thueringen,
		Saarland,
		RheinlandPfalz,
		Nordrhein,
		Westfalen,
		Niedersachsen,
		Hamburg,
		Bremen,
		Berlin,
		Brandenburg,
		SchleswigHolstein,
		SachsenAnhalt,
		Bundesverband
	}
}

