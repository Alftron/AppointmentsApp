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
using AppointmentsApp.Classes;
using SQLite;

namespace AppointmentsApp
{
    /// <summary>
    /// Interaction logic for NewAppointmentWindow.xaml
    /// </summary>
    public partial class NewAppointmentWindow : Window
    {
        public NewAppointmentWindow()
        {
            InitializeComponent();
        }

        private bool ValidateForm()
        {
            // Used to validate the form before anything happens
            // Returns true if fine, false is error
            // Currently checking first name, date and time
            if (String.IsNullOrEmpty(firstNameTextBox.Text) ||
                datePickerBox.SelectedDate == null ||
                String.IsNullOrEmpty(timePickerBox.Text))
            {
                return false;
            }
            return true;
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            // Book button was pressed so add it to the database and then close the window

            // Create new appointment after validating form
            if (ValidateForm())
            {
                Appointment appointment = new Appointment();
                // Set the fields
                appointment.FirstName = firstNameTextBox.Text;
                appointment.LastName = lastNameTextBox.Text;
                appointment.Email = emailTextBox.Text;
                appointment.PhoneNumber = phoneNumberTextBox.Text;
                // Format the DateTime so we only retrieve the date
                DateTime dt = (DateTime)datePickerBox.SelectedDate;
                appointment.AppointmentDate = dt.Date.ToShortDateString();
                appointment.AppointmentTime = timePickerBox.Text;
                // Time here
                double length, cost;
                Double.TryParse(lengthTextBox.Text, out length);
                Double.TryParse(costTextBox.Text, out cost);
                appointment.AppointmentLength = length;
                appointment.AppointmentCost = cost;
                appointment.Status = "Booked";

                // Send the info to the database
                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    // Ensure there's a table to read from in the first place
                    conn.CreateTable<Appointment>();
                    // Add the new appointment
                    conn.Insert(appointment);
                }

                // Close the window
                this.Close();
            }
            else
            {
                // Show an error
                //TODO: make error more specific
                MessageBox.Show("Error on form!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Cancel button was pressed so discard any information and close the window
            this.Close();
        }
    }
}
