﻿using Microsoft.AspNetCore.Mvc;

namespace Radikool6.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}