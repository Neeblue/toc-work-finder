import { useState, useEffect, useRef } from 'react';
import { Job } from '../types';
import DisplayAllJobs from './components/DisplayAllJobs';
import Collapsible from 'react-collapsible';
import "@mantine/core/styles.css";
import { MantineProvider, Button, Slider } from '@mantine/core';
import { theme } from "./theme";
import './index.css';

export default function App() {

  // Fetch states
  const [jobs, setJobs] = useState<Job[]>([]);
  const [filteredJobs, setFilteredJobs] = useState<Job[]>([]);

  // UI States
  const [slider, setSlider] = useState(1.5); // [0, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5] etc.
  const [checkboxAllDay, setCheckboxAllDay] = useState(false);
  const [isStartClicked, setIsStartClicked] = useState(false);

  // School checkbox states
  const [checkboxFrankHurt, setCheckboxFrankHurt] = useState(true);
  const [checkboxSemiahmoo, setCheckboxSemiahmoo] = useState(true);
  const [checkboxElginPark, setCheckboxElginPark] = useState(true);
  const [checkboxNorthSurrey, setCheckboxNorthSurrey] = useState(true);
  const [checkboxFleetwoodPark, setCheckboxFleetwoodPark] = useState(true);
  const [checkboxFraserHeights, setCheckboxFraserHeights] = useState(true);
  const [checkboxGuildfordPark, setCheckboxGuildfordPark] = useState(true);
  const [checkboxPanoramaRidge, setCheckboxPanoramaRidge] = useState(true);
  const [checkboxQueenElizabeth, setCheckboxQueenElizabeth] = useState(true);
  const [checkboxClaytonHeights, setCheckboxClaytonHeights] = useState(true);
  const [checkboxJohnstonHeights, setCheckboxJohnstonHeights] = useState(true);
  const [checkboxSullivanHeights, setCheckboxSullivanHeights] = useState(true);
  const [checkboxGrandviewHeights, setCheckboxGrandviewHeights] = useState(true);

  // Fetch on an interval. Interval ID is stored in a variable so that it can be cancelled.
  const intervalId = useRef<number | null>(null);

  const Fetch = () => {
    console.log("Fetching jobs.");
    fetch('https://super-train-q5pvvg6qp5wc9wr7-5191.app.github.dev/api/Jobs')
      // fetch('http://localhost:5191/api/Jobs')
      .then(response => response.json())
      .then(data => setJobs(data));
  };

  // Fetch jobs after the desired number of minutes
  function FetchOnInterval(minutes: number) {
    intervalId.current = setInterval(() => {
      console.log("Fetching jobs.");
      Fetch();
    }, minutes * 60 * 1000);
  }

  // Cancel the interval job fetching
  function Cancel() {
    if (intervalId.current !== null) {
      window.clearInterval(intervalId.current);
      intervalId.current = null;
      console.log("Cancelled job search.");
    }
  }

  //Filter jobs based on which checkboxes have been selected
  useEffect(() => {
    const filtered: Job[] = jobs.filter(job => {
      if (checkboxAllDay && !job.isAllDay) return false;
      if (checkboxQueenElizabeth && job.location === "Queen Elizabeth Secondary") return true;
      if (checkboxFleetwoodPark && job.location === "Fleetwood Park Secondary") return true;
      if (checkboxNorthSurrey && job.location === "North Surrey Secondary") return true;
      if (checkboxJohnstonHeights && job.location === "Johnston Heights Secondary") return true;
      if (checkboxFraserHeights && job.location === "Fraser Heights Secondary") return true;
      if (checkboxClaytonHeights && job.location === "Clayton Heights Secondary") return true;
      if (checkboxGuildfordPark && job.location === "Guildford Park Secondary") return true;
      if (checkboxFrankHurt && job.location === "Frank Hurt Secondary") return true;
      if (checkboxGrandviewHeights && job.location === "Grandview Heights Secondary") return true;
      if (checkboxSemiahmoo && job.location === "Semiahmoo Secondary") return true;
      if (checkboxElginPark && job.location === "Elgin Park Secondary") return true;
      if (checkboxPanoramaRidge && job.location === "Panorama Ridge Secondary") return true;
      if (checkboxSullivanHeights && job.location === "Sullivan Heights Secondary") return true;
      return false;
    });
    setFilteredJobs(filtered);
  }, [jobs, checkboxAllDay, checkboxQueenElizabeth, checkboxFleetwoodPark, checkboxNorthSurrey, checkboxJohnstonHeights, checkboxFraserHeights, checkboxClaytonHeights, checkboxGuildfordPark, checkboxFrankHurt, checkboxGrandviewHeights, checkboxSemiahmoo, checkboxElginPark, checkboxPanoramaRidge, checkboxSullivanHeights]);

  // Checkbox object to decrease the size of the 
  const checkboxes = [
    { label: 'Show Queen Elizabeth jobs', checked: checkboxQueenElizabeth, setChecked: setCheckboxQueenElizabeth },
    { label: 'Show Fleetwood jobs', checked: checkboxFleetwoodPark, setChecked: setCheckboxFleetwoodPark },
    { label: 'Show North Surrey jobs', checked: checkboxNorthSurrey, setChecked: setCheckboxNorthSurrey },
    { label: 'Show Johnston Heights jobs', checked: checkboxJohnstonHeights, setChecked: setCheckboxJohnstonHeights },
    { label: 'Show Fraser Heights jobs', checked: checkboxFraserHeights, setChecked: setCheckboxFraserHeights },
    { label: 'Show Clayton Heights jobs', checked: checkboxClaytonHeights, setChecked: setCheckboxClaytonHeights },
    { label: 'Show Guildford Park jobs', checked: checkboxGuildfordPark, setChecked: setCheckboxGuildfordPark },
    { label: 'Show Frank Hurt jobs', checked: checkboxFrankHurt, setChecked: setCheckboxFrankHurt },
    { label: 'Show Grandview Heights jobs', checked: checkboxGrandviewHeights, setChecked: setCheckboxGrandviewHeights },
    { label: 'Show Semiahmoo jobs', checked: checkboxSemiahmoo, setChecked: setCheckboxSemiahmoo },
    { label: 'Show Elgin Park jobs', checked: checkboxElginPark, setChecked: setCheckboxElginPark },
    { label: 'Show Panorama Ridge jobs', checked: checkboxPanoramaRidge, setChecked: setCheckboxPanoramaRidge },
    { label: 'Show Sullivan Heights jobs', checked: checkboxSullivanHeights, setChecked: setCheckboxSullivanHeights },
  ];

  return (
    <MantineProvider theme={theme}>
      <div className="p-4 flex flex-col gap-4 mt-4">

        {/* Slider */}
        <div className='flex mt-2'>
          <label className='w-[12rem]'>
            <Slider
              defaultValue={1.5}
              min={1.5}
              max={10}
              step={0.5}
              label={(value: number) => `${value} mins`}
              onChangeEnd={(e: number) => setSlider(e)}
              showLabelOnHover
              color='teal'
              radius='md'
            />
            <span>{slider.toString()} minutes</span>
          </label>
        </div>

        {/* Start and stop buttons */}
        <div className="flex">
          <Button disabled={isStartClicked} className='bg-blue-400 mr-2' onClick={() => {
            setIsStartClicked(true);
            Fetch();
            FetchOnInterval(slider);
          }}>Start</Button>
          <Button disabled={!isStartClicked} className='bg-blue-400' onClick={() => {
            Cancel();
            setIsStartClicked(false);
          }}>Stop</Button>
        </div>

        {/* All day checkbox */}
        <div>
          <label className="flex items-center">
            <input type="checkbox" checked={checkboxAllDay} onChange={() => setCheckboxAllDay(!checkboxAllDay)} />
            <span className='ml-2'>Filter all day jobs only</span>
          </label>
        </div>

        {/* Schools checkboxes */}
        <Collapsible trigger="Toggle schools >">
          <div>
            {checkboxes.map((checkbox, index) => (
              <div key={index} className=''>
                <label className="flex items-center">
                  <input type="checkbox" checked={checkbox.checked} onChange={() => checkbox.setChecked(!checkbox.checked)} />
                  <span className="ml-4">{checkbox.label}</span>
                </label>
              </div>
            ))}
          </div>
        </Collapsible>

        {/* Display the jobs that have not been filtered out */}
        <DisplayAllJobs jobs={filteredJobs} />

      </div>
    </MantineProvider>
  );
}