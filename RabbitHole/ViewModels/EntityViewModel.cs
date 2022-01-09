using ICSharpCode.AvalonEdit.Document;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using RabbitHole.Models;
namespace RabbitHole.ViewModels {
    public class EntityViewModel : ViewModel {
        public EntityViewModel() : base() {
            this.Document = new TextDocument();
            this.Document.TextChanged += (sender, e) => {
                RaisePropertyChanged(nameof(Document));
            };
        }
        // Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).

        // This method would be called from View, when ContentRendered event was raised.
        public void Initialize() {
        }

        private TextDocument _Document;

        public TextDocument Document {
            get {
                return _Document;
            }
            set {

                if (_Document == value) {
                    return;
                }
                _Document = value;
                RaisePropertyChanged();
            }
        }

        public string TemplatePath {
            get {
                return System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                    "EntityModel.template");
            }
        }
        
        private PgTable _Table;

        public PgTable Table {
            get {
                return _Table;
            }
            set {

                if (_Table == value) {
                    return;
                }
                _Table = value;
                value.InitCLRDataTypes();
                this.Document.Text = new TableConverter().Convert(value, this.TemplatePath);
                RaisePropertyChanged();
            }
        }

    }
}
