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
using SQLite;
using AppointmentsApp.Classes;

namespace AppointmentsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Appointment> appointments;

        public MainWindow()
        {
            InitializeComponent();
            // Initialise a list for the appointments
            appointments = new List<Appointment>();
            // Read from the database
            ReadDatabase();

            // Add the double click handler
            appointmentsListView.AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(HandleDoubleClick));
        }

        private void HandleDoubleClick(object sender, RoutedEventArgs e)
        {
            DependencyObject depObj = e.OriginalSource as DependencyObject;
            if (depObj != null)
            {
                // Find the list view item the click came from  
                // The click might have been on the grid or column headers so we need to cater for this  
                DependencyObject current = depObj;
                while (current != null && current != appointmentsListView)
                {
                    ListViewItem lvi = current as ListViewItem;
                    if (lvi != null)
                    {
                        // this is the list view item open the modify window and pass the details
                        Appointment tmp = (Appointment)appointmentsListView.SelectedItem;
                        ModifyAppointmentWindow modWindow = new ModifyAppointmentWindow(tmp);
                        modWindow.ShowDialog();
                        // break out of loop and read the database in the case of changes
                        ReadDatabase();
                        return;
                    }
                    current = VisualTreeHelper.GetParent(current);
                }
            }
        }

        private void ReadDatabase()
        {
            // Try and get the database up and running!
            using (SQLite.SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                // Ensure there's a table to read from in the first place
                conn.CreateTable<Appointment>();
                // Read and order by date
                appointments = conn.Table<Appointment>().ToList().OrderBy(a => a.AppointmentDate).ToList();
            }

            // Populate the main table if the appointments aren't empty
            if (appointments != null)
            {
                appointmentsListView.ItemsSource = appointments;
            }
        }

        private void NewAppointmentItem_Click(object sender, RoutedEventArgs e)
        {
            // New appointment button was pressed from the file menu so load new appointment window as modal
            NewAppointmentWindow newAppointmentWindow = new NewAppointmentWindow();
            newAppointmentWindow.ShowDialog();

            // We might have a new appointment so refresh the screen
            ReadDatabase();
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            // Exit button was pressed from the file menu so close the program
            this.Close();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Text was changed in the search/filter box
        }


    }
}
