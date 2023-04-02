﻿using SoProd.Web.Models;
using SoProd_Testing.Data;
using SoProd_Testing.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SoProd.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            using (var context = new SoProdTestingContext()) {
                List<TestDefinitionViewModel> list = new List<TestDefinitionViewModel>();

                var definitionsList = await context.TestDefinitions.ToListAsync();
                var resultList = await context.TestResults.ToListAsync();

                foreach(var result in definitionsList)
                {
                    TestDefinitionViewModel resultViewModel = new TestDefinitionViewModel();

                    resultViewModel.Id = result.Id;
                    resultViewModel.Name = result.Name;
                    resultViewModel.UserCount = result.UserCount;
                    resultViewModel.Version = result.Version;

                    if (resultViewModel.Version == null)
                        resultViewModel.Version = "N/A";

                    resultViewModel.BaseAddress = result.BaseAddress;
                    resultViewModel.TestResults = new List<TestResultViewModel>();

                    list.Add(resultViewModel);
                }

                foreach (var result in resultList)
                {
                    TestResultViewModel resultViewModel = new TestResultViewModel();

                    resultViewModel.Id = result.TestDefinitionId;
                    resultViewModel.Identifier = result.Identifier;
                    resultViewModel.TimeEllapsed = Math.Round(result.TimeEllapsed, 2);
                    resultViewModel.StartDate = result.StartDate;
                    resultViewModel.Version = result.Version;
                    resultViewModel.RequestsNumber = result.RequestsNumber;
                    resultViewModel.RequestsOK = result.RequestsOK;
                    resultViewModel.RequestsError = result.RequestsError;

                    foreach (var definition in list)
                    {
                        if (definition.Id == result.TestDefinitionId)
                            definition.TestResults.Add(resultViewModel);
                    }
                }

                return View(list);
            }
        }

        //public async Task<JsonResult> GetThemes(int flowId = -1)
        //{
        //    using (var context = new SoProdTestingContext())
        //    {
        //        List<string[]> list = new List<string[]>();
        //        var data = await context.TestResults.Distinct.ToListAsync();

        //        foreach (var item in data)
        //        {
        //            list.Add(new string[] { Languages.Get("All"), item.Version, item.Version });
        //        }

        //        return Json(new
        //        {
        //            data = list
        //        });
        //    }
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}