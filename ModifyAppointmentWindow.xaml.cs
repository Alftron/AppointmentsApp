using AppointmentsApp.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppointmentsApp
{
    /// <summary>
    /// Interaction logic for ModifyAppointmentWindow.xaml
    /// </summary>
    public partial class ModifyAppointmentWindow : Window
    {
        Appointment modAppointment;
        public ModifyAppointmentWindow(Appointment appointment)
        {
            InitializeComponent();
            // Get the details
            modAppointment = appointment;
            // Populate the fields now we have access to details
            firstNameTextBox.Text = modAppointment.FirstName;
            lastNameTextBox.Text = modAppointment.LastName;
            emailTextBox.Text = modAppointment.Email;
            phoneNumberTextBox.Text = modAppointment.PhoneNumber;
            datePickerBox.Text = modAppointment.AppointmentDate;
            timePickerBox.Text = modAppointment.AppointmentTime;
            lengthTextBox.Text = modAppointment.AppointmentLength.ToString();
            costTextBox.Text = modAppointment.AppointmentCost.ToString();
            //TODO: Status combobox can go here
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Update the fields
            modAppointment.FirstName = firstNameTextBox.Text;
            modAppointment.LastName = lastNameTextBox.Text;
            modAppointment.Email = emailTextBox.Text;
            modAppointment.PhoneNumber = phoneNumberTextBox.Text;
            // Format the DateTime so we only retrieve the date
            DateTime dt = (DateTime)datePickerBox.SelectedDate;
            modAppointment.AppointmentDate = dt.Date.ToShortDateString();
            modAppointment.AppointmentTime = timePickerBox.Text;
            // Time here
            double length, cost;
            Double.TryParse(lengthTextBox.Text, out length);
            Double.TryParse(costTextBox.Text, out cost);
            modAppointment.AppointmentLength = length;
            modAppointment.AppointmentCost = cost;
            //TODO: Allow the modifiable status stuff here
            //modAppointment.Status = "Booked";

            // Send the info to the database
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                // Ensure there's a table to read from in the first place
                conn.CreateTable<Appointment>();
                // Add the new appointment
                conn.Update(modAppointment);
            }

            // Close the window
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Delete button was pressed so prompt for confirmation before doing anything else
            // Prompt
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirmation",
                MessageBoxButton.OKCancel, MessageBoxImage.Question);
            // Delete
            if (result == MessageBoxResult.OK)
            {
                // Send the info to the database
                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    // Ensure there's a table to read from in the first place
                    conn.CreateTable<Appointment>();
                    // Add the new appointment
                    conn.Delete(modAppointment);
                }
                // Now close the window
                this.Close();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window because close was pressed
            this.Close();
        }
    }
}
