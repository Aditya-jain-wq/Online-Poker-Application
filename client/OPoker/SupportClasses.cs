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
        private string _room_id;
        private int _pot_amt;

        public string room_id { 
            get => _room_id;
            set { _room_id = value; OnPropertyChanged("room_id"); } 
        }
        public int pot_amt {
            get => _pot_amt;
            set { _pot_amt = value; OnPropertyChanged("pot_amt"); }
        }
        public Player[] Players { get; set; } = new Player[8];
        public string[] dealer_cards { get; set; } = new string[5];

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public Room() {
            room_id = "";
            Pla
        }
    }
    
    public class Player : INotifyPropertyChanged {
        private string _username;
        private int _pot_contrib;
        private int _total_money; // remaining money

        
        public string username{
            get { return _username; }
            set { _username = value; OnPropertyChanged("username"); }
        }
        public int total_money{
            get { return _total_money; }
            set { _total_money = value; OnPropertyChanged("total_money"); }
        }
        public int pot_contrib{
            get { return _pot_contrib; }
            set { _pot_contrib = value; OnPropertyChanged("pot_contrib"); }
        }
        public string[] cards { get; set; } = new string[2];
        public bool is_live { get; set; }
        public bool is_turn_now { get; set; }

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
