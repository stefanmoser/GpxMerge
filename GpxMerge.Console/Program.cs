using System;
using System.IO;

namespace GpxMerge.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                System.Console.WriteLine("Starting . . .");

                var outputFileName = @"C:\Users\stefan.moser\Google Drive\Bike\GPS\GPS-HR Merge\May01New.gpx";
                File.Delete(outputFileName);

                var merger = new GpxMerger();

                var stravaFile = new FileInfo(@"C:\Users\stefan.moser\Google Drive\Bike\GPS\GPS-HR Merge\May01Android.orig.gpx");
                var garminFile = new FileInfo(@"C:\Users\stefan.moser\Google Drive\Bike\GPS\GPS-HR Merge\May01Garmin.orig.gpx");

                merger.MergeFiles(stravaFile, garminFile, outputFileName);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }

            System.Console.WriteLine("All done!");
            System.Console.ReadLine();
        }
    }
}