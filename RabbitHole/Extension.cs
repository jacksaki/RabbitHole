using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitHole {
    public static class Extension {
        public static string ToPascalCase(this string value) {
            var count = value.Length;
            var sb = new System.Text.StringBuilder();
            var toUpper = false;
            for(var i = 0; i < count; i++) {
                if (i == 0) {
                    sb.Append(value[i].ToString().ToUpper());
                } else if (value[i] == '_') {
                    toUpper = true;
                }else if (toUpper) {
                    sb.Append(value[i].ToString().ToUpper());
                } else {
                    sb.Append(value[i].ToString());
                }
            }
            return sb.ToString();
        }
    }
}
