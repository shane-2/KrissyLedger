using Krissy_Ledger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Krissy_Ledger.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        KrissyDbContext dbContext = new KrissyDbContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Ledger(List<DateTime> dates = null)
        {
            List<Transaction> usersList = dbContext.Transactions.Where(p => p.UserName == User.Identity.Name).ToList();
            List<Transaction> transactions = usersList.OrderBy(p => p.AppointmentDate).ToList();
            List<Transaction> result = new List<Transaction>();

            if(dates == null)
            {
                return View(transactions);
            }

            dates = dates.OrderBy(d => d.Date).ToList();

            if (dates.Count == 2)
            {

                foreach (Transaction transaction in transactions)
                {
                    if (transaction.AppointmentDate > dates[0] && transaction.AppointmentDate < dates[1])
                        result.Add(transaction);
                }
            }
            else if (dates.Count == 1)
            {
                foreach (Transaction transaction in transactions)
                {
                    if (transaction.AppointmentDate > dates[0])
                    {
                        result.Add(transaction);
                    }
                }
            }
            return View(result);
        }

        [HttpGet]
        public List<Transaction> MyLedger()
        {
            return dbContext.Transactions.ToList();
        }


        //actual post method below
        [HttpPost]
        
        public IActionResult NewTransaction(Transaction transaction)
        {
            transaction.UserName = User.Identity.Name;
            dbContext.Transactions.Add(transaction);
            dbContext.SaveChanges();
            List<Transaction> updatedTransactions = MyLedger();
            return RedirectToAction("Ledger");
        }

        //Delete method below
        [HttpPost]
        public IActionResult DeleteTransaction(int id)
        {
            Transaction t = dbContext.Transactions.FirstOrDefault(p => p.Id == id);
            dbContext.Transactions.Remove(t);
            dbContext.SaveChanges();
            return RedirectToAction("Ledger");
        }
        //----------------------------------------

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
