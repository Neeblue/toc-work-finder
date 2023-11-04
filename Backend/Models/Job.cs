namespace Backend.Models;

public class Job
{

    public string? Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? SubjectsAndLevels { get; set; }
    public string? Position { get; set; }
    public string? Location { get; set; }
    public string? Teacher { get; set; }
    public string? Message { get; set; }

    public bool IsAllDay => EndTime - StartTime >= TimeSpan.FromHours(6);

    public override string ToString()
    {
        return $"Id: {Id}\n" +
               (StartDate == EndDate ? $"Date: {StartDate:ddd, dd MMM yyyy}\n" : 
               $"Start date: {StartDate:ddd, dd MMM yyyy}\n" +
               $"End date: {EndDate:ddd, dd MMM yyyy}\n") +
               $"Start  time: {StartTime.ToShortTimeString()}\n" +
               //$"End time: {EndTime.ToShortTimeString()}\n" +
               $"Subjects and levels: {SubjectsAndLevels}\n" +
               //$"Position: {Position}\n" +
               $"Location: {Location}\n" +
               $"Teacher: {Teacher}\n" +
               $"Message: {Message}\n" +
               $"Is all day: {IsAllDay} ({Math.Round((EndTime - StartTime).TotalHours, 1)} hours)";
    }
}