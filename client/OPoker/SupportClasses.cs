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
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace OPoker {
    public class MyTcpClient : INotifyPropertyChanged {
        private readonly string serverip = "127.0.0.1";
        private readonly int port = 12345;
        public TcpClient tcpClient;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Connect() {
            tcpClient = new TcpClient(serverip, port);
        }

        public void SendMsg(byte[] msg) {
            Trace.WriteLine("MSG" + msg);
            using (var stream = tcpClient.GetStream()) {
                stream.Write(msg, 0, msg.Length);
            }
        }

        public Room RcvMsg() {
            byte[] msg = new byte[2048];
            int bytes;
            using (var stream = tcpClient.GetStream()) {
                bytes = stream.Read(msg, 0, msg.Length);
            }
            string received = Encoding.ASCII.GetString(msg, 0, bytes);
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Room>(received, options);
        }

        public Room JoinRoom(string username, string room_id) {
            var join = new Command {
                kind = "JOIN",
                username = username,
                room = room_id
            };
            byte[] msg = JsonSerializer.SerializeToUtf8Bytes(join);
            SendMsg(msg);
            return RcvMsg();
        }

        public Room CreateRoom(string username) {
            var create = new Command {
                kind = "CREATE",
                username = username,
                room = ""
            };
            byte[] msg = JsonSerializer.SerializeToUtf8Bytes(create);
            SendMsg(msg);
            return RcvMsg();
        }

        public void Start(string username, string room_id) {
            var start = new Command {
                kind = "START",
                username = username,
                room = room_id
            };
            byte[] msg = JsonSerializer.SerializeToUtf8Bytes(start);
            SendMsg(msg);
        }
    }

    public class Room : INotifyPropertyChanged {
        private string _room_id;
        private int _pot_amt;

        public string Room_id {
            get => _room_id;
            set { _room_id = value; OnPropertyChanged("room_id"); }
        }
        public int Pot_amt {
            get => _pot_amt;
            set { _pot_amt = value; OnPropertyChanged("pot_amt"); }
        }
        public Player[] Players { get; set; } = new Player[8];
        public string[] Dealer_cards { get; set; } = new string[5];
        public string Winner { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public Room() {
            Room_id = "";
        }
    }

    public class Player : INotifyPropertyChanged {
        private string _username;
        private int _pot_contrib;
        private int _total_money; // remaining money


        public string Username {
            get { return _username; }
            set { _username = value; OnPropertyChanged("username"); }
        }
        public int Total_money {
            get { return _total_money; }
            set { _total_money = value; OnPropertyChanged("total_money"); }
        }
        public int Pot_contrib {
            get { return _pot_contrib; }
            set { _pot_contrib = value; OnPropertyChanged("pot_contrib"); }
        }
        public string[] Cards { get; set; } = new string[2];
        public bool Is_live { get; set; }
        public bool Is_turn_now { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyname) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        public Player() {
            Username = "";
            _total_money = 100;
        }
    }

    public class Command {
        public string kind { get; set; }
        public string username { get; set; }
        public string room { get; set; }
    }

    public class RaiseCmd : Command {
        public int amt { get; set; }
        public RaiseCmd() {
            kind = "BET";
        }
    }
}
