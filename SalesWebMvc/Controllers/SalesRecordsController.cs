﻿using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            if (!maxDate.HasValue)
                maxDate = DateTime.Now;
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            try
            {
                if (!minDate.HasValue)
                    minDate = new DateTime(DateTime.Now.Year, 1, 1);
                if (!maxDate.HasValue)
                    maxDate = DateTime.Now;
                ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
                ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
                var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
                return View(result);
            }
            catch
            {
                string message = "Página temporariamente indisponível.";
                return RedirectToAction(nameof(Error), new { message = message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}