using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorEngine;
using RazorEngine.Templating;
namespace RabbitHole.Models {
    public class RazorParser {
        public string Parse(string templatePath, object model) {
            return Engine.Razor.RunCompile(
                System.IO.File.ReadAllText(templatePath), 
                System.IO.Path.GetFileNameWithoutExtension(templatePath), 
                model.GetType(), 
                model);
        }
    }
}
