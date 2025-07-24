using System.Collections.Generic;
using Dubaicharaja.Models;
using Dubaicharaja.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Dubaicharaja.Controllers
{
    public class ExpenseRecordController : Controller
    {
        private readonly AppDbContext _context;
        public ExpenseRecordController(AppDbContext context)
        {
            _context = context;
        }

        //get expene
        public async Task<IActionResult> Index()
        {
            var records = await _context.ExpenseRecords.ToListAsync();
            ViewBag.TotalBalance = records.Where(x => x.Type == "Income").Sum(x => x.Amount) -
                 records.Where(x => x.Type == "Expense").Sum(x => x.Amount);
            return View(records);
        }

        //get exprec/create 
        public IActionResult Create()
        {
            return View();
        }

        //post : exprec/cre
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseRecord record)
        {
            if (ModelState.IsValid)
            {
                record.Date = DateTime.Now;
                _context.Add(record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(record);
        }

        //get exp/edit by id 
        public async Task<IActionResult> Edit (int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            var record = await _context.ExpenseRecords.FindAsync(id);
            if (record == null) return NotFound();

            return View(record);
        }

        //post exp/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ExpenseRecord record)
        {
            if (id != record.Id) return NotFound();

            if(ModelState.IsValid)
            {
                _context.Update(record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(record);
        }

        //get del exp
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var record = await _context.ExpenseRecords.FirstOrDefaultAsync(m => m.Id == id);
            if (record == null) return NotFound();

            return View(record);
        }

        //post del exp
        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var record = await _context.ExpenseRecords.FindAsync(id);
            _context.ExpenseRecords.Remove(record);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Dashboard(int? Year)
        {
            var currentYear = Year ?? DateTime.Now.Year;

            var records = await _context.ExpenseRecords
                .Where (e => e.Date.Year == currentYear)
                .ToListAsync();

            var totalIncome = records
                .Where(e => e.Type == "Income")
                .Sum(e => e.Amount);

            var totalExpense = records
                .Where(e => e.Type == "Expense")
                .Sum(e => e.Amount);

            ViewBag.SelectedYear = currentYear;
            ViewBag.TotalIncome = totalIncome;
            ViewBag.TotalExpense = totalExpense;
            ViewBag.TotalBalance = totalIncome - totalExpense;
            ViewBag.Years = _context.ExpenseRecords.Select(e => e.Date.Year).Distinct().OrderByDescending(y => y).ToList();

            return View(records);
        }
    }
}
