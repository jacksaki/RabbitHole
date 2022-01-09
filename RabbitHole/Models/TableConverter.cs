using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitHole.Models {
    public class TableConverter {

        public string Convert(PgTable table, string templatePath) {
            if (!System.IO.File.Exists(templatePath)) {
                throw new System.IO.FileNotFoundException(templatePath);
            }
            return new RazorParser().Parse(templatePath, table);
        }
    }
}
