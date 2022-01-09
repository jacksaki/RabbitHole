using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
namespace RabbitHole.Models {
    public class PgTable {
        public PgSchema Schema {
            get;
        }
        public PgTable(PgSchema schema) {
            this.Schema = schema;
        }
        public static IEnumerable<PgTable> GetAll(PgSchema schema, TableSearchParameter parameter) {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine(" table_catalog");
            sb.AppendLine(",table_schema");
            sb.AppendLine(",table_name");
            sb.AppendLine(",table_type");
            sb.AppendLine("FROM");
            sb.AppendLine(" information_schema.tables");
            var param = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(parameter?.QueryString)) {
                sb.AppendLine("WHERE");
                sb.AppendLine("table_name LIKE @QUERY_STRING");
                param.Add("QUERY_STRING", $"%{parameter.QueryString}%");
            }
            sb.AppendLine("ORDER BY");
            sb.AppendLine(" table_catalog");
            sb.AppendLine(",table_schema");
            sb.AppendLine(",table_name");
            return new PgQuery().GetSqlResult(sb.ToString(), param).Rows.Select(x => x.Create<PgTable,PgSchema>(schema));
        }
        [DbColumn("table_name")]
        public string Name {
            get;
            private set;
        }
        [DbColumn("table_type")]
        public string TableType {
            get;
            private set;
        }
        public bool IsView {
            get {
                return this.TableType.Equals("VIEW");
            }
        }
        private ObservableSynchronizedCollection<PgColumn> _columns;
        public ObservableSynchronizedCollection<PgColumn> Columns {
            get {
                if (_columns == null) {
                    _columns = new ObservableSynchronizedCollection<PgColumn>();
                    foreach(var col in PgColumn.GetAll(this)) {
                        _columns.Add(col);
                    }
                }
                return _columns;
            }
        }

        private ObservableSynchronizedCollection<PgKey> _keys;
        public ObservableSynchronizedCollection<PgKey> Keys {
            get {
                if (_keys == null) {
                    _keys = new ObservableSynchronizedCollection<PgKey>();
                    foreach (var key in PgKey.GetAll(this)) {
                        _keys.Add(key);
                    }
                }
                return _keys;
            }
        }

        public void InitCLRDataTypes() {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine(" *");
            sb.AppendLine("FROM");
            sb.AppendLine($" {this.Schema.Name}.{this.Name}");
            sb.AppendLine("WHERE");
            sb.AppendLine("1 = 2");
            var result = new PgQuery().GetSqlResult(sb.ToString(), null); ;
            foreach(var col in this.Columns) {
                col.SetCLRDataType(result.Columns.Where(x => x.ColumnName.Equals(col.Name)).FirstOrDefault().DataType);
            }
        }

    }
}
