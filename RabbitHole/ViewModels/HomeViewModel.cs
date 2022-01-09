using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitHole.Models;
using Livet.Commands;
namespace RabbitHole.ViewModels {
    public class HomeViewModel : MenuItemViewModelBase {
        public HomeViewModel(MainWindowViewModel parent) : base(parent) {
            this.EntityViewModel = new EntityViewModel();
            this.Parameter = new TableSearchParameter();
            this.Parameter.QueryString = string.Empty;
            this.AllSchema = new ObservableSynchronizedCollection<PgSchema>();
            this.Tables = new ObservableSynchronizedCollection<PgTable>();
            this.Connections = new ObservableSynchronizedCollection<PgConnection>();
            foreach(var conn in AppConfig.GetInstance().Connections) {
                this.Connections.Add(conn);
            }

            this.SelectedConnection = this.Connections.FirstOrDefault();
        }

        private void Schema_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            RaisePropertyChanged(nameof(AllSchema));
        }

        private ObservableSynchronizedCollection<PgConnection> _Connections;

        public ObservableSynchronizedCollection<PgConnection> Connections {
            get {
                return _Connections;
            }
            set {

                if (_Connections == value) {
                    return;
                }
                _Connections = value;
                RaisePropertyChanged();
            }
        }

        private PgConnection _SelectedConnection;

        public PgConnection SelectedConnection {
            get {
                return _SelectedConnection;
            }
            set {
                if (_SelectedConnection == value) {
                    return;
                }
                _SelectedConnection = value;
                ConnectCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }



        private TableSearchParameter _Parameter;

        public TableSearchParameter Parameter {
            get {
                return _Parameter;
            }
            set {

                if (_Parameter == value) {
                    return;
                }
                _Parameter = value;
                RaisePropertyChanged();
            }
        }


        private ObservableSynchronizedCollection<PgSchema> _AllSchema;

        public ObservableSynchronizedCollection<PgSchema> AllSchema {
            get {
                return _AllSchema;
            }
            set {
                if (_AllSchema == value) {
                    return;
                }
                _AllSchema = value;
                RaisePropertyChanged();
            }
        }


        private PgSchema _SelectedSchema;

        public PgSchema SelectedSchema {
            get {
                return _SelectedSchema;
            }
            set {

                if (_SelectedSchema == value) {
                    return;
                }
                _SelectedSchema = value;
                SearchTableCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }


        private ObservableSynchronizedCollection<PgTable> _Tables;

        public ObservableSynchronizedCollection<PgTable> Tables {
            get {
                return _Tables;
            }
            set {
                if (_Tables == value) {
                    return;
                }
                _Tables = value;
                RaisePropertyChanged();
            }
        }


        private PgTable _SelectedTable;

        public PgTable SelectedTable {
            get {
                return _SelectedTable;
            }
            set {

                if (_SelectedTable == value) {
                    return;
                }
                _SelectedTable = value;
                this.EntityViewModel.Table = value;
                RaisePropertyChanged();
            }
        }


        private EntityViewModel _EntityViewModel;

        public EntityViewModel EntityViewModel {
            get {
                return _EntityViewModel;
            }
            set {

                if (_EntityViewModel == value) {
                    return;
                }
                _EntityViewModel = value;
                RaisePropertyChanged();
            }
        }


        private Livet.Commands.ViewModelCommand _SearchTableCommand;

        public Livet.Commands.ViewModelCommand SearchTableCommand {
            get {
                if (_SearchTableCommand == null) {
                    _SearchTableCommand = new Livet.Commands.ViewModelCommand(SearchTable, CanSearchTable);
                }
                return _SearchTableCommand;
            }
        }

        public bool CanSearchTable() {
            return this.SelectedSchema != null;
        }

        public void SearchTable() {
            this.Tables.Clear();
            foreach(var table in PgTable.GetAll(this.SelectedSchema, this.Parameter)) {
                this.Tables.Add(table);
            }
            RaisePropertyChanged(nameof(Tables));
        }


        private ViewModelCommand _ConnectCommand;

        public ViewModelCommand ConnectCommand {
            get {
                if (_ConnectCommand == null) {
                    _ConnectCommand = new ViewModelCommand(Connect, CanConnect);
                }
                return _ConnectCommand;
            }
        }

        public bool CanConnect() {
            return this.SelectedConnection != null;
        }

        public void Connect() {
            try {
                PgQuery.SetConnectionString(this.SelectedConnection.ConnectionString);
                using(var q=new PgQuery()) {
                    this.AllSchema.Clear();
                    foreach(var schema in PgSchema.GetAll()) {
                        this.AllSchema.Add(schema);
                    }
                    RaisePropertyChanged(nameof(AllSchema));
                }
            } catch (Exception ex) {
                base.OnErrorOccurred(new ErrorOccurredEventArgs("接続に失敗しました", ex));
            }
        }
    }
}
