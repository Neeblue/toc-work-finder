import DisplayJob from './DisplayJob';
import { Job } from '../../types';

export default function Display({ jobs }: { jobs: Job[] }) {

	if (!jobs) {
		return <h1>Loading</h1>;
	}

	return (
		<div className='text-left flex gap-4 flex-wrap'>
			{jobs.map((job) => (
				<div key={job.id} className='mb-4 w-[90vw] max-w-[90vw] sm:max-w-[25rem] p-4 border border-black bg-gray-100'><DisplayJob job={job} /></div>
			))}
		</div>
	);
}