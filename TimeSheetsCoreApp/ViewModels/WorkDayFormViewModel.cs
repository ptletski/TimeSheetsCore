using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TimeSheetsCoreApp.DataTypes;
using TimeSheetsCoreApp.Models;

namespace TimeSheetsCoreApp.ViewModels
{
    public class WorkDayFormViewModel
    {
        public WorkDayFormViewModel()
        {
        }

        public WorkDayFormViewModel(WorkDayFormViewModel model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            UserId = model.UserId;
            Date = model.Date;
            StartTime = model.StartTime;
            EndTime = model.EndTime;
            BreakTime = model.BreakTime;
            Rate = model.Rate;
        }

        public WorkDayFormViewModel(UserDetails userDetails)
        {
            FirstName = userDetails.FirstName;
            LastName = userDetails.LastName;
            UserId = userDetails.UserId;
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string EndTime { get; set; }

        [Required]
        public short BreakTime { get; set; }

        public IEnumerable<BreakDuration> GetBreakTimeList()
        {
            short[] durations = {
                15,
                30,
                45,
                60,
                75,
                90
            };
            
            var list = new List<BreakDuration>();
            byte id = 1;

            foreach(short duration in durations)
            {
                list.Add(new BreakDuration() { Duration = duration, DurationId = id });
                id++;
            }

            return list;
        }

        public byte BreakTimeId { get; set; }

        public WorkRecordRate Rate { get; set; }

        public IEnumerable<RateDisplay> GetRatesList()
        {
            //public enum WorkRecordRate { Standard, Holiday, Overtime, DoubleOvertime };

            RateDisplay[] rates = {
                new RateDisplay() {Rate="Standard", RateId=0},
                new RateDisplay() {Rate="Holiday", RateId=1},
                new RateDisplay() {Rate="Overtime", RateId=2},
                new RateDisplay() {Rate="Double Overtime", RateId=3}
            };

            return rates;
        }
    }
}
