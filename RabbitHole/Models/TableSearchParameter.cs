using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
namespace RabbitHole.Models {
    public class TableSearchParameter:NotificationObject {

        private string _QueryString;

        public string QueryString {
            get {
                return _QueryString;
            }
            set { 
                if (_QueryString == value) {
                    return;
                }
                _QueryString = value;
                RaisePropertyChanged();
            }
        }

    }
}
