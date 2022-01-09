using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using RabbitHole.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RabbitHole.ViewModels {
    public class SettingsViewModel : MenuItemViewModelBase {
        public SettingsViewModel(MainWindowViewModel parent) : base(parent) {
        }
    }
}
