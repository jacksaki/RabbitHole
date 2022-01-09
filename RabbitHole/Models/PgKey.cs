using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitHole.Models {
    public class PgKey {
        public PgTable Table {
            get;
        }
        public PgKey(PgTable table) {
            this.Table = table;
        }
        public static IEnumerable<PgKey> GetAll(PgTable table) {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine(" C.column_name");
            sb.AppendLine(",K.ordinal_position");
            sb.AppendLine("FROM");
            sb.AppendLine(" information_schema.table_constraints P");
            sb.AppendLine("INNER JOIN information_schema.constraint_column_usage C ON(");
            sb.AppendLine("P.constraint_catalog = C.constraint_catalog");
            sb.AppendLine("AND P.constraint_schema = C.constraint_schema");
            sb.AppendLine("AND P.constraint_name = C.constraint_name");
            sb.AppendLine(")");
            sb.AppendLine("INNER JOIN information_schema.key_column_usage K ON(");
            sb.AppendLine("P.constraint_catalog = K.constraint_catalog");
            sb.AppendLine("AND P.constraint_schema = K.constraint_schema");
            sb.AppendLine("AND P.constraint_name = K.constraint_name");
            sb.AppendLine("AND C.column_name = K.column_name");
            sb.AppendLine(")");
            sb.AppendLine("WHERE");
            sb.AppendLine("P.TABLE_CATALOG = @TABLE_CATALOG");
            sb.AppendLine("AND P.TABLE_SCHEMA = @TABLE_SCHEMA");
            sb.AppendLine("AND P.table_name = @TABLE_NAME");
            sb.AppendLine("AND P.constraint_type = 'PRIMARY KEY'");
            sb.AppendLine("ORDER BY");
            sb.AppendLine(" K.ordinal_position");
            return new PgQuery().GetSqlResult(sb.ToString(), new Dictionary<string, object> {
                {"TABLE_CATALOG",table.Schema.Catalog },
                {"TABLE_SCHEMA",table.Schema.Name },
                {"TABLE_NAME",table.Name }
            }).Rows.Select(x => x.Create<PgKey, PgTable>(table));
        }
       
        [DbColumn("column_name")]
        public string Name {
            get;
            private set;
        }
        [DbColumn("ordinal_position")]
        public int OrdinalPosition {
            get;
            private set;
        }
    }
}
