using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpaceJam2.Models;

namespace SpaceJam2.Controllers
{
    public class GameController : Controller
    {
        private readonly SpaceJamDAL _SpaceJamDAL;
        
        private readonly SpaceJamContext _context;


        public GameController(SpaceJamContext context)
        {
            
            _SpaceJamDAL = new SpaceJamDAL();
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
