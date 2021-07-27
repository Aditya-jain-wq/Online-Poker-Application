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

        internal Room JoinRoom(string username, string room_id) {
            throw new NotImplementedException();
        }

        internal Room CreateRoom(string username) {
            throw new NotImplementedException();
        }

        internal void Start() {
            throw new NotImplementedException();
        }
    }

    public class Room : INotifyPropertyChanged {
        public string room_id;
        public Player Player1;
        public Player Player2;
        public Player Player3;
        public Player Player4;
        public Player Player5;
        public Player Player6;
        public Player Player7;
        public Player Player8;
        private int _pot_amt;

        public int pot_amt {
            get => _pot_amt;
            set { _pot_amt = value; OnPropertyChanged("pot_amt"); }
        }

        public string[] dealer_cards;

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
        private int _pot_contrib;

        public string[] cards;
        public bool is_live;
        public bool is_turn_now;
        
        public string username{
            get { return _username; }
            set { _username = value; OnPropertyChanged("username"); }
        }
        public int rem_money{
            get { return _rem_money; }
            set { _rem_money = value; OnPropertyChanged("rem_money"); }
        }
        public int pot_contrib{
            get { return _pot_contrib; }
            set { _pot_contrib = value; OnPropertyChanged("pot_contrib"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyname) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        public Player(){
            username = "";
            rem_money = 100;
        }
    }

    public class Command {
        public string kind;
        
    }
    
}
