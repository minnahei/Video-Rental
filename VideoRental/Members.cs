using System;
using System.Data.SqlClient;

namespace VideoRental
{
    public class Members
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Members(int id, string firstName, string lastname, string email, string password, DateTime dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            Lastname = lastname;
            Email = email;
            Password = password;
            DateOfBirth = dateOfBirth;
        }

        public int CalculateAge()
        {
            int memberAge;

            memberAge = Convert.ToInt32(DateTime.Today.Year - DateOfBirth.Year);

            return memberAge;

        }


    }


}



