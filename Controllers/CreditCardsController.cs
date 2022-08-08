using CreditCards.Data;
using CreditCards.Models;
using CreditCardValidator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreditCards.Controllers
{
    public class CreditCardsController : Controller
    {
        private readonly CreditCardsContext _context;

        public CreditCardsController(CreditCardsContext context)
        {
            _context = context;
        }

        // GET: CreditCards
        public async Task<IActionResult> Index()
        {
            return _context.CreditCard != null ?
                        View(await _context.CreditCard.ToListAsync()) :
                        Problem("Entity set 'CreditCardsContext.CreditCard'  is null.");
        }

        // GET: CreditCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CreditCard == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCard
                .FirstOrDefaultAsync(m => m.Id == id);
            if (creditCard == null)
            {
                return NotFound();
            }

            return View(creditCard);
        }

        // GET: CreditCards/Create 
        public IActionResult Create()
        {
            return View();
        }

        // POST: CreditCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CardNumber,CardName,CardCvv,CardType,CardExpDate")] CreditCard creditCard)
        {
            string errorMsg = string.Empty;
            if (ModelState.IsValid)
            {
                bool isValidFlag = false;

                if (Luhn.CheckLuhn(creditCard.CardNumber))
                    if (!(_context.CreditCard?.Any(e => e.CardNumber == creditCard.CardNumber)).GetValueOrDefault())
                        isValidFlag = true;
                    else
                        errorMsg = "Credit card Number already exist";
                else
                    errorMsg = "Credit card Number not valid";

                if (isValidFlag)
                {
                    _context.Add(new CreditCard(creditCard.Id, creditCard.CardNumber, creditCard.CardName, creditCard.CardCvv, creditCard.CardNumber.CreditCardBrandName(), creditCard.CardExpDate));
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ModelState.AddModelError(string.Empty, errorMsg);
            return View(creditCard);
        }

        // GET: CreditCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CreditCard == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCard.FindAsync(id);
            if (creditCard == null)
            {
                return NotFound();
            }
            return View(creditCard);
        }

        // POST: CreditCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CardNumber,CardName,CardCvv,CardType,CardExpDate")] CreditCard creditCard)
        {
            string errorMsg = string.Empty;
            if (id != creditCard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool isValidFlag = false;
                    if (Luhn.CheckLuhn(creditCard.CardNumber))
                        if (!(_context.CreditCard?.Any(e => e.CardNumber == creditCard.CardNumber && e.Id != creditCard.Id)).GetValueOrDefault())
                            isValidFlag = true;
                        else
                            errorMsg = "Credit card Number already exist";
                    else
                        errorMsg = "Credit card Number not valid";

                    if (isValidFlag)
                    {
                        _context.Update(new CreditCard(creditCard.Id, creditCard.CardNumber, creditCard.CardName, creditCard.CardCvv, creditCard.CardNumber.CreditCardBrandName(), creditCard.CardExpDate));
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, errorMsg);
                        return View(creditCard);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditCardExists(creditCard.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, errorMsg);
            return View(creditCard);
        }

        // GET: CreditCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CreditCard == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCard
                .FirstOrDefaultAsync(m => m.Id == id);
            if (creditCard == null)
            {
                return NotFound();
            }

            return View(creditCard);
        }

        // POST: CreditCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CreditCard == null)
            {
                return Problem("Entity set 'CreditCardsContext.CreditCard'  is null.");
            }
            var creditCard = await _context.CreditCard.FindAsync(id);
            if (creditCard != null)
            {
                _context.CreditCard.Remove(creditCard);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditCardExists(int id)
        {
            return (_context.CreditCard?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
