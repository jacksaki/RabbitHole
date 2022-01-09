using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
namespace RabbitHole.Models {
    public class PgSchema:NotificationObject {
        public ObservableSynchronizedCollection<PgTable> Tables {
            get;
            private set;
        } = new ObservableSynchronizedCollection<PgTable>();

        public static IEnumerable<PgSchema> GetAll() {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine(" catalog_name");
            sb.AppendLine(",schema_name");
            sb.AppendLine("FROM");
            sb.AppendLine("information_schema.schemata");
            sb.AppendLine("ORDER BY");
            sb.AppendLine(" schema_name");
            return new PgQuery().GetSqlResult(sb.ToString(), null).Rows.Select(x => x.Create<PgSchema>());
        }
        [DbColumn("catalog_name")]
        public string Catalog {
            get;
            private set;
        }
        [DbColumn("schema_name")]
        public string Name {
            get;
            private set;
        }
        public void SearchTables(TableSearchParameter parameter) {
            this.Tables.Clear();
            foreach(var table in PgTable.GetAll(this, parameter)) {
                this.Tables.Add(table);
            }
            RaisePropertyChanged(nameof(Tables));
        }
    }
}
