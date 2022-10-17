using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        class User
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Speciality { get; set; }
            public DatePicker DatePicker { get; set; }
        }



        interface IAdapter
        {
            void Write(User user);

        }


        class JsonAdapter : IAdapter
        {
            private JsonDb _db;

            public JsonAdapter(JsonDb db)
            {
                _db = db;
            }

            public void Write(User user)
            {
                _db.Write(user);
            }


        }


        class XmlAdapter : IAdapter
        {
            private XmlDb _db;

            public XmlAdapter(XmlDb db)
            {
                _db = db;
            }

            public void Write(User user)
            {
                _db.Write(user);
            }


        }


        class JsonDb
        {
            public void Write(User user)
            {
                var serializer = new JsonSerializer();
                string filename = "Anket" + ".json";
                using (var sw = new StreamWriter(filename))
                { 
                    using (var jw = new JsonTextWriter(sw))
                    {
                        jw.Formatting = Formatting.Indented;
                        serializer.Serialize(jw,user.Name + " " + user.Surname+" "+user.Speciality + " " + user.DatePicker );
                    }
                }
            }

        }


        class XmlDb
        {
            public void Write(User user)
            {
                string xmlDosyasi = @"Anket111.xml";
                XmlWriter xmlYazici = XmlWriter.Create(xmlDosyasi);

                xmlYazici.WriteStartDocument();

                xmlYazici.WriteStartElement("Etiket");
                xmlYazici.WriteString(user.Name + " " + user.Surname + " " + user.Speciality + " " + user.DatePicker);
                xmlYazici.WriteEndElement();

                xmlYazici.WriteEndDocument();
                xmlYazici.Close();
            }

        }



        class Converter
        {
            private readonly IAdapter _adapter;

            public Converter(IAdapter adapter)
            {
                _adapter = adapter;
            }

            public void Write(User user)
            {
                _adapter.Write(user);
            }
        }



        class Application
        {
            private readonly Converter _converter;
            public Application(IAdapter adapter)
            {
                _converter = new Converter(adapter);
            }

            public void Start(User user)
            {
                _converter.Write(user);

            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.Name = namebox.Text;
            user.Surname = surnamebox.Text;
            user.Speciality = specialitybox.Text;
            user.DatePicker = datapicker;


            if ((bool)checkboxjson.IsChecked)
            {
                IAdapter adapter;

                JsonDb jsonDb = new JsonDb();
                adapter = new JsonAdapter(jsonDb);
                Application app = new Application(adapter);
                app.Start(user);

            }
            else if ((bool)checkboxxml.IsChecked)
            {
                IAdapter adapter;
                XmlDb xmlDb = new XmlDb();
                adapter = new XmlAdapter(xmlDb); ;
                Application app = new Application(adapter);
                app.Start(user);
            }
        }
    }
}
