using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WaterLogger.Models;
using WaterLogger.Data;

namespace WaterLogger.Pages.DailyCalories;
public class EditModel(DrinkingWaterContext context) : PageModel
{
    private readonly DrinkingWaterContext _context = context;

    [BindProperty]
    public DailyCaloriesModel DailyCalories { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var dailyCalories =  await _context.DailyCalories.FirstOrDefaultAsync(m => m.Id == id);
        if (dailyCalories == null)
            return NotFound();
        
        DailyCalories = dailyCalories;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        _context.Attach(DailyCalories).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DailyCaloriesExists(DailyCalories.Id))
                return NotFound();
            else
                throw;
        }

        return RedirectToPage("./Index");
    }

    private bool DailyCaloriesExists(int id)
    {
        return _context.DailyCalories.Any(e => e.Id == id);
    }
}

