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
        Player Player2;
        Player Player4;
        Player Player5;
        Player Player3;
        Player Player6;
        Player Player7;
        Player Player8;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public Room() {
            room_id = "";
            Player1 = null;
            Player2 = null;
            Player3 = null;
            Player4 = null;
            Player5 = null;
            Player6 = null;
            Player7 = null;
            Player8 = null;
        }
    }
    
    public class Player : INotifyPropertyChanged {
        private string _username;
        private int _rem_money; // remaining money
        
        public string username{
            get { return _username; }
            set { _username = value; OnPropertyChanged("username")}
        };
        public int rem_money{
            get { return _rem_money; }
            set { _rem_money = value; OnPropertyChanged("rem_money")}
        };

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyname) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        public Player(){
            username = "";
            rem_money = 100;
        }
    }
}
