﻿using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WanderQuest.ViewModels;

namespace WanderQuest.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public IActionResult Chat()
        {
            return View();
        }
    }
}
