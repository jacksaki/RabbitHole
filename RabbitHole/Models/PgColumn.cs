using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitHole.Models {
    public class PgColumn {
        public PgTable Table {
            get;
        }
        public PgColumn(PgTable table) {
            this.Table = table;
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
        [DbColumn("column_default")]
        public string ColumnDefault {
            get;
            private set;
        }
        [DbColumn("nullable")]
        public bool Nullable {
            get;
            private set;
        }
        [DbColumn("data_type")]
        public string DataType {
            get;
            private set;
        }
        internal void SetCLRDataType(Type type) {
            this.CLRDataType = type;
        }
        public Type CLRDataType {
            get;
            private set;
        }
        public string DataTypeText {
            get {
                if (this.CharacterMaxLength.HasValue) {
                    return $"{this.DataType}({this.CharacterMaxLength})";
                } else if ((this.DatetimePrecision.HasValue)) {
                    if (this.DatetimePrecision > 0) {
                        return $"{this.DataType}({this.DatetimePrecision})";
                    } else {
                        return $"{this.DataType}";
                    }
                } else {
                    return "";
                }
            }
        }
        [DbColumn("character_maximum_length")]
        public int? CharacterMaxLength {
            get;
            private set;
        }
        [DbColumn("datetime_precision")]
        public int? DatetimePrecision {
            get;
            private set;
        }
        public bool IsKey {
            get {
                return this.Table.Keys.Where(x => x.Name.Equals(this.Name)).Any();
            }
        }
        public static IEnumerable<PgColumn> GetAll(PgTable table) {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine(" column_name");
            sb.AppendLine(",ordinal_position");
            sb.AppendLine(",column_default");
            sb.AppendLine(",CASE WHEN is_nullable = 'YES' THEN 'Y' ELSE 'N' END AS nullable");
            sb.AppendLine(",data_type");
            sb.AppendLine(",character_maximum_length");
            sb.AppendLine(",datetime_precision");
            sb.AppendLine("FROM");
            sb.AppendLine(" information_schema.columns");
            sb.AppendLine("WHERE");
            sb.AppendLine("table_catalog = @TABLE_CATALOG");
            sb.AppendLine("AND table_schema = @TABLE_SCHEMA");
            sb.AppendLine("AND table_name = @TABLE_NAME");
            sb.AppendLine("ORDER BY");
            sb.AppendLine(" ordinal_position");
            return new PgQuery().GetSqlResult(sb.ToString(), new Dictionary<string, object> {
                {"TABLE_CATALOG",table.Schema.Catalog },
                {"TABLE_SCHEMA",table.Schema.Name },
                {"TABLE_NAME",table.Name }
            }).Rows.Select(x => x.Create<PgColumn, PgTable>(table));
        }
    }
}
