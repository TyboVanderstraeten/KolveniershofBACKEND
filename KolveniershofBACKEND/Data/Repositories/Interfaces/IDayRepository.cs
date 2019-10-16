﻿using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IDayRepository
    {
        IEnumerable<Day> GetAll();
        IEnumerable<Day> GetAllByWeek(int weekNr);
        Day GetById(int id);
        Day GetByWeekAndDay(int weekNr, int dayNr);
        void Add(Day day);
        void Remove(Day day);
        void SaveChanges();
    }
}
