import { Job } from "../../types";

export default function DisplayJob({ job }: { job: Job}) {
    
    return (
        <>
            <div>{`ID: ${job.id}`}</div>
            <div>{`Start date: ${new Date(job.startDate).toLocaleDateString()}`}</div>
            {/* <div>{`End date: ${new Date(job.endDate).toLocaleDateString()}`}</div> */}
            <div>{`Start time: ${new Date(job.startTime).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit', hour12: false, hourCycle: 'h23'})}`}</div>
            {/* <div>{`End time: ${new Date(job.endTime).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit', hour12: false, hourCycle: 'h23'})}`}</div> */}
            <div>{`Subjects and levels: ${(job.subjectsAndLevels.split("Sec - ")[1]).split(" (T)")[0] ?? "No subject"}`}</div>
            {/* <div>{`Position: ${job.position}`}</div> */}
            <div>{`Location: ${job.location}`}</div>
            <div>{`Teacher: ${job.teacher}`}</div>
            <div>{`Message: ${job.message}`}</div>
            <div>{`Is all day: ${job.isAllDay ? "True" : "False"}`}</div>
        </>
    );
}