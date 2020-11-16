using System.Xml.Serialization;
using System.Text.Json.Serialization;

namespace Cwiczenie_2.models
{
    public class Student
    {
        public class Studies
        {
            [XmlElement("name"), JsonPropertyName("name")]
            public string Name{get; set;}
            [XmlElement("mode"), JsonPropertyName("mode")]
            public string Type{get; set;}
        }
        [XmlAttribute("indexNumber"), JsonPropertyName("indexNumber")]
        public string Id{get; set;}
        [XmlElement("fname"), JsonPropertyName("fname")]
        public string Name{get; set;}
        [XmlElement("lname"), JsonPropertyName("lname")]
        public string Surname{get; set;}
        [XmlElement("birthdate"), JsonPropertyName("birthdate")]
        public string Birthdate{get; set;}
        [XmlElement("email"), JsonPropertyName("email")]
        public string Email{get; set;}
        [XmlElement("mothersName"), JsonPropertyName("mothersName")]
        public string MothersName{get; set;}
        [XmlElement("fathersName"), JsonPropertyName("fathersName")]
        public string FathersName{get; set;}
        [XmlElement("studies"), JsonPropertyName("studies")]
        public Studies StudiesList{get; set;}
        [XmlAttribute("name"), JsonPropertyName("name")]
        public string Study{get; set;}
		[XmlAttribute("numberOfStudents"), JsonPropertyName("numberOfStudents")]
        public string Number{get; set;}
    }
}
