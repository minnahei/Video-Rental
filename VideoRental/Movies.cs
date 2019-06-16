using System;
namespace VideoRental
{
    public class Movies
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Status { get; set; }
        public double Age { get; set; }

        public Movies(int iD, string title, string synopsis, string status, double age)
        {
            ID = iD;
            Title = title;
            Synopsis = synopsis;
            Status = status;
            Age = age;
        }

      
    }
}
