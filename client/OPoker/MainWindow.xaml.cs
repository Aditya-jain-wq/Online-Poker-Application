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

namespace OPoker {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private TCPClient Client;
        private Room _MyRoom;

        public Room MyRoom{ 
            get { return _MyRoom; }
            set { _MyRoom = value; OnPropertyChanged("MyRoom"); }
        };

        public MainWindow() {
            Client = new TCPClient();
            this.Title = "OPoker";
            MyRoom = new Room();
            InitializeComponent();
        }

        private void BtnJoin_Click(object sender, RoutedEventArgs e) {
            InputBox.Visibility = Visibility.Visible;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e) {
            try {
                Client.CreateRoom(); // call OnRoomUpdate in this
                RoomBlock.Text = "Room ID";
                InputTextBox = MyRoom.room_id;
                InputBox.Visibility = Visibility.Visible;
                BtnGo.Visibility = Visibility.Collapsed;
                InitBtn.Visibility = Visibility.Collapsed;
                MainView.Visibility = Visibility.Visible;
            }
            catch (Exception e) {
                throw e;
            }
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e) {
            string room_id = InputTextBox.Text;
            try {
                Client.JoinRoom(id); // call OnRoomUpdate in this
                RoomBlock.Text = "Room ID";
                InputTextBox = MyRoom.room_id;
                BtnGo.Visibility = Visibility.Collapsed;
                InitBtn.Visibility = Visibility.Collapsed;
                MainView.Visibility = Visibility.Visible;
            }
            catch (Exception e) {
                throw e;
            }
        }

        public void OnRoomUpdate(Room room){
            MyRoom = room;
        }
        
        
        
    }
}
