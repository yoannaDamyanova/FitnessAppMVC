﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Web.ViewModels.FitnessClass
{
    public class FitnessClassCancelViewModel
    {
        public Guid Id { get; set; } 

        public string Title { get; set; } = string.Empty;

        public string StartTime { get; set; } = string.Empty;
    }
}
