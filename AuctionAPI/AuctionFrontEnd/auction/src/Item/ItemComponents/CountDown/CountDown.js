import React from 'react';

import './CountDown.scss';

export function CountDown(props) {
    const [counter, setCounter] = React.useState(props.countDown.seconds);
    const [minuteCounter, setMinuteCounter] = React.useState(props.countDown.minutes);
    const [hourCounter, setHourCounter] = React.useState(props.countDown.hours);
    const [dayCounter, setDayCounter] = React.useState(props.countDown.days);
  
    React.useEffect(() => {
      if(counter <= 0 && ( hourCounter !== 0 || minuteCounter !== 0 || dayCounter !== 0) ){
        if(hourCounter === 0 && minuteCounter === 0){
            setDayCounter(dayCounter-1);
            setHourCounter(23);
            setMinuteCounter(59);
            setCounter(59);
        }
        else if(minuteCounter === 0){
            setHourCounter(hourCounter-1);
            setMinuteCounter(59);
            setCounter(59);
        }
        else{
            setMinuteCounter(minuteCounter-1);
            setCounter(59);
        }
        
      }
      if(counter <= 0 && hourCounter === 0 && minuteCounter === 0 && dayCounter === 0) console.log("timer is dead")
      const timer = counter > 0 && setInterval(() => setCounter(counter - 1), 1000);
      return () =>{
        clearInterval(timer);
      } 
    }, [counter]);
  
    return (
      <div className="countdown">
          <h5 className="mb-2 text-center">Remaining Time</h5>
          <ul className="pagination">
              <li className="page-item">{dayCounter}</li>
              <li className="page-item">{hourCounter}</li>
              <li className="page-item">{minuteCounter}</li>
              <li className="page-item active">{counter}</li>
          </ul>
      </div>
    );
  }