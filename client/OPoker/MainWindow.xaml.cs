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
        private Image _Card1;
        private Image _Card2;
        private Image _Card3;
        private Image _Card4;
        private Image _Card5;

        public Room MyRoom{ 
            get { return _MyRoom; }
            set { _MyRoom = value; OnPropertyChanged("MyRoom"); }
        }
        public Image Card1{
            get { return _Card1; }
            set { _Card1 = value; OnPropertyChanged("Card1"); }
        }
        public Image Card2{
            get { return _Card2; }
            set { _Card2 = value; OnPropertyChanged("Card2"); }
        }
        public Image Card3{
            get { return _Card3; }
            set { _Card3 = value; OnPropertyChanged("Card3"); }
        }
        public Image Card4{
            get { return _Card4; }
            set { _Card4 = value; OnPropertyChanged("Card4"); }
        }
        public Image Card5{
            get { return _Card5; }
            set { _Card5 = value; OnPropertyChanged("Card5"); }
        }
        

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
