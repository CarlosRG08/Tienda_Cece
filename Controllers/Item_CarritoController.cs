using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda_Cece.Models;

namespace Tienda_Cece.Controllers
{
    public class Item_CarritoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Item_CarritoController(ApplicationDbContext context)
        {
            _context = context;
        }

       
    }
}
