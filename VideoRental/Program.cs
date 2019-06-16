using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace VideoRental
{
    class Program
    {
        static SqlConnection connection = new SqlConnection("Server=localhost;Database=VideoClub;User Id=SA;Password=minnadockersql123;");
        static Members member = null;
        static Rent rent;
        static void Main(string[] args)
        {
            int num = 0;
            do
            {
                Console.WriteLine("*********************************");
                Console.WriteLine("Welcome to The Video Rental!"); 
                Console.WriteLine("*********************************");
                Console.WriteLine("Select (1) to login or (2) to sign up as a new member");

                int answer = Convert.ToInt32(Console.ReadLine());

                switch (answer)
                {
                    case 1:
                        LogIn();
                        break;

                    case 2:
                        Register();
                        break;

                    default:
                        Console.WriteLine("Error!");
                        break;
                }

            } while (num != 1 || num != 2);
        }



        public static void LogIn()
        {
            // We set the bool to false if we dont find the member
            bool found = false; 
            do
            {

                Console.WriteLine("Enter your first name:");
                string firstname = Console.ReadLine();

                Console.WriteLine("Enter your password:");
                string password = Console.ReadLine();

                string query = "SELECT * FROM Members WHERE FirstName='" + firstname + "'AND Password = '" + password + "'";
                SqlCommand comando = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader register = comando.ExecuteReader();

                if (register.Read())
                {
                    found = true;

                    Console.WriteLine("************ WELCOME! **************");

                    int id = Convert.ToInt32(register[0].ToString());
                    string firstName = register[1].ToString();
                    string lastName = register[2].ToString();
                    string email = register[3].ToString();
                    string password1 = register[4].ToString();
                    DateTime dateofbirth = Convert.ToDateTime(register[6].ToString());

                    member = new Members(id, firstName, lastName, email, password1, dateofbirth);

                    connection.Close();

                    Menu();
                }
                else
                {
                    Console.WriteLine("Incorrect! Try again!");
                }
            } while (found == false);

        }



        public static void Register()
        {
            Console.WriteLine("Enter your details to sign up");
            Console.WriteLine("");
            Console.WriteLine("First name:");
            string firstname = Console.ReadLine();

            Console.WriteLine("Last name:");
            string lastname = Console.ReadLine();

            Console.WriteLine("Email:");
            string email = Console.ReadLine();

            Console.WriteLine("Password:");
            string password = Console.ReadLine();
              
            Console.WriteLine("Date of birth (YYYY/MM/dd):");
            DateTime dateOfBirth = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("");
            Console.WriteLine("You have signed up successfully!");
            Console.WriteLine("");

            connection.Open();

            string query = "INSERT INTO Members (FirstName, LastName, Email, Password, DateOfBirth) VALUES ('" + firstname + "','" + lastname + "','" + email + "','" + password + "','" + dateOfBirth + "')";

            SqlCommand comando = new SqlCommand(query, connection);
            comando.ExecuteNonQuery();
            connection.Close();

            Menu();
        }



        public static void Menu()
        {


            Console.WriteLine("");
            Console.WriteLine("********** MAIN MENU ************");
            Console.WriteLine("");
            Console.WriteLine("1. AVAILABLE MOVIES");
            Console.WriteLine("2. RENT MOVIE");
            Console.WriteLine("3. MY RENTALS");
            Console.WriteLine("4. RETURN MOVIE");
            Console.WriteLine("5. LOGOUT");
            Console.WriteLine("");
            Console.WriteLine("*****************************");

            Console.WriteLine("Enter you option");

            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    AvailableMovies();
                    break;

                case 2:
                    RentMovies();
                    break;

                case 3:
                    MyRentals();
                    break;

                case 4:
                    ReturnMovie();
                    break;

                default:
                    Console.WriteLine("You are logged out. Thank you and hope to see you soon again!"); //todo: back to welcome
                    break;
            }
        }




        public static void AvailableMovies() 
        {
            Console.WriteLine("AVAILABLE MOVIES");
            Console.WriteLine("*******************");

            connection.Open();

            string query = "SELECT * FROM Movies WHERE Age <='" + member.CalculateAge() + "'AND Status = 'Available'";

            SqlCommand comand = new SqlCommand(query, connection);
            SqlDataReader register = comand.ExecuteReader();

            while (register.Read())
            {
                Console.WriteLine("Movie ID: " + register[0].ToString());
                Console.WriteLine("");
                Console.WriteLine("Title: " + register[1].ToString());
                Console.WriteLine("");
                Console.WriteLine("Synopsis: " + register[2].ToString());
                Console.WriteLine("");
                Console.WriteLine("Status: " + register[3].ToString());
                Console.WriteLine("");
                Console.WriteLine("Age: " + register[4].ToString());
                Console.WriteLine("");
                Console.WriteLine("************************************");
            }
            connection.Close();

            Console.WriteLine("Enter 0 if you want to go back to the main menu");
            int mainmenu = Convert.ToInt32(Console.ReadLine());

            if (mainmenu == 0)
            {
                Menu();
            }

            //Console.WriteLine("");
            //Console.WriteLine("****** Back to main menu *****");
            //Menu();
        }



        public static void RentMovies()
        {
            //AvailableMovies();

            connection.Open();

            string query = "SELECT * FROM Movies WHERE Age <='" + member.CalculateAge() + "'AND Status = 'Available'";

            SqlCommand comand = new SqlCommand(query, connection);
            SqlDataReader register = comand.ExecuteReader();

            while (register.Read())
            {
                Console.WriteLine("Movie ID: " + register[0].ToString());
                Console.WriteLine("");
                Console.WriteLine("Title: " + register[1].ToString());
                Console.WriteLine("");
                Console.WriteLine("Synopsis: " + register[2].ToString());
                Console.WriteLine("");
                Console.WriteLine("Status: " + register[3].ToString());
                Console.WriteLine("");
                Console.WriteLine ("Age: " + register[4].ToString());
                Console.WriteLine("");
                Console.WriteLine("************************************");
            }

            connection.Close();

            Console.WriteLine("Select the movie you want to rent (Enter the Movie ID)");
            int option = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the title of the movie you want to rent");
            string title = Console.ReadLine();


            connection.Open();

            string query1 = "SELECT * FROM Movies WHERE ID= " + option;

            SqlCommand comand1 = new SqlCommand(query1, connection);
            SqlDataReader register1 = comand1.ExecuteReader();

            if (register1.Read())
            {
                connection.Close();
                connection.Open();

                string query2 = "UPDATE Movies SET Status = 'Not available' WHERE ID = " + option;

                SqlCommand comand2 = new SqlCommand(query2, connection);
                comand2.ExecuteNonQuery();
                connection.Close();


                //put into the rental table

                connection.Open();

                string query3 = "INSERT INTO Rent (IDMember, IDMovie, OutDate) VALUES ("+ member.Id +','+ option +", GETDATE())";

                SqlCommand comand3 = new SqlCommand(query3, connection);
                comand3.ExecuteNonQuery();
                connection.Close();

                connection.Open();

                ////todo insert into members MovieRented
                string query4 = "UPDATE Members SET MoviesRented = '" + title + "' WHERE ID= '" + member.Id + "'";

                SqlCommand comand4 = new SqlCommand(query4, connection);
                comand4.ExecuteNonQuery();
            }

            connection.Close();
            Console.WriteLine("You have successfully rented the movie");
            Console.WriteLine("************************************");
            Menu();

        }





        public static void MyRentals()
        {
            Console.WriteLine("************ MY RENTALS ********************");

            connection.Open();

            string query = "SELECT * FROM Rent WHERE IDMember= " + member.Id;

            SqlCommand comand = new SqlCommand(query, connection);
            SqlDataReader register = comand.ExecuteReader();

            //int id = Convert.ToInt32(register[0].ToString());
            //DateTime outDate = Convert.ToDateTime(register[1].ToString());
            //DateTime returnDate = Convert.ToDateTime(register[2].ToString());
            //int fkMovies = Convert.ToInt32(register[3].ToString());
            //int fkMembers = Convert.ToInt32(register[4].ToString());

            //rent = new Rent(id, outDate, returnDate, fkMovies, fkMembers);

            // We loop through the register and show the movies rented
            while (register.Read()) 
            {
                if (rent.DeadLine()) 
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("");
                    Console.WriteLine("Movie ID: " + register[4].ToString());
                    Console.WriteLine ("Renting date: " + register[1].ToString());
                    Console.WriteLine("");
                    Console.WriteLine("Your rental has expired");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Movie ID " + register[4].ToString());
                    Console.WriteLine ("Renting date: " + register[1].ToString());
                }


                string answer;

                do
                {
                    Console.WriteLine("Do you want to return a movie? Answer Y/N");
                    answer = Console.ReadLine();
                    if (answer.ToUpper() != "Y" || answer.ToUpper() != "N")
                    {
                        Console.WriteLine("Error! Enter Y/N");
                    }
                    else if (answer.ToUpper() == "Y")
                    {
                        ReturnMovie();
                    }
                    else if (answer.ToUpper() == "N")
                    {
                        Menu();
                    }
                } while (answer.ToUpper() != "Y" || answer.ToUpper() != "N");
            }
            connection.Close();
            Menu();

        }

        public static void ReturnMovie()
        {
            Console.WriteLine("Enter the MovieID to return");
            int IDMovie = Convert.ToInt32(Console.ReadLine());

            connection.Open();

            string query = "UPDATE Rent SET ReturnDate = GETDATE() WHERE IDMember= " + member.Id + "AND IDMovie= " + IDMovie + "AND ReturnDate is null";

            SqlCommand comand = new SqlCommand(query, connection);
            comand.ExecuteNonQuery();
            connection.Close();

            connection.Open();

            string query1 = "UPDATE Movies SET Status= 'Available'";

            SqlCommand comand1 = new SqlCommand(query1, connection);
            comand1.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine("You have successfully returned the movie");

            Menu();
        }



    }
}

















