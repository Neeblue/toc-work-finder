using Backend.Interfaces;
using Backend.Models;
using Backend.Configurations;

namespace Backend.Services;

public class JobFilterService : IJobFilterService
{
    public List<string> oldIds = new();

    /// <summary>
    /// Removes unwanted jobs from the list of scraped jobs.
    /// </summary>
    /// <param name="jobs"></param>
    /// <returns></returns>
    public List<Job> FilterJobs(List<Job> jobs)
    {        
        jobs = jobs
            .Where(job => job.Id is not null && !oldIds.Contains(job.Id))
            // .Where(job => job.IsAllDay)
            // .Where(job => job.Location is not null && Secrets.GoodSchools.Any(goodSchool => job.Location.Contains(goodSchool)))
            // .Where(job => job.SubjectsAndLevels is not null && Secrets.GoodSubjects.Any(goodSubject => job.SubjectsAndLevels.Contains(goodSubject)))
            // .Where(job => job.Teacher is not null && !Secrets.BadTeachers.Any(badTeacher => job.Teacher.Equals(badTeacher)))
            .ToList();
        
        foreach (var job in jobs)
        {
            if (job.Id is not null)
                oldIds.Add(job.Id);
        }

        return jobs;
    }
}