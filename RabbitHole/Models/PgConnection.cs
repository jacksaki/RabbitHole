using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitHole.Models {
    public class PgConnection {
        public string Name {
            get;
            set;
        }
        public string ConnectionString {
            get;
            set;
        }
    }
}
