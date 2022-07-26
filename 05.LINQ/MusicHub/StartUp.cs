namespace MusicHub
{
    using System;
    using System.Linq;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            /*
             * Export all albums which are produced by the provided Producer Id
             * For each Album, get the Name, Release date in format the "MM/dd/yyyy",
             * Producer Name, the Album Songs with each Song Name, Price (formatted to the second digit)
             * and the Song Writer Name. Sort the Songs by Song Name (descending) and by Writer (ascending).
             * At the end export the Total Album Price with exactly two digits after the decimal place.
             * Sort the Albums by their Total Price (descending).
             */
            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new 
                { 
                    a.Name,
                    a.ReleaseDate,
                    a.P
                })
                .ToList();

            return albums[0].Name;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
