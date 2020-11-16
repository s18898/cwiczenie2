using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cwiczenie_2.models
{
    [XmlRoot("uczelnia")]
	public class Academy
	{
		public class Group
		{
			private string gDate;
			private string gAuthor;
			private List<Student> students;
			private List<Student> studies;
			public Group(string gDate, string gAuthor, List<Student> students, List<Student> studies)
			{
				this.gDate=gDate;
				this.gAuthor=gAuthor;
				this.students=students;
				this.studies=studies;
			}
			[XmlIgnore, JsonPropertyName("createdAt")]
			public string GDate=>this.gDate;
			[XmlIgnore, JsonPropertyName("author")]
			public string GAuthor=>this.gAuthor;
			[XmlIgnore, JsonPropertyName("studenci")]
			public List<Student> StudentList=>this.students;
			[XmlIgnore, JsonPropertyName("activeStudies")]
			public List<Student> StudiesList=>this.studies;
		}
		[XmlAttribute("createdAt"), JsonIgnore]
        public string Date{get; set;}
		[XmlAttribute("author"), JsonIgnore]
        public string Author{get; set;}
		[XmlArray("studenci"), XmlArrayItem("student")]
        public List<Student> students=new List<Student>();
		[XmlArray("activeStudies"), XmlArrayItem("studies")]
		public List<Student> studies=new List<Student>();
		[XmlIgnore, JsonPropertyName("uczelnia")]
		public Group Colleage=>new Group(this.Date, this.Author, this.students, this.studies);
	}
}
