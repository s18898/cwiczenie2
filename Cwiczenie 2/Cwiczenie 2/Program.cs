using Cwiczenie_2.models;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.Encodings.Web;

namespace Cwiczenie_2
{
    public class Program
    {
        private const string DATE_FORMAT="dd.MM.yyyy";
        private StreamWriter log=new StreamWriter(@"..\..\..\data\łog.txt");
        private string dataPath=string.Empty;
        private string resultPath=string.Empty;
        private string fileType=string.Empty;
        public static void Main(string[] args)
        {
            new Program(args);
        }
        public Program(params string[] args)
        {
            try
            {
                switch(args.Length)
                {
                    case 0:
                    {
                        this.dataPath=@"data\data.csv";
                        this.resultPath=@"..\..\..\data\result.xml";
                        this.fileType="xml";
                        break;
                    }
                    case 1:
                    {
                        this.dataPath=args[0];
                        this.resultPath=@"..\..\..\data\result.xml";
                        this.fileType="xml";
                        break;
                    }
                    case 2:
                    {
                        this.dataPath=args[0];
                        this.resultPath=args[1];
                        this.fileType="xml";
                        break;
                    }
                    case 3:
                    {
                        this.dataPath=args[0];
                        this.resultPath=args[1];
                        this.fileType=args[2];
                        break;
                    }
                    default: throw new ArgumentException("Invalid number of arguments.");
                }
                if(!this.resultPath.Contains(this.fileType)) throw new InvalidOperationException("Invalid type of result file.");
                this.Init();
            }
            catch(Exception exc)
            {
                this.log.WriteLine("Klasa błędu: "+exc.GetType().Name+", komunikat błędu: "+exc.Message);
                this.log.Flush();
            }
        }
        public void Init()
        {
            Academy ctg=new Academy
            {
                Date=DateTime.Today.ToString(Program.DATE_FORMAT),
                Author="Oleksii Arskyi"
            };
            FileStream fs=new FileStream(this.resultPath, FileMode.Create);
            Dictionary<string, int> count=new Dictionary<string, int>();
            XmlSerializer xs=new XmlSerializer(ctg.GetType());
            XmlSerializerNamespaces ns=new XmlSerializerNamespaces(new[]{XmlQualifiedName.Empty});
            XmlWriterSettings xws=new XmlWriterSettings()
            {
                Indent=true,
                OmitXmlDeclaration=true
            };
            JsonSerializerOptions jso=new JsonSerializerOptions
            {
                Encoder=JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase,
                IgnoreNullValues=true,
                WriteIndented=true,
                PropertyNameCaseInsensitive=true
            };
            using(StreamReader sr=new StreamReader(new FileInfo(this.dataPath).OpenRead()))
            {
                string line=string.Empty;
                while((line=sr.ReadLine())!=null)
                {
                    string[] element=line.Split(',');
                    try
                    {
                        if(element.Length!=9||element.Any(s=>s.Equals(""))||element.Any(s=>s.Equals(" "))) throw new InvalidOperationException("Invalid data occures, could not add a new student: "+line);
                        if(ctg.students.Where(s=>s.Id=="s"+element[4]&&s.Name==element[0]&&s.Surname==element[1]).Count()!=0) throw new InvalidOperationException("Repeated data occures, could not add a new student: "+line);
                    }
                    catch(Exception exc)
                    {
                        this.log.WriteLine("Klasa błędu: "+exc.GetType().Name+", komunikat błędu: "+exc.Message);
                        this.log.Flush();
                        continue;
                    }
                    if(element[2].StartsWith("Informatyka")) element[2]="Computer Science";
                    else if(element[2].StartsWith("Sztuka Nowych Mediów")) element[2]="New Media Art";
                    else if(element[2].StartsWith("Zarządzanie informacją")) element[2]="Information Management";
                    else if(element[2].StartsWith("MBA dla branży IT")) element[2]="MBA for IT";
                    Student student=new Student
                    {
                        Id="s"+element[4],
                        Name=element[0],
                        Surname=element[1],
                        Birthdate=DateTime.Parse(element[5]).ToString(Program.DATE_FORMAT),
                        Email=element[6],
                        MothersName=element[7],
                        FathersName=element[8],
                        StudiesList=new Student.Studies
                        {
                            Name=element[2],
                            Type=element[3]
                        }
                    };
                    ctg.students.Add(student);
                    if(!count.ContainsKey(element[2])) count.Add(element[2], 1);
                    else count[element[2]]++;
                }
                List<string> studies=new List<string>(count.Keys);
                for(int i=0; i<studies.Count; i++)
                {
                    Student activeStudy=new Student
                    {
                        Study=studies[i],
                        Number=count[studies[i]].ToString()
                    };
                    ctg.studies.Add(activeStudy);
                }
                if(this.fileType.Equals("xml")) xs.Serialize(XmlWriter.Create(fs, xws), ctg, ns);
                else if(this.fileType.Equals("json"))
                {
                    fs.Close();
                    File.WriteAllText(this.resultPath, JsonSerializer.Serialize(ctg, jso));
                }
            }
            this.log.Close();
        }
    }
}
