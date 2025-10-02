using laba2.DTO;
using laba2.Models;
using Microsoft.AspNetCore.Mvc;

namespace laba2.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class NotesController : ControllerBase
{
    private static readonly List<Note> _notes = [];

    [HttpGet]
    public IActionResult GetAll()
        => Ok(_notes);

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);

        if (note is null)
        {
            return NotFound();
        }
        
        return Ok(note);
    }

    [HttpPost]
    public IActionResult Create([FromBody] NoteToCreateDto request)
    {
        var id = _notes.Count == 0
            ? 0
            : _notes.Max(n => n.Id) + 1;
        
        var note = new Note(
            id,
            request.Title,
            request.Content);
        
        _notes.Add(note);
        
        return Ok(note);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] NoteToUpdateDto request)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        
        if (note is null)
        {
            return NotFound();
        }
        
        note.Title = request.Title;
        note.Content = request.Content;
        
        return Ok(note);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var idx = _notes.FindIndex(n => n.Id == id);
        
        if (idx < 0)
        {
            return NotFound();
        }
        
        _notes.RemoveAt(idx);
        
        return Ok();
    }
}
