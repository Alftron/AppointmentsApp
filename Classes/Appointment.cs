using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AppointmentsApp.Classes
{
    public class Appointment
    {
        // Primary key for the table
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Properties and columns for the table
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public double AppointmentLength { get; set; }
        public double AppointmentCost { get; set; }
        public string Status { get; set; }
    }
}
