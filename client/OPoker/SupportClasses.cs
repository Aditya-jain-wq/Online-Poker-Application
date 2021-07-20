using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace OPoker {
    public class MyTcpClient : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyname) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        internal void JoinRoom(string room_id) {
            throw new NotImplementedException();
        }

        internal void CreateRoom() {
            throw new NotImplementedException();
        }
    }

    public class Room : INotifyPropertyChanged {
        public string room_id;
        Player Player1;
        string Player2;
        string Player3;
        string Player4;
        string Player5;
        string Player6;
        string Player7;
        string Player8;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public string Pl1 {
            get => Player1.username;
            set => Player1.username = value;
        }

        public Room() {
            room_id = "";
            Player1 = null;
            Player2 = "";
            Player3 = "";
            Player4 = "";
            Player5 = "";
            Player6 = "";
            Player7 = "";
            Player8 = "";
        }
    }
    
    public class Player : INotifyPropertyChanged {
        public string username;
        public int rem_money;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyname) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
