using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc.Data;
using mvc.Models;

[Authorize(Roles = "Teacher")]
public class EventsController : Controller
{
    private readonly ApplicationDbContext _context;

    public EventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var events = _context.Events.ToList();
        return View(events);
    }

    public IActionResult Details(int id)
    {
        var eventItem = _context.Events.FirstOrDefault(e => e.Id == id);
        if (eventItem == null)
            return NotFound();

        return View(eventItem);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Event eventItem)
    {
        if (ModelState.IsValid)
        {
            _context.Events.Add(eventItem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(eventItem);
    }

    public IActionResult Edit(int id)
    {
        var eventItem = _context.Events.FirstOrDefault(e => e.Id == id);
        if (eventItem == null)
            return NotFound();

        return View(eventItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Event eventItem)
    {
        if (id != eventItem.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Events.Update(eventItem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(eventItem);
    }

    public IActionResult Delete(int id)
    {
        var eventItem = _context.Events.FirstOrDefault(e => e.Id == id);
        if (eventItem == null)
            return NotFound();

        return View(eventItem);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var eventItem = _context.Events.FirstOrDefault(e => e.Id == id);
        if (eventItem != null)
        {
            _context.Events.Remove(eventItem);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}
