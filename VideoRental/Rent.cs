using System;
namespace VideoRental
{
    public class Rent
    {
        public int Id{ get; set; }
        public DateTime OutDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int FkMovies { get; set; }
        public int FkMembers { get; set; }

        public Rent(int id, DateTime outdate, DateTime returnDate, int fkMovies, int fkMembers)
        {
            Id = id;
            OutDate = outdate;
            ReturnDate = returnDate;
            FkMovies = fkMovies;
            FkMembers = fkMembers;
        }

        public bool DeadLine()
        {
            if (DateTime.Today > OutDate.AddDays(3))
            {

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
