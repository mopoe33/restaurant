using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Models;
using System.Web;

namespace restaurant.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
         
        private readonly AppDbContext _context;
       
        private readonly UserManager<Users> _userManager;
        

        public ReservationsController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> BookTable(int id)
        {

            var Table = await _context.Tables.Where(e => e.Id == id).SingleOrDefaultAsync();

            if (Table != null)
            {
                var bookingTable = new BookingTable
                {
                    TableId = Table.Id
                };
               
                return View("BookTable", bookingTable);
            }

            return NotFound();
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        public async Task<IActionResult> BookTable(BookingTable b)
        {

            Console.WriteLine($"Start: {b.bookingDateStart}, End: {b.BookingDateEnd}");
            if (User.Identity.IsAuthenticated)
            {
                string userId = _userManager.GetUserId(User);

                var existingReservation = await _context.BookingTables.Where(bt => bt.TableId == b.TableId && bt.bookingDateStart < b.BookingDateEnd && bt.BookingDateEnd > b.bookingDateStart).FirstOrDefaultAsync();


                if (existingReservation != null)
                {
                    return Content("La table est deja reserve pour cette date.");
                }

                if (b.bookingDateStart == DateTime.MinValue)
                {
                    return Content("date invalide.");
                }

                var bookingTimeStart = b.bookingDateStart.TimeOfDay;
                var BookingTimeEnd = b.BookingDateEnd.TimeOfDay;

                Console.WriteLine(bookingTimeStart + " " + BookingTimeEnd);

                var availableServeurs = await _context.Serveurs.Where(e => e.timeStart <= bookingTimeStart && e.timeEnd >= BookingTimeEnd).ToListAsync();

                if (!availableServeurs.Any())
                {
                    return Content("aucun serveur est disponible");
                }

                var selectedServeur = availableServeurs.FirstOrDefault();
                if (selectedServeur != null)
                {
                    var bookingTable = new BookingTable
                    {
                        TableId = b.TableId,
                        bookingDateStart = b.bookingDateStart,
                        BookingDateEnd=b.BookingDateEnd,
                        Status = "En cours", 
                        serveurId = selectedServeur.Id,
                        ClientId = userId
                    };

                    _context.Add(bookingTable);

                    

                    await _context.SaveChangesAsync();

                    return RedirectToAction("IndexUser", "Tables");
                }

                return Content("aucun serveur est disponible.");
            }

            return Unauthorized("vous etes pas connecte");
        }



        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return View(await _context.BookingTables.ToArrayAsync());
            else
            {
                string UserId = _userManager.GetUserId(User);

                var Orders = await _context.BookingTables.Where(e => e.ClientId == UserId).ToListAsync();

                return View("Index", Orders);
            }
        }

        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> Details(int id)
       {

   var reservation = await(from bt in _context.BookingTables join table in _context.Tables on bt.TableId equals table.Id
                             join serveur in _context.Serveurs on bt.serveurId equals serveur.Id
                             join Users in _context.Users on bt.ClientId equals Users.Id
                           where bt.Id == id
                           select new{
                       bt.Id,
                      TableId = table.Id,
                      personNumber = table.PeopleNumber,
                      ServeurName = serveur.firstName + " " + serveur.lastName,
                       bt.bookingDateStart,
                       bt.BookingDateEnd,
                      bt.Status,
                      ClientName = Users.FirstName + " " + Users.LastName,
                      ClientPhone = Users.PhoneNumber,
                      ClientEmail = Users.Email,}).FirstOrDefaultAsync();

                if (reservation == null)
                {
                    return NotFound(); 
                }
                return View(reservation);
            }
    }
}
