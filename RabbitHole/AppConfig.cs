using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RabbitHole.Models;
namespace RabbitHole {
    public enum DatabaseType {
        [EnumText("PostgreSQL")]
        PostgreSQL,
    }
    public class AppConfig {

        private static AppConfig _singleton = null;
        private AppConfig() {

        }
        public static AppConfig GetInstance() {
            if (_singleton == null) {
                _singleton = new AppConfig();
                _singleton.Load();
            }
            return _singleton;
        }
        public string ConfigPath {
            get {
                return System.IO.Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, ".conf");
            }
        }
        public void Load() {
            var doc = XDocument.Load(this.ConfigPath);
            var root = doc.Element("Configurations");
            var db = root.Element("Databases");
            this.Connections = db.Elements("Database").Select(x=>new PgConnection() { Name = x.Attribute("Name").Value, ConnectionString = x.Attribute("ConnectionString").Value });
        }
        public IEnumerable<PgConnection> Connections {
            get;
            private set;
        }
    }
}
