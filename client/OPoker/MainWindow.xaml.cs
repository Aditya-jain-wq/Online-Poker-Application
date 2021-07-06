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
        public MainWindow() {
            InitializeComponent();
        }

        private void BtnJoin_Click(object sender, RoutedEventArgs e) {
            InputBox.Visibility = Visibility.Visible;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e) {

        }

        private void BtnGo_Click(object sender, RoutedEventArgs e) {
            string room_id = InputTextBox.Text;
            try {
                var id = new RoomId(room_id);
                JoinRoom(id);
                InputBox.Visibility = Visibility.Collapsed;
            } catch (Exception) {

            }
        }

        private void JoinRoom(RoomId room_id) { throw new NotImplementedException(); }
    }
}
